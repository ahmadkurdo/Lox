using Lox.Models;

namespace Lox.Exceptions
{
    public class RuntimeError : Exception
    {
        private Token Token { get; set; }

        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
