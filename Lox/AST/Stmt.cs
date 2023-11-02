using Lox.AST.Abstract;
using Lox.Models;

namespace Lox.AST
{
    /*
        program → statement* EOF ;
        statement → exprStmt | printStmt ;
        exprStmt → expression ";" ;
        printStmt → "print" expression ";" ;
        
        expression → equality ;
        equality → comparison ( ( "!=" | "==" ) comparison )* ;
        comparison → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
        term → factor ( ( "-" | "+" ) factor )* ;
        factor → unary ( ( "/" | "*" ) unary )* ;
        unary → ( "!" | "-" ) unary | primary ;
        primary → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
     */
    public abstract record Stmt() 
    {
        public abstract void Accept(IStmtVisitor visitor);
    }

    public record PrintStmt(Expr expression) : Stmt 
    {
        public override void Accept(IStmtVisitor visitor) => visitor.VisitPrintStmt(this);
    }

    public record ExpressionStmt(Expr expression) : Stmt 
    {
        public override void Accept(IStmtVisitor visitor) => visitor.VisitExpressionStmt(this);
    }

    public record VarStmt(Token name, Expr expression) : Stmt
    {
        public override void Accept(IStmtVisitor visitor) => visitor.VisitVarStmt(this);
    }
}
