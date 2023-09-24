using Lox.Extensions;
using Newtonsoft.Json.Linq;

namespace Lox.Test
{
    [TestClass]
    public class ScannerUnitTest
    {
        [TestMethod]
        public void Scan_Assignment_Statement_ReturnsCorrectTokens()
        {
            string source = "var name = \"John\"; 123";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(7, tokens.Count);

            Assert.AreEqual("var", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.VAR, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("name", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("=", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual("\"John\"", tokens[3].Lexeme);
            Assert.AreEqual("John", tokens[3].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual(";", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual("123", tokens[5].Lexeme);
            Assert.AreEqual(123D, tokens[5].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);
        }

        [TestMethod]
        public void Scan_Arithmatic_Expr_ReturnsCorrectTokens()
        {
            string source = "(1 + 2) - 3";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(8, tokens.Count);

            Assert.AreEqual("(", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("1", tokens[1].Lexeme);
            Assert.AreEqual(1d, tokens[1].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("+", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.PLUS, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual("2", tokens[3].Lexeme);
            Assert.AreEqual(2d, tokens[3].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual(")", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual("-", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.MINUS, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            Assert.AreEqual("3", tokens[6].Lexeme);
            Assert.AreEqual(3d, tokens[6].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);
        }

        [TestMethod]
        public void Scan_If_Statement_ReturnsCorrectTokens()
        {
            string source = "if(true){return \"Hello World!\"};";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(10, tokens.Count);

            Assert.AreEqual("if", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.IF, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("(", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("true", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.TRUE, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual(")", tokens[3].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual("{", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual("return", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.RETURN, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            Assert.AreEqual("\"Hello World!\"", tokens[6].Lexeme);
            Assert.AreEqual("Hello World!", tokens[6].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);

            Assert.AreEqual("}", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[7].Type);
            Assert.AreEqual(1, tokens[7].Line);

            Assert.AreEqual(";", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[8].Type);
            Assert.AreEqual(1, tokens[8].Line);
        }

        [TestMethod]
        public void Scan_DoubleQuotedString_ReturnsCorrectTokens()
        {
            // Arrange
            string source = "\"Hello, world!\"";
            var scanner = new Scanner(source);

            // Act
            List<Token> tokens = scanner.Scan();

            // Assert
            Assert.AreEqual(2, tokens.Count);

            // Token 0: "Hello, world!"
            Assert.AreEqual("\"Hello, world!\"", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.STRING, tokens[0].Type);
            Assert.AreEqual("Hello, world!", tokens[0].Literal);
            Assert.AreEqual(1, tokens[0].Line);
        }
    
        [TestMethod]
        public void Scan_CombinationOfLiteralsAndOperators_ReturnsCorrectToken()
        {
            string source = "12343\"Ahmed1233\"//comment should be ignored\n23213\"HelloWorld!\"\n123.5\"Test\"!=<=<>=>";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual("12343", tokens[0].Lexeme);
            Assert.AreEqual(12343D, tokens[0].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("\"Ahmed1233\"", tokens[1].Lexeme);
            Assert.AreEqual("Ahmed1233", tokens[1].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("23213", tokens[2].Lexeme);
            Assert.AreEqual(23213D, tokens[2].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[2].Type);
            Assert.AreEqual(2, tokens[2].Line);

            Assert.AreEqual("\"HelloWorld!\"", tokens[3].Lexeme);
            Assert.AreEqual("HelloWorld!", tokens[3].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[3].Type);
            Assert.AreEqual(2, tokens[3].Line);

            Assert.AreEqual("123.5", tokens[4].Lexeme);
            Assert.AreEqual(123.5D, tokens[4].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[4].Type);
            Assert.AreEqual(3, tokens[4].Line);

            Assert.AreEqual("\"Test\"", tokens[5].Lexeme);
            Assert.AreEqual("Test", tokens[5].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[5].Type);
            Assert.AreEqual(3, tokens[5].Line);

            Assert.AreEqual("!=", tokens[6].Lexeme);
            Assert.AreEqual(TokenType.BANG_EQUAL, tokens[6].Type);
            Assert.AreEqual(3, tokens[6].Line);

            Assert.AreEqual("<=", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.LESS_EQUAL, tokens[7].Type);
            Assert.AreEqual(3, tokens[7].Line);

            Assert.AreEqual("<", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.LESS, tokens[8].Type);
            Assert.AreEqual(3, tokens[8].Line);

            Assert.AreEqual(">=", tokens[9].Lexeme);
            Assert.AreEqual(TokenType.GREATER_EQUAL, tokens[9].Type);
            Assert.AreEqual(3, tokens[9].Line);

            Assert.AreEqual(">", tokens[10].Lexeme);
            Assert.AreEqual(TokenType.GREATER, tokens[10].Type);
            Assert.AreEqual(3, tokens[10].Line);
        }

        [TestMethod]
        public void Scan_Comments_ReturnsEmptyTokens()
        {
            string source = "// This is a comment\n123.5";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();
            
            Assert.AreEqual("123.5", tokens[0].Lexeme);
            Assert.AreEqual(123.5D, tokens[0].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[0].Type);
            Assert.AreEqual(2, tokens[0].Line);
            Assert.AreEqual(2, tokens.Count);
        }



        [TestMethod]
        public void Scan_Should_ReturnTokenOnCorrect_Line()
        {
            string source = "\"Hello, World!\"\n123.5";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual("\"Hello, World!\"", tokens[0].Lexeme);
            Assert.AreEqual("Hello, World!", tokens[0].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[0].Type);

            Assert.AreEqual("123.5", tokens[1].Lexeme);
            Assert.AreEqual(123.5D, tokens[1].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[1].Type);
            Assert.AreEqual(2, tokens[1].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_BangEqual()
        {
            string source = "!=\n123.5\n!=";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(4, tokens.Count);

            Assert.AreEqual("!=", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.BANG_EQUAL, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("!=", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.BANG_EQUAL, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_Bang()
        {
            string source = "!\n123.5\n!";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(4, tokens.Count);

            Assert.AreEqual("!", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.BANG, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("!", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.BANG, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_Equal()
        {
            string source = "=\n123.5\n=";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(4, tokens.Count);

            Assert.AreEqual("=", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("=", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_EqualEqual()
        {
            string source = "==\n123.5\n===";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(5, tokens.Count);

            Assert.AreEqual("==", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.EQUAL_EQUAL, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("==", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.EQUAL_EQUAL, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_Equal_As_String_In_A_String()
        {
            string source = "\"Ahmed1233==\"\n\"HelloWorld=\"\n\"Test\"";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.IsTrue(tokens.All(token => token.Type == TokenType.STRING || token.Type == TokenType.EOF));
        }

        [TestMethod]
        public void Scan_Should_Recognize_Less()
        {
            string source = "<\n123.5\n<";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(4, tokens.Count);

            Assert.AreEqual("<", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.LESS, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("<", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.LESS, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_LessEqual()
        {
            string source = "<=\n123.5\n<==";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(5, tokens.Count);

            Assert.AreEqual("<=", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.LESS_EQUAL, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("<=", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.LESS_EQUAL, tokens[2].Type);
            Assert.AreEqual(3, tokens[2].Line);
        }

        [TestMethod]
        public void Scan_Should_Recognize_Less_As_String_In_A_String()
        {
            string source = "\"Ahmed1233<=\"\n\"HelloWorld<\"\n\"Test<\"";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.IsTrue(tokens.All(token => token.Type == TokenType.STRING || token.Type == TokenType.EOF));
        }

        [TestMethod]
        public void Scan_Should_Recognize_Bang_As_String_In_A_String()
        {
            string source = "\"Ahmed1233\"\n\"HelloWorld!\"\n\"Test\"";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.IsTrue(tokens.All(token => token.Type == TokenType.STRING || token.Type == TokenType.EOF));
        }

        [TestMethod]
        public void Scan_EmptySource_ReturnsEmptyTokens()
        {
            string source = "";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void Scan_WhitespaceSource_ReturnsEmptyTokens()
        {
            string source = "    \t  \r";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void Scan_SpecialCharacters_ReturnsCorrectTokens()
        {
            string source = "( ) { } , ;";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[0].Type);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[1].Type);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[2].Type);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[3].Type);
            Assert.AreEqual(TokenType.COMMA, tokens[4].Type);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[5].Type);
        }

        [TestMethod]
        public void Scan_Should_ReturnKeywordTokens()
        {
            var scanner = new Scanner("class fun var if else");
            List<Token> tokens = scanner.Scan();

            Assert.IsNotNull(tokens);
            Assert.AreEqual(6, tokens.Count);
            Assert.AreEqual(TokenType.CLASS, tokens[0].Type);
            Assert.AreEqual(TokenType.FUN, tokens[1].Type);
            Assert.AreEqual(TokenType.VAR, tokens[2].Type);
            Assert.AreEqual(TokenType.IF, tokens[3].Type);
            Assert.AreEqual(TokenType.ELSE, tokens[4].Type);
        }

        [TestMethod]
        public void Scan_Should_ReturnEOFTokenForUnterminatedString()
        {
            var scanner = new Scanner("\"Unterminated string");
            List<Token> tokens = scanner.Scan();

            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.EOF, tokens[0].Type);
        }

        [TestMethod]
        public void Scan_Should_ReturnBooleanTokens()
        {
            var scanner = new Scanner("true false");
            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(3, tokens.Count);

            Assert.AreEqual("true", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.TRUE, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("false", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.FALSE, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);
        }

        [TestMethod]
        public void Scan_Should_IgnoreSingleLineComment()
        {
            var scanner = new Scanner("int x = 42; // This is a comment");
            List<Token> tokens = scanner.Scan();
            Assert.AreEqual(6, tokens.Count);
        }

        [TestMethod]
        public void Scan_ForLoop_Should_ReturnCorrectTokens()
        {
            var scanner = new Scanner("for (int i = 0; i < 5; i = i + 1) { print(i); }");
            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(25, tokens.Count);

            Assert.AreEqual("for", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.FOR, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("(", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("int", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual("i", tokens[3].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual("=", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual("0", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            Assert.AreEqual(";", tokens[6].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);

            Assert.AreEqual("i", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[7].Type);
            Assert.AreEqual(1, tokens[7].Line);

            Assert.AreEqual("<", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.LESS, tokens[8].Type);
            Assert.AreEqual(1, tokens[8].Line);

            Assert.AreEqual("5", tokens[9].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[9].Type);
            Assert.AreEqual(1, tokens[9].Line);

            Assert.AreEqual(";", tokens[10].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[10].Type);
            Assert.AreEqual(1, tokens[10].Line);

            Assert.AreEqual("i", tokens[11].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[11].Type);
            Assert.AreEqual(1, tokens[11].Line);

            Assert.AreEqual("=", tokens[12].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[12].Type);
            Assert.AreEqual(1, tokens[12].Line);

            Assert.AreEqual("i", tokens[13].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[13].Type);
            Assert.AreEqual(1, tokens[13].Line);

            Assert.AreEqual("+", tokens[14].Lexeme);
            Assert.AreEqual(TokenType.PLUS, tokens[14].Type);
            Assert.AreEqual(1, tokens[14].Line);

            Assert.AreEqual("1", tokens[15].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[15].Type);
            Assert.AreEqual(1, tokens[15].Line);

            Assert.AreEqual(")", tokens[16].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[16].Type);
            Assert.AreEqual(1, tokens[16].Line);

            Assert.AreEqual("{", tokens[17].Lexeme);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[17].Type);
            Assert.AreEqual(1, tokens[17].Line);

            Assert.AreEqual("print", tokens[18].Lexeme);
            Assert.AreEqual(TokenType.PRINT, tokens[18].Type);
            Assert.AreEqual(1, tokens[18].Line);

            Assert.AreEqual("(", tokens[19].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[19].Type);
            Assert.AreEqual(1, tokens[19].Line);


            Assert.AreEqual("i", tokens[20].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[20].Type);
            Assert.AreEqual(1, tokens[20].Line);

            Assert.AreEqual(")", tokens[21].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[21].Type);
            Assert.AreEqual(1, tokens[21].Line);

            Assert.AreEqual(";", tokens[22].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[22].Type);
            Assert.AreEqual(1, tokens[22].Line);

            Assert.AreEqual("}", tokens[23].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[23].Type);
            Assert.AreEqual(1, tokens[23].Line);
        }

        [TestMethod]
        public void Scan_IfElseStatement_ReturnsCorrectTokens()
        {
            // Arrange
            string source = "if (condition) { statement1; } else { statement2; }";
            var scanner = new Scanner(source);

            // Act
            List<Token> tokens = scanner.Scan();

            // Assert
            Assert.AreEqual(14, tokens.Count);

            // Token 0: if
            Assert.AreEqual("if", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.IF, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            // Token 1: (
            Assert.AreEqual("(", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            // Token 2: condition
            Assert.AreEqual("condition", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            // Token 3: )
            Assert.AreEqual(")", tokens[3].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            // Token 4: {
            Assert.AreEqual("{", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            // Token 5: statement1
            Assert.AreEqual("statement1", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            // Token 6: ;
            Assert.AreEqual(";", tokens[6].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);

            // Token 7: }
            Assert.AreEqual("}", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[7].Type);
            Assert.AreEqual(1, tokens[7].Line);

            // Token 8: else
            Assert.AreEqual("else", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.ELSE, tokens[8].Type);
            Assert.AreEqual(1, tokens[8].Line);

            // Token 9: {
            Assert.AreEqual("{", tokens[9].Lexeme);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[9].Type);
            Assert.AreEqual(1, tokens[9].Line);

            // Token 10: statement2
            Assert.AreEqual("statement2", tokens[10].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[10].Type);
            Assert.AreEqual(1, tokens[10].Line);

            // Token 11: ;
            Assert.AreEqual(";", tokens[11].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[11].Type);
            Assert.AreEqual(1, tokens[11].Line);

            // Token 12: }
            Assert.AreEqual("}", tokens[12].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[12].Type);
            Assert.AreEqual(1, tokens[12].Line);
        }


        [TestMethod]
        public void Scan_WhileLoop_Should_ReturnCorrectTokens()
        {
            var scanner = new Scanner("while (x < 10) { x = x + 1; }");
            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(15, tokens.Count);

            Assert.AreEqual("while", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.WHILE, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("(", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("x", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual("<", tokens[3].Lexeme);
            Assert.AreEqual(TokenType.LESS, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual("10", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual(")", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            Assert.AreEqual("{", tokens[6].Lexeme);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);

            Assert.AreEqual("x", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[7].Type);
            Assert.AreEqual(1, tokens[7].Line);

            Assert.AreEqual("=", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[8].Type);
            Assert.AreEqual(1, tokens[8].Line);

            Assert.AreEqual("x", tokens[9].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[9].Type);
            Assert.AreEqual(1, tokens[9].Line);

            Assert.AreEqual("+", tokens[10].Lexeme);
            Assert.AreEqual(TokenType.PLUS, tokens[10].Type);
            Assert.AreEqual(1, tokens[10].Line);

            Assert.AreEqual("1", tokens[11].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[11].Type);
            Assert.AreEqual(1, tokens[11].Line);

            Assert.AreEqual(";", tokens[12].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[12].Type);
            Assert.AreEqual(1, tokens[12].Line);

            Assert.AreEqual("}", tokens[13].Lexeme);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[13].Type);
            Assert.AreEqual(1, tokens[13].Line);
        }

        [TestMethod]
        public void Scan_Should_ReturnIdentifierTokens()
        {
            var scanner = new Scanner("var foo = 123; var bar = \"abc\";");
            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(11, tokens.Count);

            Assert.AreEqual("var", tokens[0].Lexeme);
            Assert.AreEqual(TokenType.VAR, tokens[0].Type);
            Assert.AreEqual(1, tokens[0].Line);

            Assert.AreEqual("foo", tokens[1].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual(1, tokens[1].Line);

            Assert.AreEqual("=", tokens[2].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[2].Type);
            Assert.AreEqual(1, tokens[2].Line);

            Assert.AreEqual("123", tokens[3].Lexeme);
            Assert.AreEqual(TokenType.NUMBER, tokens[3].Type);
            Assert.AreEqual(1, tokens[3].Line);

            Assert.AreEqual(";", tokens[4].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[4].Type);
            Assert.AreEqual(1, tokens[4].Line);

            Assert.AreEqual("var", tokens[5].Lexeme);
            Assert.AreEqual(TokenType.VAR, tokens[5].Type);
            Assert.AreEqual(1, tokens[5].Line);

            Assert.AreEqual("bar", tokens[6].Lexeme);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[6].Type);
            Assert.AreEqual(1, tokens[6].Line);

            Assert.AreEqual("=", tokens[7].Lexeme);
            Assert.AreEqual(TokenType.EQUAL, tokens[7].Type);
            Assert.AreEqual(1, tokens[7].Line);

            Assert.AreEqual("\"abc\"", tokens[8].Lexeme);
            Assert.AreEqual(TokenType.STRING, tokens[8].Type);
            Assert.AreEqual("abc", tokens[8].Literal);
            Assert.AreEqual(1, tokens[8].Line);

            Assert.AreEqual(";", tokens[9].Lexeme);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[9].Type);
            Assert.AreEqual(1, tokens[9].Line);
        }


        [TestMethod]
        public void Scan_Keywords_ReturnsCorrectTokens()
        {
            string source = "if else while";
            var scanner = new Scanner(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(TokenType.IF, tokens[0].Type);
            Assert.AreEqual(TokenType.ELSE, tokens[1].Type);
            Assert.AreEqual(TokenType.WHILE, tokens[2].Type);
        }
    }
}