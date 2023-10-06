namespace Lox.Parser
{
    public class ParserState
    {
        public int Cursor { get; set; }
        
        public List<Token> Tokens { get; set; }

        public ParserState(int cursor, List<Token> tokens)
        {
            Cursor = cursor;
            Tokens = tokens;
        }

        public void MoveNext() 
        {
            Cursor++;
        }

        public Token GetCurrent() => Tokens[Cursor++];

        public Token GetNext() => Tokens[++Cursor];

        public Token Peek() => Tokens[++Cursor];

        public Token PeekNext() => Tokens[Cursor + 1];

        public Token Previous() => Tokens[Cursor - 1];

        public bool IsAtEnd() => Peek().Type == TokenType.EOF;

        public bool IsSameAsNext(TokenType type) => !IsAtEnd() && Peek().Type == type;

        public void Consume(TokenType type, string message)
        {
            if (IsSameAsNext(type))
            {
                MoveNext();
            }
            else
            {
                Program.Error(Peek().Line, message);
            }
        }
    }
}
