namespace Lox.AST.Abstract
{
    public interface IExprVisitor<T>
    {
        T VisitBinaryExpr(Binary binary);
        T VisitUnaryExpr(Unary unary);
        T VisitLiteralExpr(Literal binary);
        T VisitGroupingExpr(Grouping binary);
    }
}
