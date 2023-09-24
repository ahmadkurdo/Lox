using Lox.Models;
using System.Globalization;

namespace Lox
{
    public class ScannerV2
    {
        private readonly string source;

        public ScannerV2(string source)
        {
            this.source = source;
        }

        public ScannerState Match(ScannerState state, int currentIndex, char currentChar, char nextChar, char prevChar, bool isLastChar) =>
            (currentChar, nextChar, expected: state.Expected) switch
            {
                ('\r', _, _) => state,
                ('\t', _, _) => state,
                (' ', _, _) => state,
                ('\n', _, _) => state with { line = state.line + 1 },

                ('/', '/', Expect.None) => state with { Expected = Expect.Comment },
                ('/', _, Expect.Comment) => state with { Expected = Expect.Comment },
                (_, _, Expect.Comment) when nextChar == '\n' || isLastChar => state with { Expected = Expect.None },
                ('/', _, Expect.None) when nextChar != '/' => state.AddToken(CreateToken(TokenType.SLASH, currentIndex, 1, state.line)),

                ('!', '=', Expect.None) => state.AddToken(CreateToken(TokenType.BANG_EQUAL, currentIndex, 2, state.line)),
                ('>', '=', Expect.None) => state.AddToken(CreateToken(TokenType.GREATER_EQUAL, currentIndex, 2, state.line)),
                ('<', '=', Expect.None) => state.AddToken(CreateToken(TokenType.LESS_EQUAL, currentIndex, 2, state.line)),
                ('=', '=', Expect.None) when !IsTokenType(prevChar) => state.AddToken(CreateToken(TokenType.EQUAL_EQUAL, currentIndex, 2, state.line)),

                ('!', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.BANG, currentIndex, 1, state.line)),
                ('>', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.GREATER, currentIndex, 1, state.line)),
                ('<', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.LESS, currentIndex, 1, state.line)),
                ('=', _, Expect.None) when !IsTokenType(prevChar) && nextChar != '=' => state.AddToken(CreateToken(TokenType.EQUAL, currentIndex, 1, state.line)),

                ('"', _, Expect.None) when nextChar != '"' && !isLastChar => state with { LexemeStartIndex = currentIndex, Expected = Expect.String },
                ('"', '"', Expect.String) => state.AddToken(CreateToken(TokenType.STRING, currentIndex, 2, state.line)).Reset(),
                ('"', _, Expect.String) => state.AddToken(CreateStringToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),
                (_, _, Expect.String) when isLastChar => state.AddToken(CreateErrorToken("Unterminated string", state.line)),

                (_, _, Expect.None) when IsDigit(currentChar) => state with { LexemeStartIndex = currentIndex, Expected = Expect.DotOrDigid },
                (_, _, Expect.DotOrDigid) when IsDigit(currentChar) && IsDigit(nextChar) => state with { Expected = Expect.DotOrDigid },
                (_, _, Expect.DotOrDigid) when IsDigit(currentChar) && nextChar != '.' && (!IsDigit(nextChar) || isLastChar) => state.AddToken(CreateDigitToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),
                ('.', _, Expect.DotOrDigid) when IsDigit(nextChar) => state with { Expected = Expect.Digid },
                (_, _, Expect.Digid) when IsDigit(currentChar) && IsDigit(nextChar) => state with { Expected = Expect.Digid },
                (_, _, Expect.Digid) when IsDigit(currentChar) && (!IsDigit(nextChar) || isLastChar) => state.AddToken(CreateDigitToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),

                _ => state
            };


        public List<Token> Scan()
        {
            var tokens = new List<Token>();

            ScannerState state = source
                .Select((character, index) => (character, index))
                .Aggregate(new ScannerState(), (state, cursor) =>
                {
                    bool isLast = cursor.index >= source.Length -1;
                    bool isFirst = cursor.index == 0;
                    char next = !isLast ? source[cursor.index + 1] : default;
                    char prev = !isFirst ? source[cursor.index - 1] : default;
                    return Match(state, cursor.index, cursor.character, next, prev, isLast);
                });

            return state.Tokens.ToList();
        }

        public Token CreateToken(TokenType type, int start, int end, int line ,object? literal = null)
        {
            string lexeme = source.Substring(start, end);
            return new(type, lexeme, literal, line);
        }

        public Token CreateStringToken(int start, int end, int line)
        {
            string value = source.Substring(start + 1, (end - start) - 1);
            string lexeme = source.Substring(start, (end - start) + 1);
            return new(TokenType.STRING, lexeme, value, line);
        }

        public Token CreateDigitToken(int start, int end, int line)
        {
            double value = double.Parse(source.Substring(start, (end - start) + 1), CultureInfo.InvariantCulture);
            string lexeme = source.Substring(start, (end - start) + 1);
            return new(TokenType.NUMBER, lexeme, value, line);
        }

        public Token CreateErrorToken(string error, int line)
        {
            return new(TokenType.Error, null, error, line);
        }
        public bool IsDigit(char c) => c >= '0' && c <= '9';

        public bool IsTokenType(char token) =>
            token switch
            {
                '!' => true,
                '>' => true,
                '<' => true,
                '=' => true,
                '{' => true,
                '}' => true,
                '(' => true,
                ')' => true,
                '.' => true,
                '+' => true,
                '-' => true,
                '/' => true,
                '*' => true,
                _ => false
            };
    }
}
