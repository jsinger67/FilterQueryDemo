using System;
using System.Collections.Generic;
using System.Reflection;
using Parol.Runtime;
using Parol.Runtime.Scanner;

namespace FilterQueryDemo {
    // Deduced grammar types
    // Type derived for non-terminal AndExpr
    public sealed record AndExpr(UnaryExpr UnaryExpr, List<AndExprList> AndExprList);

    // Type derived for non-terminal AndExprList
    public sealed record AndExprList(UnaryExpr UnaryExpr);

    // Type derived for non-terminal AndOp
    public sealed record AndOp();

    // Type derived for non-terminal BoolLit
    public abstract record BoolLit;
    public sealed record BoolLitTrueVariant(BoolLitTrue Value) : BoolLit;
    public sealed record BoolLitFalseVariant(BoolLitFalse Value) : BoolLit;

    // Type derived for non-terminal CompareOp
    public sealed record CompareOp(Token CompareOpValue);

    // Type derived for non-terminal Comparison
    public sealed record Comparison(Operand Operand, CompareOp CompareOp, Operand Operand0);

    // Type derived for non-terminal Expr
    public sealed record Expr(OrExpr OrExpr);

    // Type derived for non-terminal False
    public sealed record False();

    // Type derived for non-terminal Identifier
    public sealed record Identifier(Token IdentifierValue);

    // Type derived for non-terminal NotOp
    public sealed record NotOp();

    // Type derived for non-terminal NumberLit
    public sealed record NumberLit(Token NumberLitValue);

    // Type derived for non-terminal Operand
    public abstract record Operand;
    public sealed record OperandIdentifierVariant(OperandIdentifier Value) : Operand;
    public sealed record OperandStringLitVariant(OperandStringLit Value) : Operand;
    public sealed record OperandNumberLitVariant(OperandNumberLit Value) : Operand;
    public sealed record OperandBoolLitVariant(OperandBoolLit Value) : Operand;

    // Type derived for non-terminal OrExpr
    public sealed record OrExpr(AndExpr AndExpr, List<OrExprList> OrExprList);

    // Type derived for non-terminal OrExprList
    public sealed record OrExprList(AndExpr AndExpr);

    // Type derived for non-terminal OrOp
    public sealed record OrOp();

    // Type derived for non-terminal Primary
    public abstract record Primary;
    public sealed record PrimaryComparisonVariant(PrimaryComparison Value) : Primary;
    public sealed record PrimaryOperandVariant(PrimaryOperand Value) : Primary;
    public sealed record PrimaryLParenExprRParenVariant(PrimaryLParenExprRParen Value) : Primary;

    // Type derived for non-terminal Query
    public sealed record Query(Expr Expr);

    // Type derived for non-terminal StringLit
    public sealed record StringLit(Token StringLitValue);

    // Type derived for non-terminal True
    public sealed record True();

    // Type derived for non-terminal UnaryExpr
    public abstract record UnaryExpr;
    public sealed record UnaryExprNotOpUnaryExprVariant(UnaryExprNotOpUnaryExpr Value) : UnaryExpr;
    public sealed record UnaryExprPrimaryVariant(UnaryExprPrimary Value) : UnaryExpr;

    // Type derived for production 10
    public sealed record UnaryExprNotOpUnaryExpr(NotOp NotOp, UnaryExpr UnaryExpr);

    // Type derived for production 11
    public sealed record UnaryExprPrimary(Primary Primary);

    // Type derived for production 13
    public sealed record PrimaryComparison(Comparison Comparison);

    // Type derived for production 14
    public sealed record PrimaryOperand(Operand Operand);

    // Type derived for production 15
    public sealed record PrimaryLParenExprRParen(Expr Expr);

    // Type derived for production 18
    public sealed record OperandIdentifier(global::FilterQueryDemo.FilterIdentifier Identifier);

    // Type derived for production 19
    public sealed record OperandStringLit(global::FilterQueryDemo.FilterString StringLit);

    // Type derived for production 20
    public sealed record OperandNumberLit(global::FilterQueryDemo.FilterNumber NumberLit);

