using Lox.AST;
using Lox.AST.Abstract;
using Lox.Models;

namespace Lox
{
    public class Interperter : IExprVisitor<object>
    {
        public object VisitBinaryExpr(Binary expr) 
        {
            object left = Eval(expr.Left);
            object right = Eval(expr.Right);

            return
                expr is { Operation.Type: TokenType.STAR } ? (double)left * (double)right :
                expr is { Operation.Type: TokenType.MINUS } ? (double)left - (double)right :
                expr is { Operation.Type: TokenType.SLASH } ? (double)left / (double)right :
                expr is { Operation.Type: TokenType.PLUS } && left.GetType() == typeof(string) && left.GetType() == typeof(string) ? (string)left + (string)right :
                expr is { Operation.Type: TokenType.PLUS } && left.GetType() == typeof(double) && left.GetType() == typeof(double) ? (double)left + (double)right :
                default;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Eval(expr.right);
            
            return
                expr is { operation.Type: TokenType.MINUS } ? -(double)right :
                expr is { operation.Type: TokenType.BANG } ? !IsTruthy(right) : 
                default;
        }

        public object VisitGroupingExpr(Grouping expr) => Eval(expr.expr);

        public object VisitLiteralExpr(Literal expr) => expr.Value;

        public object Eval(Expr expr) => expr.Accept(this);

        private bool IsTruthy(object obj) => 
            obj == null ? false : 
            obj.GetType() == typeof(bool) ? (bool)obj : 
            true;
    }
}
