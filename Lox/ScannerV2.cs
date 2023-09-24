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


        public ScannerState Match(ScannerState state, (char currentCharacter, int index) cursor) =>
            (cursor.currentCharacter, state.Expected, state.NextChar, state.IsLastChar) switch
            {
                ('\r', _, _, _) => state,
                ('\t', _, _, _) => state,
                (' ', _, _, _) => state,
                ('\n', _, _, _) => state with { line = state.line + 1 },

                ('/', Expect.None, '/', _) => state with { Expected = Expect.Comment },
                ('/', Expect.Comment, _, _) => state with { Expected = Expect.None },
                ('/', Expect.None, _, _) when state.NextChar != '/' => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.SLASH, cursor.index, 1, state.line)) },

                ('!', Expect.None, '=', _) => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.BANG_EQUAL, cursor.index, 2, state.line)) },
                ('>', Expect.None, '=', _) => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.GREATER_EQUAL, cursor.index, 2, state.line)) },
                ('<', Expect.None, '=', _) => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.LESS_EQUAL, cursor.index, 2, state.line)) },
                ('=', Expect.None, '=', _) => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.EQUAL_EQUAL, cursor.index, 2, state.line)) },

                ('!', Expect.None, _, _) when state.NextChar != '=' => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.BANG, cursor.index, 1, state.line)) },
                ('>', Expect.None, _, _) when state.NextChar != '=' => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.GREATER, cursor.index, 1, state.line)) },
                ('<', Expect.None, _, _) when state.NextChar != '=' => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.LESS, cursor.index, 1, state.line)) },
                ('=', Expect.None, _, _) when state.NextChar != '=' => state with { Tokens = state.Tokens.Append(CreateToken(TokenType.EQUAL, cursor.index, 1, state.line)) },

                ('"', Expect.None, _, _) when state.NextChar != '"' && !state.IsLastChar => state with { LexemeStartIndex = cursor.index, Expected = Expect.String },
                ('"', Expect.String, '"', _) => state with { LexemeStartIndex = 0, Tokens = state.Tokens.Append(CreateToken(TokenType.STRING, cursor.index, 2, state.line)), Expected = Expect.None },
                ('"', Expect.String, _, _) => state with { LexemeStartIndex = 0, Tokens = state.Tokens.Append(CreateStringToken(state.LexemeStartIndex, cursor.index, state.line)), Expected = Expect.None },
                (_, Expect.String, _, true) => state with { Tokens = state.Tokens.Append(CreateErrorToken("Unterminated string", state.line)) },

                (_, Expect.None, _, _) when IsDigit(cursor.currentCharacter) => state with { LexemeStartIndex = cursor.index, Expected = Expect.DotOrDigid },
                (_, Expect.DotOrDigid, _, _) when IsDigit(cursor.currentCharacter) && IsDigit(state.NextChar) => state with { Expected = Expect.DotOrDigid },
                (_, Expect.DotOrDigid, _, _) when IsDigit(cursor.currentCharacter) && state.NextChar != '.' && (!IsDigit(state.NextChar) || state.IsLastChar) => state with { LexemeStartIndex = 0, Tokens = state.Tokens.Append(CreateDigitToken(state.LexemeStartIndex, cursor.index, state.line)), Expected = Expect.None },
                ('.', Expect.DotOrDigid, _, _) when IsDigit(state.NextChar) => state with { Expected = Expect.Digid },
                (_, Expect.Digid, _, _) when IsDigit(cursor.currentCharacter) && IsDigit(state.NextChar) => state with { Expected = Expect.Digid },
                (_, Expect.Digid, _, _) when IsDigit(cursor.currentCharacter) && (!IsDigit(state.NextChar) || state.IsLastChar) => state with { LexemeStartIndex = 0, Tokens = state.Tokens.Append(CreateDigitToken(state.LexemeStartIndex, cursor.index, state.line)), Expected = Expect.None },
                _ => state
            };
        
        public bool IsDigit(char c) => c >= '0' && c <= '9';

        public List<Token> Scan()
        {
            var tokens = new List<Token>();

            ScannerState state = source
                .Select((character, index) => (character, index))
                .Aggregate(new ScannerState(), (state, cursor) =>
                {
                    bool isLast = cursor.index >= source.Length -1;
                    char next = !isLast ? source[cursor.index + 1] : default;
                    return Match(state with { IsLastChar = isLast, NextChar = next }, cursor);
                });

            var z = state.Tokens.ToList();
            return z;
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
    }
}
