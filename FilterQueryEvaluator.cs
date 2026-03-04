namespace FilterQueryDemo;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

internal static class FilterQueryEvaluator
{
    public static bool Evaluate(Query query, IReadOnlyDictionary<string, object> context)
    {
        return EvaluateExpr(query.Expr, context);
    }

    private static bool EvaluateExpr(Expr expr, IReadOnlyDictionary<string, object> context)
    {
        return EvaluateOrExpr(expr.OrExpr, context);
    }

    private static bool EvaluateOrExpr(OrExpr expr, IReadOnlyDictionary<string, object> context)
    {
        var value = EvaluateAndExpr(expr.AndExpr, context);
        foreach (var item in expr.OrExprList)
        {
            value = value || EvaluateAndExpr(item.AndExpr, context);
        }

        return value;
    }

    private static bool EvaluateAndExpr(AndExpr expr, IReadOnlyDictionary<string, object> context)
    {
        var value = EvaluateUnaryExpr(expr.UnaryExpr, context);
        foreach (var item in expr.AndExprList)
        {
            value = value && EvaluateUnaryExpr(item.UnaryExpr, context);
        }

        return value;
    }

    private static bool EvaluateUnaryExpr(UnaryExpr expr, IReadOnlyDictionary<string, object> context)
    {
        return expr switch
        {
            UnaryExprNotOpUnaryExprVariant negated => !EvaluateUnaryExpr(negated.Value.UnaryExpr, context),
            UnaryExprPrimaryVariant primary => EvaluatePrimary(primary.Value.Primary, context),
            _ => throw new InvalidOperationException($"Unsupported unary expression node '{expr.GetType().Name}'."),
        };
    }

    private static bool EvaluatePrimary(Primary primary, IReadOnlyDictionary<string, object> context)
    {
        return primary switch
        {
            PrimaryComparisonVariant comparison => EvaluateComparison(comparison.Value.Comparison, context),
            PrimaryOperandVariant operand => ToBoolean(EvaluateOperand(operand.Value.Operand, context)),
            PrimaryLParenExprRParenVariant grouped => EvaluateExpr(grouped.Value.Expr, context),
            _ => throw new InvalidOperationException($"Unsupported primary node '{primary.GetType().Name}'."),
        };
    }

    private static bool EvaluateComparison(Comparison comparison, IReadOnlyDictionary<string, object> context)
    {
        var left = EvaluateOperand(comparison.Operand, context);
        var right = EvaluateOperand(comparison.Operand0, context);
        var op = comparison.CompareOp.CompareOpValue.Text;
        return CompareValues(left, right, op);
    }

    private static object EvaluateOperand(Operand operand, IReadOnlyDictionary<string, object> context)
    {
        return operand switch
        {
            OperandIdentifierVariant identifier => ResolveIdentifier(identifier.Value.Identifier.Text, context),
            OperandStringLitVariant stringLit => ParseStringLiteral(stringLit.Value.StringLit.Text),
            OperandNumberLitVariant numberLit => ParseNumberLiteral(numberLit.Value.NumberLit.Text),
            OperandBoolLitVariant boolLit => EvaluateBoolLiteral(boolLit.Value.BoolLit),
            _ => throw new InvalidOperationException($"Unsupported operand node '{operand.GetType().Name}'."),
        };
    }

    private static object ResolveIdentifier(string name, IReadOnlyDictionary<string, object> context)
    {
        if (context.TryGetValue(name, out var resolved))
        {
            return resolved;
        }

        throw new InvalidOperationException($"Unknown identifier '{name}' in evaluation context.");
    }

    private static string ParseStringLiteral(string raw)
    {
        return JsonSerializer.Deserialize<string>(raw) ?? raw;
    }

    private static double ParseNumberLiteral(string raw)
    {
        return double.Parse(raw, CultureInfo.InvariantCulture);
    }

    private static bool EvaluateBoolLiteral(BoolLit boolLit)
    {
        return boolLit switch
        {
            BoolLitTrueVariant => true,
            BoolLitFalseVariant => false,
            _ => throw new InvalidOperationException($"Unsupported bool literal node '{boolLit.GetType().Name}'."),
        };
    }


    private static bool TryGetNumber(object value, out double number)
    {
        switch (value)
        {
            case double d:
                number = d;
                return true;
            case float f:
                number = f;
                return true;
            case int i:
                number = i;
                return true;
            case long l:
                number = l;
                return true;
            case decimal m:
                number = (double)m;
                return true;
            case string s when double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed):
                number = parsed;
                return true;
            default:
                number = 0;
                return false;
        }
    }

    private static bool CompareValues(object left, object right, string op)
    {
        if (TryGetNumber(left, out var leftNumber) && TryGetNumber(right, out var rightNumber))
        {
            return CompareComparableValue(leftNumber, rightNumber, op);
        }

        if (left is bool leftBool && right is bool rightBool)
        {
            return CompareComparableValue(leftBool, rightBool, op);
        }

        var leftText = Convert.ToString(left, CultureInfo.InvariantCulture) ?? string.Empty;
        var rightText = Convert.ToString(right, CultureInfo.InvariantCulture) ?? string.Empty;

        var cmp = string.Compare(leftText, rightText, StringComparison.OrdinalIgnoreCase);
        return op switch
        {
            "=" or "==" => cmp == 0,
            "!=" => cmp != 0,
            "<" => cmp < 0,
            "<=" => cmp <= 0,
            ">" => cmp > 0,
            ">=" => cmp >= 0,
            _ => throw new InvalidOperationException($"Unsupported comparison operator '{op}'."),
        };
    }

    private static bool CompareComparableValue<T>(T left, T right, string op) where T : IComparable<T>
    {
        var cmp = left.CompareTo(right);
        return op switch
        {
            "=" or "==" => cmp == 0,
            "!=" => cmp != 0,
            "<" => cmp < 0,
            "<=" => cmp <= 0,
            ">" => cmp > 0,
            ">=" => cmp >= 0,
            _ => throw new InvalidOperationException($"Unsupported comparison operator '{op}'."),
        };
    }

    private static bool ToBoolean(object value)
    {
        if (value is bool b)
        {
            return b;
        }

        if (value is string s)
        {
            if (bool.TryParse(s, out var parsedBool))
            {
                return parsedBool;
            }

            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedNumber))
            {
                return Math.Abs(parsedNumber) > double.Epsilon;
            }

            return !string.IsNullOrWhiteSpace(s);
        }

        if (TryGetNumber(value, out var number))
        {
            return Math.Abs(number) > double.Epsilon;
        }

        throw new InvalidOperationException($"Value of type '{value.GetType().Name}' cannot be used as boolean operand.");
    }

}
