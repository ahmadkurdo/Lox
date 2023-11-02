using Lox.AST;
using Lox.Models;

namespace Lox.Parser
{
    /*
        program → statement* EOF ;
        declaration → varDecl | statement
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
    public class Parser
    {
        public ParserState State { get; set; }

        public Parser(ParserState state) => State = state;

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();

            while (!State.IsAtEnd())
            {
                statements.Add(Declaration());
            }
            return statements;
        }

        private Expr Expression() => Equality();

        private Expr Equality() => ParseBinary(Comparison, TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL);

        private Expr Comparison() => ParseBinary(Term, TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL);

        private Expr Term() => ParseBinary(Factor, TokenType.PLUS, TokenType.MINUS);

        private Expr Factor() => ParseBinary(Unary, TokenType.SLASH, TokenType.STAR);

        private Expr Unary()
        {
            if (State.CurrentIs(TokenType.BANG, TokenType.MINUS))
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

            while (State.CurrentIs(types))
            {
                Token op = State.Previous();
                Expr right = next();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Stmt Statement() 
        {
            if (State.CurrentIs(TokenType.PRINT))
            {

                return Print();
            }
            else 
            {
                return ExpressionStatement();
            }
        }

        private Stmt Print() 
        {
            Expr value = Expression();
            State.Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new PrintStmt(value); 
        }

        private Stmt ExpressionStatement()
        {
            Expr value = Expression();
            State.Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new ExpressionStmt(value);
        }

        private Stmt Declaration() 
        {
            try 
            {
                if (State.CurrentIs(TokenType.VAR))
                {
                    return VarDeclaration();
                }
                else 
                {
                    return Statement();
                }            
            } catch (ParseError e) 
            {
                State.Synchronize();
                return default;
            }
        }

        private Stmt VarDeclaration()
        {
            Token name = State.Consume(TokenType.IDENTIFIER, "Expect variable name");

            Expr initializer = null;

            if (State.CurrentIs(TokenType.EQUAL)) 
            {
                initializer = Expression();
            }
            _ = State.Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");

            return new VarStmt(name, initializer);
        }

        private Expr Primary()
        {
            switch (State.Peek().Type)
            {
                case TokenType.FALSE:
                    State.MoveNext();
                    return new Literal(false);

                case TokenType.TRUE:
                    State.MoveNext();
                    return new Literal(true);

                case TokenType.NIL:
                    State.MoveNext();

                    return new Literal(null);

                case TokenType.NUMBER:
                case TokenType.STRING:
                    State.MoveNext();
                    return new Literal(State.Previous().Literal);


                case TokenType.IDENTIFIER:
                    return new Var(State.Previous());

                case TokenType.LEFT_PAREN:
                    State.MoveNext();
                    Expr expr = Expression();
                    State.Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                    return new Grouping(expr);

                default:
                    State.Error(State.Previous(), "Expect expression.");
                    return default;
            }
        }
    }
}