    // Type derived for production 21
    public sealed record OperandBoolLit(BoolLit BoolLit);

    // Type derived for production 22
    public sealed record BoolLitTrue(True True);

    // Type derived for production 23
    public sealed record BoolLitFalse(False False);

    /// <summary>
    /// User actions interface for the FilterQueryDemo grammar.
    /// </summary>
    public interface IFilterQueryDemoActions : IUserActions, IProvidesValueConverter {
        void OnQuery(Query arg);

        void OnExpr(Expr arg);

        void OnOrExpr(OrExpr arg);

        void OnOrOp(OrOp arg);

        void OnAndExpr(AndExpr arg);

        void OnAndOp(AndOp arg);

        void OnUnaryExpr(UnaryExpr arg);

        void OnNotOp(NotOp arg);

        void OnPrimary(Primary arg);

        void OnComparison(Comparison arg);

        void OnCompareOp(CompareOp arg);

        void OnOperand(Operand arg);

        void OnBoolLit(BoolLit arg);

        void OnTrue(True arg);

        void OnFalse(False arg);

        void OnIdentifier(Identifier arg);

        void OnStringLit(StringLit arg);

        void OnNumberLit(NumberLit arg);

        /// <summary>
        /// Semantic action for production 0:
        /// Query: Expr; 
        /// </summary>
        void Query(object[] children);

        /// <summary>
        /// Semantic action for production 1:
        /// Expr: OrExpr; 
        /// </summary>
        void Expr(object[] children);

        /// <summary>
        /// Semantic action for production 2:
        /// OrExpr: AndExpr OrExprList /* Vec */; 
        /// </summary>
        void OrExpr(object[] children);

        /// <summary>
        /// Semantic action for production 3:
        /// OrExprList: OrOp^ /* Clipped */ AndExpr OrExprList; 
        /// </summary>
        void OrExprList0(object[] children);

        /// <summary>
        /// Semantic action for production 4:
        /// OrExprList: ; 
        /// </summary>
        void OrExprList1(object[] children);

        /// <summary>
        /// Semantic action for production 5:
        /// OrOp: /(?i)or/^ /* Clipped */; 
        /// </summary>
        void OrOp(object[] children);

        /// <summary>
        /// Semantic action for production 6:
        /// AndExpr: UnaryExpr AndExprList /* Vec */; 
        /// </summary>
        void AndExpr(object[] children);

        /// <summary>
        /// Semantic action for production 7:
        /// AndExprList: AndOp^ /* Clipped */ UnaryExpr AndExprList; 
        /// </summary>
        void AndExprList0(object[] children);

        /// <summary>
        /// Semantic action for production 8:
        /// AndExprList: ; 
        /// </summary>
        void AndExprList1(object[] children);

        /// <summary>
        /// Semantic action for production 9:
        /// AndOp: /(?i)and/^ /* Clipped */; 
        /// </summary>
        void AndOp(object[] children);

        /// <summary>
        /// Semantic action for production 10:
        /// UnaryExpr: NotOp UnaryExpr; 
        /// </summary>
        void UnaryExpr0(object[] children);

        /// <summary>
        /// Semantic action for production 11:
        /// UnaryExpr: Primary; 
        /// </summary>
        void UnaryExpr1(object[] children);

        /// <summary>
        /// Semantic action for production 12:
        /// NotOp: /(?i)not/^ /* Clipped */; 
        /// </summary>
        void NotOp(object[] children);

        /// <summary>
        /// Semantic action for production 13:
        /// Primary: Comparison; 
        /// </summary>
        void Primary0(object[] children);

        /// <summary>
        /// Semantic action for production 14:
        /// Primary: Operand; 
        /// </summary>
        void Primary1(object[] children);

        /// <summary>
        /// Semantic action for production 15:
        /// Primary: "\("^ /* Clipped */ Expr "\)"^ /* Clipped */; 
        /// </summary>
        void Primary2(object[] children);

        /// <summary>
        /// Semantic action for production 16:
        /// Comparison: Operand CompareOp Operand; 
        /// </summary>
        void Comparison(object[] children);

