using Lox.Extensions;
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

        public ScannerState Match(ScannerState state, int currentIndex, char currentChar, char nextChar, char prevChar, bool isLastChar) =>
            (currentChar, nextChar, expected: state.Expected) switch
            {
                ('\r', _, _) => state,
                ('\t', _, _) => state,
                (' ', _, _) => state,
                ('\n', _, _) => state with { line = state.line + 1 },

                ('(', _, Expect.None) => state.AddToken(CreateToken(TokenType.RIGHT_PAREN, currentIndex, 1, state.line)),
                (')', _, Expect.None) => state.AddToken(CreateToken(TokenType.LEFT_PAREN, currentIndex, 1, state.line)),
                ('{', _, Expect.None) => state.AddToken(CreateToken(TokenType.RIGHT_BRACE, currentIndex, 1, state.line)),
                ('}', _, Expect.None) => state.AddToken(CreateToken(TokenType.LEFT_BRACE, currentIndex, 1, state.line)),
                (',', _, Expect.None) => state.AddToken(CreateToken(TokenType.COMMA, currentIndex, 1, state.line)),
                ('.', _, Expect.None) => state.AddToken(CreateToken(TokenType.DOT, currentIndex, 1, state.line)),
                ('+', _, Expect.None) => state.AddToken(CreateToken(TokenType.PLUS, currentIndex, 1, state.line)),
                ('-', _, Expect.None) => state.AddToken(CreateToken(TokenType.MINUS, currentIndex, 1, state.line)),
                (';', _, Expect.None) => state.AddToken(CreateToken(TokenType.SEMICOLON, currentIndex, 1, state.line)),


                ('/', '/', Expect.None) => state with { Expected = Expect.Comment },
                ('/', _, Expect.Comment) => state with { Expected = Expect.Comment },
                (_, _, Expect.Comment) when nextChar == '\n' || isLastChar => state with { Expected = Expect.None },
                ('/', _, Expect.None) when nextChar != '/' => state.AddToken(CreateToken(TokenType.SLASH, currentIndex, 1, state.line)),

                ('!', '=', Expect.None) => state.AddToken(CreateToken(TokenType.BANG_EQUAL, currentIndex, 2, state.line)),
                ('>', '=', Expect.None) => state.AddToken(CreateToken(TokenType.GREATER_EQUAL, currentIndex, 2, state.line)),
                ('<', '=', Expect.None) => state.AddToken(CreateToken(TokenType.LESS_EQUAL, currentIndex, 2, state.line)),
                ('=', '=', Expect.None) when !prevChar.IsOperator() => state.AddToken(CreateToken(TokenType.EQUAL_EQUAL, currentIndex, 2, state.line)),

                ('!', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.BANG, currentIndex, 1, state.line)),
                ('>', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.GREATER, currentIndex, 1, state.line)),
                ('<', _, Expect.None) when nextChar != '=' => state.AddToken(CreateToken(TokenType.LESS, currentIndex, 1, state.line)),
                //To prevent ===
                ('=', _, Expect.None) when !prevChar.IsOperator() && nextChar != '=' => state.AddToken(CreateToken(TokenType.EQUAL, currentIndex, 1, state.line)),

                ('"', _, Expect.None) when nextChar != '"' && !isLastChar => state with { LexemeStartIndex = currentIndex, Expected = Expect.String },
                ('"', '"', Expect.String) => state.AddToken(CreateToken(TokenType.STRING, currentIndex, 2, state.line)).Reset(),
                ('"', _, Expect.String) => state.AddToken(CreateStringToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),
                (_, _, Expect.String) when isLastChar => state.AddToken(CreateErrorToken("Unterminated string", state.line)),

                (_, _, Expect.None) when currentChar.IsDigit() && !nextChar.IsDigit() => state.AddToken(CreateDigitToken(currentIndex, currentIndex, state.line)),
                (_, _, Expect.None) when currentChar.IsDigit() && nextChar.IsDigit() => state with { LexemeStartIndex = currentIndex, Expected = Expect.DotOrDigid },
                (_, _, Expect.DotOrDigid) when currentChar.IsDigit() && nextChar != '.' && (!nextChar.IsDigit() || isLastChar) => state.AddToken(CreateDigitToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),
                ('.', _, Expect.DotOrDigid) when nextChar.IsDigit() => state with { Expected = Expect.Digid },
                (_, _, Expect.Digid) when currentChar.IsDigit() && nextChar.IsDigit() => state with { Expected = Expect.Digid },
                (_, _, Expect.Digid) when currentChar.IsDigit() && (!nextChar.IsDigit() || isLastChar) => state.AddToken(CreateDigitToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),

                (_, _, Expect.None) when currentChar.IsAlpha() && !prevChar.IsAlphaNumeric() && !nextChar.IsAlphaNumeric() => state.AddToken(CreateIdentifierToken(currentIndex, currentIndex, state.line)).Reset(),
                (_, _, Expect.None) when currentChar.IsAlpha() && !prevChar.IsAlphaNumeric() && nextChar.IsAlphaNumeric() => state with { LexemeStartIndex = currentIndex, Expected = Expect.Identifier},
                (_, _, Expect.Identifier) when currentChar.IsAlphaNumeric() && nextChar.IsAlphaNumeric() => state with { Expected = Expect.Identifier },
                (_, _, Expect.Identifier) when currentChar.IsAlphaNumeric() && !nextChar.IsAlphaNumeric() => state.AddToken(CreateIdentifierToken(state.LexemeStartIndex, currentIndex, state.line)).Reset(),


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

        public Token CreateIdentifierToken(int start, int end, int line)
        {
            string value = source.Substring(start, (end - start) + 1);
            string lexeme = source.Substring(start, (end - start) + 1);
            TokenType type = keywords.FirstOrDefault(x => x.Key == lexeme).Value;
            return new(type, lexeme, value, line);
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
    }
}
