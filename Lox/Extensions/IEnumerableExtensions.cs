using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string substring(this string input, int beginIndex, int endIndex)
        {
            if (beginIndex < 0 || beginIndex >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(beginIndex), "Invalid begin index");
            }

            if (endIndex < beginIndex || endIndex > input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(endIndex), "Invalid end index");
            }

            int length = endIndex - beginIndex;
            return input.Substring(beginIndex, length);
        }
    }

}