        /// <summary>
        /// Semantic action for production 17:
        /// CompareOp: /==|!=|<=|>=|<|>|=/; 
        /// </summary>
        void CompareOp(object[] children);

        /// <summary>
        /// Semantic action for production 18:
        /// Operand: Identifier : FilterQueryDemo::FilterIdentifier ; 
        /// </summary>
        void Operand0(object[] children);

        /// <summary>
        /// Semantic action for production 19:
        /// Operand: StringLit : FilterQueryDemo::FilterString ; 
        /// </summary>
        void Operand1(object[] children);

        /// <summary>
        /// Semantic action for production 20:
        /// Operand: NumberLit : FilterQueryDemo::FilterNumber ; 
        /// </summary>
        void Operand2(object[] children);

        /// <summary>
        /// Semantic action for production 21:
        /// Operand: BoolLit; 
        /// </summary>
        void Operand3(object[] children);

        /// <summary>
        /// Semantic action for production 22:
        /// BoolLit: True; 
        /// </summary>
        void BoolLit0(object[] children);

        /// <summary>
        /// Semantic action for production 23:
        /// BoolLit: False; 
        /// </summary>
        void BoolLit1(object[] children);

        /// <summary>
        /// Semantic action for production 24:
        /// True: /(?i)true/^ /* Clipped */; 
        /// </summary>
        void True(object[] children);

        /// <summary>
        /// Semantic action for production 25:
        /// False: /(?i)false/^ /* Clipped */; 
        /// </summary>
        void False(object[] children);

        /// <summary>
        /// Semantic action for production 26:
        /// Identifier: /(?i)[a-z_][a-z0-9_\.]*/; 
        /// </summary>
        void Identifier(object[] children);

        /// <summary>
        /// Semantic action for production 27:
        /// StringLit: /"(\\.|[^"\\])*"/; 
        /// </summary>
        void StringLit(object[] children);

        /// <summary>
        /// Semantic action for production 28:
        /// NumberLit: /-?(0|[1-9][0-9]*)(\.[0-9]+)?/; 
        /// </summary>
        void NumberLit(object[] children);

    }

    /// <summary>
    /// Base class for user actions for the FilterQueryDemo grammar.
    /// </summary>
    public partial class FilterQueryDemoActions : IFilterQueryDemoActions {
        /// <inheritdoc/>
        public virtual object CallSemanticActionForProductionNumber(int productionNumber, object[] children) {
            switch (productionNumber) {
                case 0: { var value = MapQuery(children); OnQuery(value); return value; }
                case 1: { var value = MapExpr(children); OnExpr(value); return value; }
                case 2: { var value = MapOrExpr(children); OnOrExpr(value); return value; }
                case 3: return MapOrExprList0(children);
                case 4: return MapOrExprList1(children);
                case 5: { var value = MapOrOp(children); OnOrOp(value); return value; }
                case 6: { var value = MapAndExpr(children); OnAndExpr(value); return value; }
                case 7: return MapAndExprList0(children);
                case 8: return MapAndExprList1(children);
                case 9: { var value = MapAndOp(children); OnAndOp(value); return value; }
                case 10: { var value = MapUnaryExpr0(children); OnUnaryExpr(value); return value; }
                case 11: { var value = MapUnaryExpr1(children); OnUnaryExpr(value); return value; }
                case 12: { var value = MapNotOp(children); OnNotOp(value); return value; }
                case 13: { var value = MapPrimary0(children); OnPrimary(value); return value; }
                case 14: { var value = MapPrimary1(children); OnPrimary(value); return value; }
                case 15: { var value = MapPrimary2(children); OnPrimary(value); return value; }
                case 16: { var value = MapComparison(children); OnComparison(value); return value; }
                case 17: { var value = MapCompareOp(children); OnCompareOp(value); return value; }
                case 18: { var value = MapOperand0(children); OnOperand(value); return value; }
                case 19: { var value = MapOperand1(children); OnOperand(value); return value; }
                case 20: { var value = MapOperand2(children); OnOperand(value); return value; }
                case 21: { var value = MapOperand3(children); OnOperand(value); return value; }
                case 22: { var value = MapBoolLit0(children); OnBoolLit(value); return value; }
                case 23: { var value = MapBoolLit1(children); OnBoolLit(value); return value; }
                case 24: { var value = MapTrue(children); OnTrue(value); return value; }
                case 25: { var value = MapFalse(children); OnFalse(value); return value; }
                case 26: { var value = MapIdentifier(children); OnIdentifier(value); return value; }
                case 27: { var value = MapStringLit(children); OnStringLit(value); return value; }
                case 28: { var value = MapNumberLit(children); OnNumberLit(value); return value; }
                default: throw new ArgumentException($"Invalid production number {productionNumber}");
            }
        }

