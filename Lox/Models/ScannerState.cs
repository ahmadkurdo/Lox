using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Models
{
    public record ScannerState(int LexemeStartIndex, int line, IEnumerable<Token> Tokens, Expect Expected) 
    {
        public ScannerState() : this(0, 1, new List<Token>(), Expect.None) { }

        public ScannerState AddToken(Token token) => this with { Tokens = Tokens.Append(token) };

        public ScannerState Reset() => this with { LexemeStartIndex = 0, Expected = Expect.None };
    
    }
}
