using Lox.AST;

namespace Lox.Parser
{
    /*
        expression → equality ;
        equality → comparison ( ( "!=" | "==" ) comparison )* ;
        comparison → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
        term → factor ( ( "-" | "+" ) factor )* ;
        factor → unary ( ( "/" | "*" ) unary )* ;
        unary → ( "!" | "-" ) unary | primary ;
        primary → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
     */
    public class Parser
    {
        public ParserState State { get; set; }

        public Parser(ParserState state) => State = state;
        
        private Expr Expression() =>  Equality();

        private Expr Equality() => ParseBinary(() => Comparison(), TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL);
    
        private Expr Comparison() => ParseBinary(() => Term(), TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL);

        private Expr Term() =>  ParseBinary(() => Factor(), TokenType.PLUS, TokenType.MINUS);

        private Expr Factor() => ParseBinary(() => Unary(), TokenType.SLASH, TokenType.STAR);

        private Expr Unary() 
        {
            if (State.NextIs(TokenType.BANG, TokenType.MINUS)) 
            {
                Token op = State.Previous();
                Expr right = Unary();
                return new Unary(op, right);
            }

            return Primary();
        }

        private Expr ParseBinary(Func<Expr> next, params TokenType[] types)
        {
            Expr expr = next();

            while (State.NextIs(types))
            {
                Token op = State.Previous();
                Expr right = next();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Primary()
        {
            switch (State.GetNext().Type)
            {
                case TokenType.FALSE:
                    return new Literal(false);

                case TokenType.TRUE:
                    return new Literal(true);

                case TokenType.NIL:
                    return new Literal(null);

                case TokenType.NUMBER:
                case TokenType.STRING:
                    return new Literal(State.GetCurrent().Literal);

                case TokenType.LEFT_PAREN:
                    Expr expr = Expression();
                    State.Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                    return new Grouping(expr);

                default:
                    Program.Error(State.GetCurrent(), "Expect expression.");
                    return default;
            }
        }

    }
}
