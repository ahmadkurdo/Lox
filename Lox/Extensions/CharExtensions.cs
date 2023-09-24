using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Extensions
{
    public static class CharExtensions
    {
        public static bool IsDigit(this char c) => c >= '0' && c <= '9';

        public static bool IsTokenType(this char token) =>
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
