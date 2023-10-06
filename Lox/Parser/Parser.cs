using Lox.AST;
using System.Runtime.InteropServices;

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

        public Parser(ParserState state)
        {
            State = state;
        }

        private Expr Expression() =>  Equality();
        
        private Expr Equality() 
        {
            Expr expr = Comparison();

            while(NextIs(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)) 
            {
                // a == b
                Token op = State.Previous();
                Expr right = Comparison();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {

            Expr expr = Term();

            while (NextIs(TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL))
            {
                Token op = State.Previous();
                Expr right = Term();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Term() 
        {
            Expr expr = Factor();

            while (NextIs(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = State.Previous();
                Expr right = Factor();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Factor() 
        {
            Expr expr = Unary();

            while (NextIs(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = State.Previous();
                Expr right = Unary();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary() 
        {

            if (NextIs(TokenType.BANG, TokenType.MINUS)) 
            {
                Token op = State.Previous();
                Expr right = Unary();
                return new Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary() 
        {
            if (NextIs(TokenType.FALSE))
                return new Literal(false);

            if (NextIs(TokenType.TRUE))
                return new Literal(true);

            if (NextIs(TokenType.NIL))
                return new Literal(null);

            if (NextIs(TokenType.NUMBER, TokenType.STRING))
                return new Literal(State.Previous().Literal);

            if (NextIs(TokenType.LEFT_PAREN)) 
            {
                Expr expr = Expression();
                State.Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Grouping(expr);
            }
            return default;
        }

        private bool NextIs(params TokenType[] types) 
        {
            if (types.Any(t => State.IsSameAsNext(t))) 
            {
                State.MoveNext();
                return true;
            
            }
            return false;
        }
    }
}
