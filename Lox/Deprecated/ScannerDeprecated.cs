//namespace Deprecated.Lox
//{
//    public class DeprecatedScanner
//    {
//        private readonly string source;
//        private int line = 1;
//        private int start = 0;
//        private int current = 0;

//        private readonly List<Token> tokens = new();

//        public DeprecatedScanner(string source)
//        {
//            this.source = source;
//        }

//        public List<Token> Scan()
//        {
//            while (!IsEndOfFile())
//            {
//                Match(GetNext());
//                start = current;
//            }
//            tokens.Add(new Token(TokenType.EOF, "", null, line));
//            return tokens;
//        }

//        public bool IsEndOfFile() => current >= source.Length;

//        public bool IsEndOfFile(int cursor) => cursor >= source.Length;

//        private void Match(char c)
//        {
//            switch (c)
//            {
//                case '(':
//                    AddToken(TokenType.LEFT_PAREN);
//                    break;
//                case ')':
//                    AddToken(TokenType.RIGHT_PAREN);
//                    break;
//                case '{':
//                    AddToken(TokenType.LEFT_BRACE);
//                    break;
//                case '}':
//                    AddToken(TokenType.RIGHT_BRACE);
//                    break;
//                case '.':
//                    AddToken(TokenType.DOT);
//                    break;
//                case ',':
//                    AddToken(TokenType.COMMA);
//                    break;
//                case ';':
//                    AddToken(TokenType.SEMICOLON);
//                    break;
//                case '+':
//                    AddToken(TokenType.LEFT_PAREN);
//                    break;
//                case '-':
//                    AddToken(TokenType.LEFT_PAREN);
//                    break;
//                case '*':
//                    AddToken(TokenType.STAR);
//                    break;
//                case '!':
//                    AddToken(IsSameAsNext('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
//                    break;
//                case '=':
//                    AddToken(IsSameAsNext('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
//                    break;
//                case '<':
//                    AddToken(IsSameAsNext('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
//                    break;
//                case '>':
//                    AddToken(IsSameAsNext('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
//                    break;
//                case '/':
//                    if (IsSameAsNext('/'))
//                    {
//                        while (Peek() != '\n' && !IsEndOfFile())
//                            GetNext();
//                    }
//                    else
//                    {
//                        AddToken(TokenType.SLASH);
//                    }
//                    break;
//                case ' ':
//                case '\r':
//                case '\t':
//                    break;
//                case '\n':
//                    line++;
//                    break;
//                case '"':
//                    String();
//                    break;
//                default:
//                    if (isDigit(c))
//                        Number();
//                    else
//                        Program.Error(line, "Unexpected character.");
//                    break;
//            }
//        }

//        public void String()
//        {
//            while (Peek() != '"' && !IsEndOfFile())
//            {
//                if (Peek() == '\n')
//                    line++;
//                GetNext();

//                if (IsEndOfFile())
//                {
//                    Program.Error(line, "Unterminated string");
//                    return;
//                }
//            }
//            // The closing ".
//            GetNext();

//            string value = source.Substring(start + 1, current - 1);
//            AddToken(TokenType.STRING, value);
//        }

//        public void AddToken(TokenType type) => AddToken(type);

//        public void AddToken(TokenType type, object? literal = null)
//        {
//            string lexeme = source.Substring(start, current);
//            tokens.Add(new(type, lexeme, literal, line));

//        }

//        public bool isDigit(char c) => c >= '0' && c <= '9';

//        private void Number()
//        {
//            while (isDigit(Peek()))
//                GetNext();
//            if (Peek() == '.' && isDigit(PeekNext()))
//                GetNext();
//            while (isDigit(Peek()))
//                GetNext();

//            AddToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current)));
//        }


//        public char GetNext() => source[current++];

//        public char Peek() => IsEndOfFile() ? '\0' : source[current];

//        public char PeekNext() => IsEndOfFile(current + 1) ? '\0' : source[current + 1];


//        public bool IsSameAsNext(char c)
//        {
//            if (source[current] == c && !IsEndOfFile())
//            {
//                current++;
//                return true;
//            }

//            return false;
//        }
//    }
//}
