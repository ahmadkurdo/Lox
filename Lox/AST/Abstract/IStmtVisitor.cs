namespace Lox.AST.Abstract
{
    public interface IStmtVisitor
    {
        void VisitPrintStmt(PrintStmt printStmt);
        void VisitExpressionStmt(ExpressionStmt expressionStmt);
        void VisitVarStmt(VarStmt expressionStmt);
    }
}
