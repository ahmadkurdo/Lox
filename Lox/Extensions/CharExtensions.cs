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

        public static bool IsOperator(this char token) =>
            token switch
            {
                '!' => true,
                '>' => true,
                '<' => true,
                '=' => true,
                '+' => true,
                '-' => true,
                '/' => true,
                '*' => true,
                _ => false
            };

        public static bool IsAlpha(this char c) =>
            (c >= 'a' && c <= 'z') ||
            (c >= 'A' && c <= 'Z') ||
            c == '_';

        public static bool IsAlphaNumeric(this char c) => c.IsDigit() || c.IsAlpha();
    }
}
