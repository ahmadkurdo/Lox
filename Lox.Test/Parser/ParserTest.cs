using Lox.AST;
using Lox.Models;
using Lox.Parser;
using Lox.Test.TestHelpers;

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

        [TestMethod]
        public void Parser_Should_Parse_Equality_And_Comparison_Epxr()
        {
            var expression = "1 > 2 == 3 < 4";
            Scanner scanner = new Scanner(expression);
            var tokens = scanner.Scan();
            ParserState parserState = new ParserState(0, tokens);
            AstPrinter printer = new AstPrinter();
            Lox.Parser.Parser parser = new Lox.Parser.Parser(parserState);

            var res = parser.Parse();

            Assert.AreEqual(res.As(x => (Binary)x).Right.As(x => (Binary)x).Operation.Type, TokenType.LESS);
            Assert.AreEqual(3D, res.As(x => (Binary)x).Right.As(x => (Binary)x).Left.As(x => (Literal)x).Value);
            Assert.AreEqual(4D, res.As(x => (Binary)x).Right.As(x => (Binary)x).Right.As(x => (Literal)x).Value);
            Assert.AreEqual(res.As(x => (Binary)x).Left.As(x => (Binary)x).Operation.Type, TokenType.GREATER);
            Assert.AreEqual(1D, res.As(x => (Binary)x).Left.As(x => (Binary)x).Left.As(x => (Literal)x).Value);
            Assert.AreEqual(2D, res.As(x => (Binary)x).Left.As(x => (Binary)x).Right.As(x => (Literal)x).Value);
            Assert.AreEqual(res.As(x => (Binary)x).Operation.Type, TokenType.EQUAL_EQUAL);
            Assert.IsNotNull(res);
            Assert.IsTrue(res is Binary);
        }

        [TestMethod]
        public void Parser_Should_Parse_Addition_Expression()
        {
            var expression = "1 + 2";
            Scanner scanner = new Scanner(expression);
            var tokens = scanner.Scan();
            ParserState parserState = new ParserState(0, tokens);
            AstPrinter printer = new AstPrinter();
            Lox.Parser.Parser parser = new Lox.Parser.Parser(parserState);

            var res = parser.Parse();

            Assert.IsNotNull(res);
            Assert.IsTrue(res is Binary);
            Assert.AreEqual(printer.Print(res), "(+ 1 2)");
        }

        [TestMethod]
        public void Parser_Should_Parse_Multiplication_Precedence()
        {
            var expression = "1 + 2 * 3";
            Scanner scanner = new Scanner(expression);
            var tokens = scanner.Scan();
            ParserState parserState = new ParserState(0, tokens);
            AstPrinter printer = new AstPrinter();
            Lox.Parser.Parser parser = new Lox.Parser.Parser(parserState);

            var res = parser.Parse();

            Assert.IsNotNull(res);
            Assert.IsTrue(res is Binary);
            Assert.AreEqual(printer.Print(res), "(+ 1 (* 2 3))");
        }

        [TestMethod]
        public void Parser_Should_Parse_Grouped_Expressions()
        {
            var expression = "(1 + 2) * 3";
            Scanner scanner = new Scanner(expression);
            var tokens = scanner.Scan();
            ParserState parserState = new ParserState(0, tokens);
            AstPrinter printer = new AstPrinter();
            Lox.Parser.Parser parser = new Lox.Parser.Parser(parserState);

            var res = parser.Parse();

            Assert.IsNotNull(res);
            Assert.IsTrue(res is Binary);
            Assert.AreEqual("(* (group (+ 1 2)) 3)", printer.Print(res));
        }
    }
}
