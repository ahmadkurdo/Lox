using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Models
{
    public record ScannerState(int LexemeStartIndex, int line, IEnumerable<Token> Tokens, Expect Expected, char NextChar, bool IsLastChar) 
    {
        public ScannerState() : this(0, 1, new List<Token>(), Expect.None, '\0', false) { }
    
    }
}
