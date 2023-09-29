using Lox.AST.Abstract;

namespace Lox.AST
{
    /* 
        Json -> object 
        object -> "{" key : value "}"
        key -> STRING
        value -> object | array | literal
        literal → NUMBER | STRING | "true" | "false" | "nul" ;
        array -> "[" literal* "]"
     
        expression → literal | unary | binary | grouping ;
        literal → NUMBER | STRING | "true" | "false" | "nil" ;
        grouping → "(" expression ")" ;
        unary → ( "-" | "!" ) expression ;
        binary → expression operator expression ;
        operator → "==" | "!=" | "<" | "<=" | ">" | ">=" | "+" | "-" | "*" | "/" ;
   */
    public abstract record Expr() 
    {
        public abstract T Accept<T>(IExprVisitor<T> visitor);
    };

    public record Unary(Token operation, Expr right) : Expr 
    {
        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.VisitUnaryExpr(this);
    };

    public record Binary(Expr Left, Token Operation, Expr Right) : Expr 
    {
        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.VisitBinaryExpr(this);
    };

    public record Literal(object Value) : Expr 
    {
        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
    };

    public record Grouping (Expr expr) : Expr 
    {
        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
    };
}
