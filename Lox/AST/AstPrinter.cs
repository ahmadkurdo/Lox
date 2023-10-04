using Lox.AST.Abstract;
using System.Text;

namespace Lox.AST
{
    public class AstPrinter : IExprVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);

        public string VisitBinaryExpr(Binary binaryExpr) => Parenthesize(binaryExpr.Operation.Lexeme, binaryExpr.Left, binaryExpr.Right);

        public string VisitGroupingExpr(Grouping grouping) => Parenthesize("group", grouping.expr);

        public string VisitLiteralExpr(Literal literal) => literal == null ? "nil" : literal.Value?.ToString();

        public string VisitUnaryExpr(Unary unary) => Parenthesize(unary.operation.Lexeme, unary.right);

        private string Parenthesize(string name, params Expr[] exprs) 
        {
        
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(name);
            foreach (Expr expr in exprs) 
            {
                sb.Append(" ");
                sb.Append(expr.Accept(this));
            }
            sb.Append(")");
            return sb.ToString();

        }
    }
}
