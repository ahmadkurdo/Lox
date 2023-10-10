using Lox.Extensions;
using Lox.Models;
using System.Globalization;

namespace Lox
{
    public class Scanner
    {

        private readonly string source;
        private int line = 1;
        private int start = 0;
        private int current = 0;

        private readonly List<Token> tokens = new();

        public Scanner(string source)
        {
            this.source = source;
        }

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };

        public List<Token> Scan()
        {
            while (!IsEndOfFile())
            {
                start = current;
                Match(GetNext());
            }
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        public bool IsEndOfFile() => current >= source.Length;

        public bool IsEndOfFile(int cursor) => cursor >= source.Length;

        private void Match(char c)
        {
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(IsSameAsNext('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(IsSameAsNext('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(IsSameAsNext('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(IsSameAsNext('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (IsSameAsNext('/'))
                    {
                        while (Peek() != '\n' && !IsEndOfFile())
                            GetNext();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    String();
                    break;
                default:
                    if (isDigit(c))
                        Number();
                    else if (c.IsAlpha())
                        Identifier();
                    else
                        Program.Error(line, "Unexpected character.");
                    break;
            }
        }
        public void Identifier()
        {
            while (Peek().IsAlphaNumeric())
                GetNext();

            string text = source.substring(start, current);
            TokenType type = keywords.FirstOrDefault(x => x.Key == text).Value;
            AddToken(type);
        }
        public void String()
        {
            while (Peek() != '"' && !IsEndOfFile())
            {
                if (Peek() == '\n') line++;
                GetNext();

                if (IsEndOfFile())
                {
                    Program.Error(line, "Unterminated string");
                    return;
                }
            }

            // The closing ".
            GetNext();
            string value = source.substring(start + 1, current - 1);
            AddToken(TokenType.STRING, value);

        }

        public void AddToken(TokenType type) => AddToken(type, null);

        public void AddToken(TokenType type, object? literal = null)
        {
            string lexeme = source.substring(start, current);
            tokens.Add(new(type, lexeme, literal, line));

        }

        public bool isDigit(char c) => c >= '0' && c <= '9';

        private void Number()
        {
            while (isDigit(Peek()))
                GetNext();
            if (Peek() == '.' && isDigit(PeekNext()))
                GetNext();
            while (isDigit(Peek()))
                GetNext();

            AddToken(TokenType.NUMBER, double.Parse(source.substring(start, current), CultureInfo.InvariantCulture));
        }


        public char GetNext() => source[current++];

        public char Peek() => IsEndOfFile() ? '\0' : source[current];

        public char PeekNext() => IsEndOfFile(current + 1) ? '\0' : source[current + 1];


        public bool IsSameAsNext(char c)
        {
            if (!IsEndOfFile() && source[current] == c)
            {
                current++;
                return true;
            }

            return false;
        }
    }
}