        /// <inheritdoc/>
        public virtual void OnComment(Token token) { }

        /// <inheritdoc/>
        public virtual IValueConverter ValueConverter { get; } = new GeneratedValueConverter();

        private sealed class GeneratedValueConverter : IValueConverter {
            public bool TryConvert(object value, Type targetType, out object? convertedValue) {
                convertedValue = null;
                if (value == null) return false;
                var sourceType = value.GetType();
                foreach (var owner in new[] { sourceType, targetType }) {
                    foreach (var method in owner.GetMethods(BindingFlags.Public | BindingFlags.Static)) {
                        if ((method.Name == "op_Implicit" || method.Name == "op_Explicit")
                            && method.ReturnType == targetType) {
                            var parameters = method.GetParameters();
                            if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(sourceType)) {
                                convertedValue = method.Invoke(null, new[] { value });
                                return true;
                            }
                        }
                    }
                }
                var ctor = targetType.GetConstructor(new[] { sourceType });
                if (ctor != null) {
                    convertedValue = ctor.Invoke(new[] { value });
                    return true;
                }
                return false;
            }
        }

        private static TTarget ConvertValue<TTarget>(object value) {
            return RuntimeValueConverter.Convert<TTarget>(value);
        }

        /// <summary>
        /// User-facing action for non-terminal Query.
        /// </summary>
        public virtual void OnQuery(Query arg) { }

        /// <summary>
        /// User-facing action for non-terminal Expr.
        /// </summary>
        public virtual void OnExpr(Expr arg) { }

        /// <summary>
        /// User-facing action for non-terminal OrExpr.
        /// </summary>
        public virtual void OnOrExpr(OrExpr arg) { }

        /// <summary>
        /// User-facing action for non-terminal OrOp.
        /// </summary>
        public virtual void OnOrOp(OrOp arg) { }

        /// <summary>
        /// User-facing action for non-terminal AndExpr.
        /// </summary>
        public virtual void OnAndExpr(AndExpr arg) { }

        /// <summary>
        /// User-facing action for non-terminal AndOp.
        /// </summary>
        public virtual void OnAndOp(AndOp arg) { }

        /// <summary>
        /// User-facing action for non-terminal UnaryExpr.
        /// </summary>
        public virtual void OnUnaryExpr(UnaryExpr arg) { }

        /// <summary>
        /// User-facing action for non-terminal NotOp.
        /// </summary>
        public virtual void OnNotOp(NotOp arg) { }

        /// <summary>
        /// User-facing action for non-terminal Primary.
        /// </summary>
        public virtual void OnPrimary(Primary arg) { }

        /// <summary>
        /// User-facing action for non-terminal Comparison.
        /// </summary>
        public virtual void OnComparison(Comparison arg) { }

        /// <summary>
        /// User-facing action for non-terminal CompareOp.
        /// </summary>
        public virtual void OnCompareOp(CompareOp arg) { }

        /// <summary>
        /// User-facing action for non-terminal Operand.
        /// </summary>
        public virtual void OnOperand(Operand arg) { }

        /// <summary>
        /// User-facing action for non-terminal BoolLit.
        /// </summary>
        public virtual void OnBoolLit(BoolLit arg) { }

        /// <summary>
        /// User-facing action for non-terminal True.
        /// </summary>
        public virtual void OnTrue(True arg) { }

