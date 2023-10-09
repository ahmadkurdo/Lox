using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Test.TestHelpers
{
    internal static class TypeExtensions
    {
        public static U As<T, U>(this T type, Func<T, U> f) => f(type);
    }
}
