using Lox.AST;
using Lox.Parser;

namespace Lox.Test.Parser
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Parser_Should_Parse_Equality_Epxr() 
        {
            Token one = new Token(TokenType.NUMBER, "1", 1, 1);
            Token greater = new Token(TokenType.GREATER, ">", null, 1);
            Token two  = new Token(TokenType.NUMBER, "2", 2, 1);
            Token eof = new Token(TokenType.EOF, null, null, 1);
            List<Token> tokens = new List<Token>()
            {
                one, greater, two, eof
            };

            ParserState parserState = new ParserState(0, tokens);
            AstPrinter printer = new AstPrinter();
            Lox.Parser.Parser parser = new Lox.Parser.Parser(parserState);

            var res = parser.Parse();

            Assert.IsNotNull(res);
            Assert.IsTrue(res is Binary);
            Assert.AreEqual(printer.Print(res), "(> 1 2)");
        }
    }
}