        /// <summary>
        /// User-facing action for non-terminal False.
        /// </summary>
        public virtual void OnFalse(False arg) { }

        /// <summary>
        /// User-facing action for non-terminal Identifier.
        /// </summary>
        public virtual void OnIdentifier(Identifier arg) { }

        /// <summary>
        /// User-facing action for non-terminal StringLit.
        /// </summary>
        public virtual void OnStringLit(StringLit arg) { }

        /// <summary>
        /// User-facing action for non-terminal NumberLit.
        /// </summary>
        public virtual void OnNumberLit(NumberLit arg) { }

        /// <summary>
        /// Semantic action for production 0:
        /// Query: Expr; 
        /// </summary>
        public virtual void Query(object[] children) {
            var value = MapQuery(children);
            OnQuery(value);
        }

        private static Query MapQuery(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new Query((Expr)children[0 + 0]);
            if (children.Length == 1 && children[0] is Query directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 0 (Query: Expr;)");
        }

        /// <summary>
        /// Semantic action for production 1:
        /// Expr: OrExpr; 
        /// </summary>
        public virtual void Expr(object[] children) {
            var value = MapExpr(children);
            OnExpr(value);
        }

        private static Expr MapExpr(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new Expr((OrExpr)children[0 + 0]);
            if (children.Length == 1 && children[0] is Expr directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 1 (Expr: OrExpr;)");
        }

        /// <summary>
        /// Semantic action for production 2:
        /// OrExpr: AndExpr OrExprList /* Vec */; 
        /// </summary>
        public virtual void OrExpr(object[] children) {
            var value = MapOrExpr(children);
            OnOrExpr(value);
        }

        private static OrExpr MapOrExpr(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 2 ) return new OrExpr((AndExpr)children[0 + 0], (List<OrExprList>)children[0 + 1]);
            if (children.Length == 1 && children[0] is OrExpr directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 2 (OrExpr: AndExpr OrExprList /* Vec */;)");
        }

        /// <summary>
        /// Semantic action for production 3:
        /// OrExprList: OrOp^ /* Clipped */ AndExpr OrExprList; 
        /// </summary>
        public virtual void OrExprList0(object[] children) {
            var value = MapOrExprList0(children);
        }

        private static List<OrExprList> MapOrExprList0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 && children[0] is List<OrExprList> directValue) return directValue;
            if (children.Length == 2) {
                var item = new OrExprList((AndExpr)children[0 + 1]);
                return new List<OrExprList> { item };
            }
            if (children.Length == 2 + 1 && children[2] is List<OrExprList> previous) {
                var item = new OrExprList((AndExpr)children[0 + 1]);
                var items = new List<OrExprList>();
                items.Add(item);
                items.AddRange(previous);
                return items;
            }
            throw new InvalidOperationException("Unsupported C# mapping for production 3 (OrExprList: OrOp^ /* Clipped */ AndExpr OrExprList;)");
        }

        /// <summary>
        /// Semantic action for production 4:
        /// OrExprList: ; 
        /// </summary>
        public virtual void OrExprList1(object[] children) {
            var value = MapOrExprList1(children);
        }

