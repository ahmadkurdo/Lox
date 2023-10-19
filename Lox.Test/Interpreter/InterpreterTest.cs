using Lox.AST;
using Lox.Exceptions;
using Lox.Models;
using Lox.Parser;
using Lox.Test.TestHelpers;

namespace Lox.Test.Interpreter
{
    [TestClass]
    public class InterpreterTests
    {
        [TestMethod]
        public void TestBinaryExpression_Addition()
        {
            var interpreter = new Interperter();
            var expr = new Binary(
                new Literal(1.0),
                new Token(TokenType.PLUS, "+", null, 1),
                new Literal(2.0)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(3.0, result);
        }

        [TestMethod]
        public void TestBinaryExpression_Concatenation()
        {
            var interpreter = new Interperter();
            var expr = new Binary(
                new Literal("Hello"),
                new Token(TokenType.PLUS, "+", null, 1),
                new Literal(" World")
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual("Hello World", result);
        }

        [TestMethod]
        public void TestUnaryExpression_Minus()
        {
            var interpreter = new Interperter();
            var expr = new Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(5.0)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(-5.0, result);
        }

        [TestMethod]
        public void TestGroupingExpression()
        {
            var interpreter = new Interperter();
            var expr = new Grouping(new Literal(42.0));

            var result = interpreter.Eval(expr);

            Assert.AreEqual(42.0, result);
        }

        [TestMethod]
        public void TestBinaryExpression_Subtraction()
        {
            var interpreter = new Interperter();
            var expr = new Binary(
                new Literal(5.0),
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(2.0)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(3.0, result);
        }

        [TestMethod]
        public void TestBinaryExpression_Multiplication()
        {
            var interpreter = new Interperter();
            var expr = new Binary(
                new Literal(3.0),
                new Token(TokenType.STAR, "*", null, 1),
                new Literal(4.0)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(12.0, result);
        }

        [TestMethod]
        public void TestBinaryExpression_Division()
        {
            var interpreter = new Interperter();
            var expr = new Binary(
                new Literal(8.0),
                new Token(TokenType.SLASH, "/", null, 1),
                new Literal(2.0)
            );

            double result = (double)interpreter.Eval(expr);

            Assert.AreEqual(4.0D, result);
        }

        [TestMethod]
        public void TestUnaryExpression_Negation()
        {
            var interpreter = new Interperter();
            var expr = new Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(7.0)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(-7.0, result);
        }

        [TestMethod]
        public void TestUnaryExpression_LogicalNot()
        {
            var interpreter = new Interperter();
            var expr = new Unary(
                new Token(TokenType.BANG, "!", null, 1),
                new Literal(false)
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestGroupingExpression_Nested()
        {
            var interpreter = new Interperter();
            var expr = new Grouping(
                new Binary(
                    new Literal(1.0),
                    new Token(TokenType.PLUS, "+", null, 1),
                    new Binary(
                        new Literal(2.0),
                        new Token(TokenType.STAR, "*", null, 1),
                        new Literal(3.0)
                    )
                )
            );

            var result = interpreter.Eval(expr);

            Assert.AreEqual(7.0, result);
        }

        [TestMethod]
        public void TestEquality_GT_Expression()
        {
            var interpreter = new Interperter();
            var expr = "1 > 2;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();
            
            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestEquality_LT_Expression()
        {
            var interpreter = new Interperter();
            var expr = "1 < 2;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();
            
            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestEquality_EQ_EQ_Expression()
        {
            var interpreter = new Interperter();
            var expr = "1 == 2;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();
            
            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestEquality_EQ_EQ_NULL_Expression()
        {
            var interpreter = new Interperter();
            var expr = "nil == nil;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestEquality_EQ_EQ_RightSide_NULL_Expression()
        {
            var interpreter = new Interperter();
            var expr = "1 == nil;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestEquality_NOT_EQ_Expression()
        {
            var interpreter = new Interperter();
            var expr = "1 != 2;";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();
            
            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestEquality_Addition_String_Expression()
        {
            var interpreter = new Interperter();
            var expr = " \"Ahmed\" +  \" \" + \"Rashid\"; ";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            var result = interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);

            Assert.AreEqual("Ahmed Rashid", result);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestBinary_Plus_ExprWithNonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "10 + \"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestBinary_GT_ExprWithNonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "10 > \"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestBinary_LT_Expr_WithNonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "10 < \"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestBinary_Slash_Expr_With_NonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "10 / \"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestBinary_Minus_Expr_With_NonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "10 - \"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void TestUnary_Minus_Expr_With_NonNumericRightOperand_Should_Throw_Exception()
        {
            var interpreter = new Interperter();
            var expr = "-\"20\";";
            Scanner scanner = new Scanner(expr);
            var tokens = scanner.Scan();
            var state = new ParserState(0, tokens);
            var parser = new Lox.Parser.Parser(state);
            var ast = parser.Parse();

            // Act and Assert
            interpreter.Eval(ast.First().As(stmt => (ExpressionStmt)stmt).expression);
        }
    }
}
