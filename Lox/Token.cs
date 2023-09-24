namespace Lox
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public object Literal { get; set; }
        public int Line { get; set; }

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString() => Type + " " + Lexeme + " " + Literal;

        public void Deconstruct(out TokenType type, out string lexeme, out object literal, out int line)
        {
            type = Type;
            lexeme = Lexeme;
            literal = Literal;
            line = Line;
        }
    }

}
