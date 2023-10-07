using Lox.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox.Test.Extenstions
{
    [TestClass]
    public class AstTest
    {
        [TestMethod]
        public void Print_BinaryExpr_PrintsCorrectly()
        {
            var binaryExpr = new Binary(new Literal(1), new Token(TokenType.PLUS, "+", null, 0), new Literal(2));
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(binaryExpr);

            Assert.AreEqual("(+ 1 2)", result);
        }

        [TestMethod]
        public void Print_GroupingExpr_PrintsCorrectly()
        {
            var groupingExpr = new Grouping(new Literal(42));
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(groupingExpr);

            Assert.AreEqual("(group 42)", result);
        }

        [TestMethod]
        public void Print_LiteralExpr_PrintsCorrectly()
        {
            var literalExpr = new Literal("Hello, World!");
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(literalExpr);

            Assert.AreEqual("Hello, World!", result);
        }

        [TestMethod]
        public void Print_UnaryExpr_PrintsCorrectly()
        {
            var unaryExpr = new Unary(new Token(TokenType.MINUS, "-", null, 0), new Literal(7));
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(unaryExpr);

            Assert.AreEqual("(- 7)", result);
        }

        [TestMethod]
        public void Print_BinaryExpr_PrintsCorrectly_Plus()
        {
            var binaryExpr = new Binary(new Literal(1), new Token(TokenType.PLUS, "+", null, 0), new Literal(2));
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(binaryExpr);

            Assert.AreEqual("(+ 1 2)", result);
        }

        [TestMethod]
        public void Print_MultipleNestedBinaryExprs_PrintsCorrectly()
        {
            // (1 + 2) * (3 - 4)
            var nestedExpr1 = new Binary(new Literal(1), new Token(TokenType.PLUS, "+", null, 0), new Literal(2));
            var nestedExpr2 = new Binary(new Literal(3), new Token(TokenType.MINUS, "-", null, 0), new Literal(4));
            var topLevelExpr = new Binary(nestedExpr1, new Token(TokenType.STAR, "*", null, 0), nestedExpr2);

            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(topLevelExpr);

            Assert.AreEqual("(* (+ 1 2) (- 3 4))", result);
        }

        [TestMethod]
        public void Print_MultipleNestedExpr_PrintsCorrectly()
        {
            var nestedExpr = new Binary(new Literal(1), new Token(TokenType.PLUS, "+", null, 0), new Grouping(new Literal(2)));
            var astPrinter = new AstPrinter();

            var result = astPrinter.Print(nestedExpr);

            Assert.AreEqual("(+ 1 (group 2))", result);
        }
    }
}
