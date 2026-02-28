namespace FilterQueryDemo;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

internal static class FilterQueryEvaluator
{
    public static bool Evaluate(string input, IReadOnlyDictionary<string, object> context)
    {
        var parser = new Parser(input, context);
        return parser.Parse();
    }

    private enum TokenKind
    {
        Identifier,
        String,
        Number,
        Boolean,
        And,
        Or,
        Not,
        LParen,
        RParen,
        Eq,
        Ne,
        Lt,
        Le,
        Gt,
        Ge,
        End,
    }

    private readonly record struct Token(TokenKind Kind, string Text, object? Literal = null);

    private sealed class Parser
    {
        private readonly IReadOnlyDictionary<string, object> _context;
        private readonly List<Token> _tokens;
        private int _position;

        public Parser(string input, IReadOnlyDictionary<string, object> context)
        {
            _context = context;
            _tokens = Tokenize(input);
            _position = 0;
        }

        public bool Parse()
        {
            var value = ParseOr();
            Expect(TokenKind.End);
            return value;
        }

        private bool ParseOr()
        {
            var value = ParseAnd();
            while (Match(TokenKind.Or))
            {
                value = value || ParseAnd();
            }

            return value;
        }

        private bool ParseAnd()
        {
            var value = ParseUnary();
            while (Match(TokenKind.And))
            {
                value = value && ParseUnary();
            }

            return value;
        }

        private bool ParseUnary()
        {
            if (Match(TokenKind.Not))
            {
                return !ParseUnary();
            }

            return ParsePrimary();
        }

        private bool ParsePrimary()
        {
            if (Match(TokenKind.LParen))
            {
                var value = ParseOr();
                Expect(TokenKind.RParen);
                return value;
            }

            if (IsOperandStart(Peek().Kind) && !IsComparisonAhead())
            {
                var operand = ParseOperand();
                return ToBoolean(operand);
            }

            return ParseComparison();
        }

        private bool ParseComparison()
        {
            var left = ParseOperand();
            var op = ParseComparisonOperator();
            var right = ParseOperand();
            return Compare(left, right, op);
        }

        private object ParseOperand()
        {
            var token = Peek();
            switch (token.Kind)
            {
                case TokenKind.Identifier:
                    Advance();
                    if (_context.TryGetValue(token.Text, out var resolved))
                    {
                        return resolved;
                    }

                    throw new InvalidOperationException($"Unknown identifier '{token.Text}' in evaluation context.");

                case TokenKind.String:
                case TokenKind.Number:
                case TokenKind.Boolean:
                    Advance();
                    return token.Literal ?? token.Text;

                default:
                    throw new InvalidOperationException($"Expected operand but got '{token.Text}'.");
            }
        }

        private string ParseComparisonOperator()
        {
            var token = Peek();
            Advance();
            return token.Kind switch
            {
                TokenKind.Eq => token.Text,
                TokenKind.Ne => token.Text,
                TokenKind.Lt => token.Text,
                TokenKind.Le => token.Text,
                TokenKind.Gt => token.Text,
                TokenKind.Ge => token.Text,
                _ => throw new InvalidOperationException($"Expected comparison operator but got '{token.Text}'."),
            };
        }

        private bool IsComparisonAhead()
        {
            if (!IsOperandStart(Peek().Kind))
            {
                return false;
            }

            if (_position + 1 >= _tokens.Count)
            {
                return false;
            }

            var nextKind = _tokens[_position + 1].Kind;
            return nextKind is TokenKind.Eq or TokenKind.Ne or TokenKind.Lt or TokenKind.Le or TokenKind.Gt or TokenKind.Ge;
        }