        private static List<OrExprList> MapOrExprList1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0) return new List<OrExprList>();
            if (children.Length == 1 && children[0] is List<OrExprList> directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 4 (OrExprList: ;)");
        }

        /// <summary>
        /// Semantic action for production 5:
        /// OrOp: /(?i)or/^ /* Clipped */; 
        /// </summary>
        public virtual void OrOp(object[] children) {
            var value = MapOrOp(children);
            OnOrOp(value);
        }

        private static OrOp MapOrOp(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0 ) return new OrOp();
            if (children.Length == 1 && children[0] is OrOp directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 5 (OrOp: /(?i)or/^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 6:
        /// AndExpr: UnaryExpr AndExprList /* Vec */; 
        /// </summary>
        public virtual void AndExpr(object[] children) {
            var value = MapAndExpr(children);
            OnAndExpr(value);
        }

        private static AndExpr MapAndExpr(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 2 ) return new AndExpr((UnaryExpr)children[0 + 0], (List<AndExprList>)children[0 + 1]);
            if (children.Length == 1 && children[0] is AndExpr directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 6 (AndExpr: UnaryExpr AndExprList /* Vec */;)");
        }

        /// <summary>
        /// Semantic action for production 7:
        /// AndExprList: AndOp^ /* Clipped */ UnaryExpr AndExprList; 
        /// </summary>
        public virtual void AndExprList0(object[] children) {
            var value = MapAndExprList0(children);
        }

        private static List<AndExprList> MapAndExprList0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 && children[0] is List<AndExprList> directValue) return directValue;
            if (children.Length == 2) {
                var item = new AndExprList((UnaryExpr)children[0 + 1]);
                return new List<AndExprList> { item };
            }
            if (children.Length == 2 + 1 && children[2] is List<AndExprList> previous) {
                var item = new AndExprList((UnaryExpr)children[0 + 1]);
                var items = new List<AndExprList>();
                items.Add(item);
                items.AddRange(previous);
                return items;
            }
            throw new InvalidOperationException("Unsupported C# mapping for production 7 (AndExprList: AndOp^ /* Clipped */ UnaryExpr AndExprList;)");
        }

        /// <summary>
        /// Semantic action for production 8:
        /// AndExprList: ; 
        /// </summary>
        public virtual void AndExprList1(object[] children) {
            var value = MapAndExprList1(children);
        }

        private static List<AndExprList> MapAndExprList1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0) return new List<AndExprList>();
            if (children.Length == 1 && children[0] is List<AndExprList> directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 8 (AndExprList: ;)");
        }

        /// <summary>
        /// Semantic action for production 9:
        /// AndOp: /(?i)and/^ /* Clipped */; 
        /// </summary>
        public virtual void AndOp(object[] children) {
            var value = MapAndOp(children);
            OnAndOp(value);
        }

        private static AndOp MapAndOp(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0 ) return new AndOp();
            if (children.Length == 1 && children[0] is AndOp directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 9 (AndOp: /(?i)and/^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 10:
        /// UnaryExpr: NotOp UnaryExpr; 
        /// </summary>
        public virtual void UnaryExpr0(object[] children) {
            var value = MapUnaryExpr0(children);
            OnUnaryExpr(value);
        }

        private static UnaryExpr MapUnaryExpr0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 2) {
                var value = new UnaryExprNotOpUnaryExpr((NotOp)children[0 + 0], (UnaryExpr)children[0 + 1]);
                return new UnaryExprNotOpUnaryExprVariant(value);
            }
            if (children.Length == 1 && children[0] is UnaryExpr directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 10 (UnaryExpr: NotOp UnaryExpr;)");
        }

        /// <summary>
        /// Semantic action for production 11:
        /// UnaryExpr: Primary; 
        /// </summary>
        public virtual void UnaryExpr1(object[] children) {
            var value = MapUnaryExpr1(children);
            OnUnaryExpr(value);
        }

        private static UnaryExpr MapUnaryExpr1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new UnaryExprPrimary((Primary)children[0 + 0]);
                return new UnaryExprPrimaryVariant(value);
            }
            if (children.Length == 1 && children[0] is UnaryExpr directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 11 (UnaryExpr: Primary;)");
        }

        /// <summary>
        /// Semantic action for production 12:
        /// NotOp: /(?i)not/^ /* Clipped */; 
        /// </summary>
        public virtual void NotOp(object[] children) {
            var value = MapNotOp(children);
            OnNotOp(value);
        }

        private static NotOp MapNotOp(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0 ) return new NotOp();
            if (children.Length == 1 && children[0] is NotOp directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 12 (NotOp: /(?i)not/^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 13:
        /// Primary: Comparison; 
        /// </summary>
        public virtual void Primary0(object[] children) {
            var value = MapPrimary0(children);
            OnPrimary(value);
        }

        private static Primary MapPrimary0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new PrimaryComparison((Comparison)children[0 + 0]);
                return new PrimaryComparisonVariant(value);
            }
            if (children.Length == 1 && children[0] is Primary directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 13 (Primary: Comparison;)");
        }

        /// <summary>
        /// Semantic action for production 14:
        /// Primary: Operand; 
        /// </summary>
        public virtual void Primary1(object[] children) {
            var value = MapPrimary1(children);
            OnPrimary(value);
        }

        private static Primary MapPrimary1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new PrimaryOperand((Operand)children[0 + 0]);
                return new PrimaryOperandVariant(value);
            }
            if (children.Length == 1 && children[0] is Primary directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 14 (Primary: Operand;)");
        }

        /// <summary>
        /// Semantic action for production 15:
        /// Primary: "\("^ /* Clipped */ Expr "\)"^ /* Clipped */; 
        /// </summary>
        public virtual void Primary2(object[] children) {
            var value = MapPrimary2(children);
            OnPrimary(value);
        }

        private static Primary MapPrimary2(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new PrimaryLParenExprRParen((Expr)children[0 + 0]);
                return new PrimaryLParenExprRParenVariant(value);
            }
            if (children.Length == 1 && children[0] is Primary directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 15 (Primary: \"\\(\"^ /* Clipped */ Expr \"\\)\"^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 16:
        /// Comparison: Operand CompareOp Operand; 
        /// </summary>
        public virtual void Comparison(object[] children) {
            var value = MapComparison(children);
            OnComparison(value);
        }

        private static Comparison MapComparison(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 3 ) return new Comparison((Operand)children[0 + 0], (CompareOp)children[0 + 1], (Operand)children[0 + 2]);
            if (children.Length == 1 && children[0] is Comparison directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 16 (Comparison: Operand CompareOp Operand;)");
        }

        /// <summary>
        /// Semantic action for production 17:
        /// CompareOp: /==|!=|<=|>=|<|>|=/; 
        /// </summary>
        public virtual void CompareOp(object[] children) {
            var value = MapCompareOp(children);
            OnCompareOp(value);
        }

        private static CompareOp MapCompareOp(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new CompareOp((Token)children[0 + 0]);
            if (children.Length == 1 && children[0] is CompareOp directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 17 (CompareOp: /==|!=|<=|>=|<|>|=/;)");
        }

        /// <summary>
        /// Semantic action for production 18:
        /// Operand: Identifier : FilterQueryDemo::FilterIdentifier ; 
        /// </summary>
        public virtual void Operand0(object[] children) {
            var value = MapOperand0(children);
            OnOperand(value);
        }

        private static Operand MapOperand0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new OperandIdentifier(ConvertValue<global::FilterQueryDemo.FilterIdentifier>(children[0 + 0]));
                return new OperandIdentifierVariant(value);
            }
            if (children.Length == 1 && children[0] is Operand directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 18 (Operand: Identifier : FilterQueryDemo::FilterIdentifier ;)");
        }

        /// <summary>
        /// Semantic action for production 19:
        /// Operand: StringLit : FilterQueryDemo::FilterString ; 
        /// </summary>
        public virtual void Operand1(object[] children) {
            var value = MapOperand1(children);
            OnOperand(value);
        }

        private static Operand MapOperand1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new OperandStringLit(ConvertValue<global::FilterQueryDemo.FilterString>(children[0 + 0]));
                return new OperandStringLitVariant(value);
            }
            if (children.Length == 1 && children[0] is Operand directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 19 (Operand: StringLit : FilterQueryDemo::FilterString ;)");
        }

        /// <summary>
        /// Semantic action for production 20:
        /// Operand: NumberLit : FilterQueryDemo::FilterNumber ; 
        /// </summary>
        public virtual void Operand2(object[] children) {
            var value = MapOperand2(children);
            OnOperand(value);
        }

        private static Operand MapOperand2(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new OperandNumberLit(ConvertValue<global::FilterQueryDemo.FilterNumber>(children[0 + 0]));
                return new OperandNumberLitVariant(value);
            }
            if (children.Length == 1 && children[0] is Operand directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 20 (Operand: NumberLit : FilterQueryDemo::FilterNumber ;)");
        }

        /// <summary>
        /// Semantic action for production 21:
        /// Operand: BoolLit; 
        /// </summary>
        public virtual void Operand3(object[] children) {
            var value = MapOperand3(children);
            OnOperand(value);
        }

        private static Operand MapOperand3(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new OperandBoolLit((BoolLit)children[0 + 0]);
                return new OperandBoolLitVariant(value);
            }
            if (children.Length == 1 && children[0] is Operand directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 21 (Operand: BoolLit;)");
        }

        /// <summary>
        /// Semantic action for production 22:
        /// BoolLit: True; 
        /// </summary>
        public virtual void BoolLit0(object[] children) {
            var value = MapBoolLit0(children);
            OnBoolLit(value);
        }

        private static BoolLit MapBoolLit0(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new BoolLitTrue((True)children[0 + 0]);
                return new BoolLitTrueVariant(value);
            }
            if (children.Length == 1 && children[0] is BoolLit directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 22 (BoolLit: True;)");
        }

        /// <summary>
        /// Semantic action for production 23:
        /// BoolLit: False; 
        /// </summary>
        public virtual void BoolLit1(object[] children) {
            var value = MapBoolLit1(children);
            OnBoolLit(value);
        }

        private static BoolLit MapBoolLit1(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1) {
                var value = new BoolLitFalse((False)children[0 + 0]);
                return new BoolLitFalseVariant(value);
            }
            if (children.Length == 1 && children[0] is BoolLit directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 23 (BoolLit: False;)");
        }

        /// <summary>
        /// Semantic action for production 24:
        /// True: /(?i)true/^ /* Clipped */; 
        /// </summary>
        public virtual void True(object[] children) {
            var value = MapTrue(children);
            OnTrue(value);
        }

        private static True MapTrue(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0 ) return new True();
            if (children.Length == 1 && children[0] is True directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 24 (True: /(?i)true/^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 25:
        /// False: /(?i)false/^ /* Clipped */; 
        /// </summary>
        public virtual void False(object[] children) {
            var value = MapFalse(children);
            OnFalse(value);
        }

        private static False MapFalse(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 0 ) return new False();
            if (children.Length == 1 && children[0] is False directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 25 (False: /(?i)false/^ /* Clipped */;)");
        }

        /// <summary>
        /// Semantic action for production 26:
        /// Identifier: /(?i)[a-z_][a-z0-9_\.]*/; 
        /// </summary>
        public virtual void Identifier(object[] children) {
            var value = MapIdentifier(children);
            OnIdentifier(value);
        }

        private static Identifier MapIdentifier(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new Identifier((Token)children[0 + 0]);
            if (children.Length == 1 && children[0] is Identifier directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 26 (Identifier: /(?i)[a-z_][a-z0-9_\\.]*/;)");
        }

        /// <summary>
        /// Semantic action for production 27:
        /// StringLit: /"(\\.|[^"\\])*"/; 
        /// </summary>
        public virtual void StringLit(object[] children) {
            var value = MapStringLit(children);
            OnStringLit(value);
        }

        private static StringLit MapStringLit(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new StringLit((Token)children[0 + 0]);
            if (children.Length == 1 && children[0] is StringLit directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 27 (StringLit: /\"(\\\\.|[^\"\\\\])*\"/;)");
        }

        /// <summary>
        /// Semantic action for production 28:
        /// NumberLit: /-?(0|[1-9][0-9]*)(\.[0-9]+)?/; 
        /// </summary>
        public virtual void NumberLit(object[] children) {
            var value = MapNumberLit(children);
            OnNumberLit(value);
        }

        private static NumberLit MapNumberLit(object[] children) {
            if (children == null) throw new ArgumentNullException(nameof(children));
            if (children.Length == 1 ) return new NumberLit((Token)children[0 + 0]);
            if (children.Length == 1 && children[0] is NumberLit directValue) return directValue;
            throw new InvalidOperationException("Unsupported C# mapping for production 28 (NumberLit: /-?(0|[1-9][0-9]*)(\\.[0-9]+)?/;)");
        }

    }
}