        private bool Compare(object left, object right, string op)
        {
            if (TryGetNumber(left, out var leftNumber) && TryGetNumber(right, out var rightNumber))
            {
                return CompareComparable(leftNumber, rightNumber, op);
            }

            if (left is bool leftBool && right is bool rightBool)
            {
                return CompareComparable(leftBool, rightBool, op);
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

        private static bool CompareComparable<T>(T left, T right, string op) where T : IComparable<T>
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

        private bool Match(TokenKind kind)
        {
            if (Peek().Kind != kind)
            {
                return false;
            }

            Advance();
            return true;
        }

        private void Expect(TokenKind kind)
        {
            var token = Peek();
            if (token.Kind != kind)
            {
                throw new InvalidOperationException($"Expected {kind} but got '{token.Text}'.");
            }

            Advance();
        }

        private Token Peek()
        {
            return _tokens[_position];
        }

        private void Advance()
        {
            if (_position < _tokens.Count - 1)
            {
                _position++;
            }
        }
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

    private static bool IsOperandStart(TokenKind kind)
    {
        return kind is TokenKind.Identifier or TokenKind.String or TokenKind.Number or TokenKind.Boolean;
    }

    private static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        var index = 0;

        while (index < input.Length)
        {
            var current = input[index];

            if (char.IsWhiteSpace(current))
            {
                index++;
                continue;
            }

            if (current == '/' && index + 1 < input.Length && input[index + 1] == '/')
            {
                while (index < input.Length && input[index] != '\n')
                {
                    index++;
                }

                continue;
            }

            if (current == '(')
            {
                tokens.Add(new Token(TokenKind.LParen, "("));
                index++;
                continue;
            }

            if (current == ')')
            {
                tokens.Add(new Token(TokenKind.RParen, ")"));
                index++;
                continue;
            }

            if (index + 1 < input.Length)
            {
                var twoChars = input.Substring(index, 2);
                switch (twoChars)
                {
                    case "==":
                        tokens.Add(new Token(TokenKind.Eq, twoChars));
                        index += 2;
                        continue;
                    case "!=":
                        tokens.Add(new Token(TokenKind.Ne, twoChars));
                        index += 2;
                        continue;
                    case "<=":
                        tokens.Add(new Token(TokenKind.Le, twoChars));
                        index += 2;
                        continue;
                    case ">=":
                        tokens.Add(new Token(TokenKind.Ge, twoChars));
                        index += 2;
                        continue;
                }
            }

            if (current == '=')
            {
                tokens.Add(new Token(TokenKind.Eq, "="));
                index++;
                continue;
            }

            if (current == '<')
            {
                tokens.Add(new Token(TokenKind.Lt, "<"));
                index++;
                continue;
            }

            if (current == '>')
            {
                tokens.Add(new Token(TokenKind.Gt, ">"));
                index++;
                continue;
            }

            if (current == '"')
            {
                var (token, consumed) = ReadStringToken(input, index);
                tokens.Add(token);
                index += consumed;
                continue;
            }

            if (char.IsDigit(current) || (current == '-' && index + 1 < input.Length && char.IsDigit(input[index + 1])))
            {
                var (token, consumed) = ReadNumberToken(input, index);
                tokens.Add(token);
                index += consumed;
                continue;
            }

            if (char.IsLetter(current) || current == '_')
            {
                var (token, consumed) = ReadWordToken(input, index);
                tokens.Add(token);
                index += consumed;
                continue;
            }

            throw new InvalidOperationException($"Unexpected character '{current}' at position {index}.");
        }

        tokens.Add(new Token(TokenKind.End, "<eof>"));
        return tokens;
    }

    private static (Token Token, int Consumed) ReadWordToken(string input, int start)
    {
        var index = start;
        while (index < input.Length && (char.IsLetterOrDigit(input[index]) || input[index] == '_' || input[index] == '.'))
        {
            index++;
        }

        var text = input[start..index];
        if (text.Equals("and", StringComparison.OrdinalIgnoreCase))
        {
            return (new Token(TokenKind.And, text), index - start);
        }

        if (text.Equals("or", StringComparison.OrdinalIgnoreCase))
        {
            return (new Token(TokenKind.Or, text), index - start);
        }

        if (text.Equals("not", StringComparison.OrdinalIgnoreCase))
        {
            return (new Token(TokenKind.Not, text), index - start);
        }

        if (text.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return (new Token(TokenKind.Boolean, text, true), index - start);
        }

        if (text.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return (new Token(TokenKind.Boolean, text, false), index - start);
        }

        return (new Token(TokenKind.Identifier, text), index - start);
    }

    private static (Token Token, int Consumed) ReadNumberToken(string input, int start)
    {
        var index = start;
        if (input[index] == '-')
        {
            index++;
        }

        while (index < input.Length && char.IsDigit(input[index]))
        {
            index++;
        }

        if (index < input.Length && input[index] == '.')
        {
            index++;
            while (index < input.Length && char.IsDigit(input[index]))
            {
                index++;
            }
        }

        var text = input[start..index];
        var number = double.Parse(text, CultureInfo.InvariantCulture);
        return (new Token(TokenKind.Number, text, number), index - start);
    }

    private static (Token Token, int Consumed) ReadStringToken(string input, int start)
    {
        var builder = new StringBuilder();
        var index = start + 1;

        while (index < input.Length)
        {
            var current = input[index];
            if (current == '"')
            {
                index++;
                var raw = input[start..index];
                var value = JsonSerializer.Deserialize<string>(raw) ?? builder.ToString();
                return (new Token(TokenKind.String, raw, value), index - start);
            }

            if (current == '\\' && index + 1 < input.Length)
            {
                builder.Append(current);
                builder.Append(input[index + 1]);
                index += 2;
                continue;
            }

            builder.Append(current);
            index++;
        }

        throw new InvalidOperationException("Unterminated string literal.");
    }
}
