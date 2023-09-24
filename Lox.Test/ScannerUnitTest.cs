using Newtonsoft.Json.Linq;

namespace Lox.Test
{
    [TestClass]
    public class ScannerUnitTest
    {


        [TestMethod]
        public void CreateStringToken_Should_TokenFromCorrectPositionInSource()
        {
            string source = "12343\"Ahmed1233\"23213\"HelloWorld!\"432234\"Test\"";
            var x = source.Select((x, index) => (x, index)).ToList();
            var scanner = new ScannerV2(source);

            Token ahmedToken = scanner.CreateStringToken(5,15,1);
            Token helloWorldToken = scanner.CreateStringToken(21, 33, 1);
            Token testToken = scanner.CreateStringToken(40, 45, 1);
            
            Assert.AreEqual("\"Ahmed1233\"", ahmedToken.Lexeme);
            Assert.AreEqual("Ahmed1233", ahmedToken.Literal.ToString());
            Assert.AreEqual("\"HelloWorld!\"", helloWorldToken.Lexeme);
            Assert.AreEqual("HelloWorld!", helloWorldToken.Literal.ToString());
            Assert.AreEqual("\"Test\"", testToken.Lexeme);
            Assert.AreEqual("Test", testToken.Literal.ToString());
        }

        [TestMethod]
        public void CreateDigidToken_Should_TokenFromCorrectPositionInSource()
        {
            string source = "123.5";
            var x = source.Select((x, index) => (x, index)).ToList();
            var scanner = new ScannerV2(source);

            Token res = scanner.CreateDigitToken(0, 4, 1);

            Assert.AreEqual("123.5", res.Lexeme);
            Assert.AreEqual(123.5D, res.Literal);
        }

        [TestMethod]
        public void Scan_StringAndNumberLiteral_ReturnsCorrectToken()
        {
            string source = "12343\"Ahmed1233\"\n23213\"HelloWorld!\"\n123.5\"Test\"";
            var scanner = new ScannerV2(source);

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
        }

        [TestMethod]
        public void Scan_Comments_ReturnsEmptyTokens()
        {
            string source = "// This is a comment\n123.5";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();
            
            Assert.AreEqual("123.5", tokens[0].Lexeme);
            Assert.AreEqual(123.5D, tokens[0].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[0].Type);
            Assert.AreEqual(2, tokens[0].Line);
            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void IsDigit_Should_ReturnTrue_ForValidDigits()
        {
            var scanner = new ScannerV2("");

            // Valid digit characters
            Assert.IsTrue(scanner.IsDigit('0'));
            Assert.IsTrue(scanner.IsDigit('1'));
            Assert.IsTrue(scanner.IsDigit('2'));
            Assert.IsTrue(scanner.IsDigit('3'));
            Assert.IsTrue(scanner.IsDigit('4'));
            Assert.IsTrue(scanner.IsDigit('5'));
            Assert.IsTrue(scanner.IsDigit('6'));
            Assert.IsTrue(scanner.IsDigit('7'));
            Assert.IsTrue(scanner.IsDigit('8'));
            Assert.IsTrue(scanner.IsDigit('9'));
        }

        [TestMethod]
        public void IsDigit_Should_ReturnFalse_ForNonDigits()
        {
            var scanner = new ScannerV2("");

            // Non-digit characters
            Assert.IsFalse(scanner.IsDigit('A'));
            Assert.IsFalse(scanner.IsDigit('B'));
            Assert.IsFalse(scanner.IsDigit('C'));
            Assert.IsFalse(scanner.IsDigit('D'));
            Assert.IsFalse(scanner.IsDigit('E'));
            Assert.IsFalse(scanner.IsDigit('F'));
            Assert.IsFalse(scanner.IsDigit('G'));
            Assert.IsFalse(scanner.IsDigit('H'));
            Assert.IsFalse(scanner.IsDigit('I'));
            Assert.IsFalse(scanner.IsDigit('J'));
            Assert.IsFalse(scanner.IsDigit('K'));
            Assert.IsFalse(scanner.IsDigit('L'));
            Assert.IsFalse(scanner.IsDigit('M'));
            Assert.IsFalse(scanner.IsDigit('N'));
            Assert.IsFalse(scanner.IsDigit('O'));
            Assert.IsFalse(scanner.IsDigit('P'));
            Assert.IsFalse(scanner.IsDigit('Q'));
            Assert.IsFalse(scanner.IsDigit('R'));
            Assert.IsFalse(scanner.IsDigit('S'));
            Assert.IsFalse(scanner.IsDigit('T'));
            Assert.IsFalse(scanner.IsDigit('U'));
            Assert.IsFalse(scanner.IsDigit('V'));
            Assert.IsFalse(scanner.IsDigit('W'));
            Assert.IsFalse(scanner.IsDigit('X'));
            Assert.IsFalse(scanner.IsDigit('Y'));
            Assert.IsFalse(scanner.IsDigit('Z'));

            Assert.IsFalse(scanner.IsDigit('a'));
            Assert.IsFalse(scanner.IsDigit('b'));
            Assert.IsFalse(scanner.IsDigit('c'));
            Assert.IsFalse(scanner.IsDigit('d'));
            Assert.IsFalse(scanner.IsDigit('e'));
            Assert.IsFalse(scanner.IsDigit('f'));
            Assert.IsFalse(scanner.IsDigit('g'));
            Assert.IsFalse(scanner.IsDigit('h'));
            Assert.IsFalse(scanner.IsDigit('i'));
            Assert.IsFalse(scanner.IsDigit('j'));
            Assert.IsFalse(scanner.IsDigit('k'));
            Assert.IsFalse(scanner.IsDigit('l'));
            Assert.IsFalse(scanner.IsDigit('m'));
            Assert.IsFalse(scanner.IsDigit('n'));
            Assert.IsFalse(scanner.IsDigit('o'));
            Assert.IsFalse(scanner.IsDigit('p'));
            Assert.IsFalse(scanner.IsDigit('q'));
            Assert.IsFalse(scanner.IsDigit('r'));
            Assert.IsFalse(scanner.IsDigit('s'));
            Assert.IsFalse(scanner.IsDigit('t'));
            Assert.IsFalse(scanner.IsDigit('u'));
            Assert.IsFalse(scanner.IsDigit('v'));
            Assert.IsFalse(scanner.IsDigit('w'));
            Assert.IsFalse(scanner.IsDigit('x'));
            Assert.IsFalse(scanner.IsDigit('y'));
            Assert.IsFalse(scanner.IsDigit('z'));
            Assert.IsFalse(scanner.IsDigit('"'));

            // Add more non-digit characters as needed
        }

        [TestMethod]
        public void IsDigit_Should_ReturnFalse_ForNullCharacter()
        {
            var scanner = new ScannerV2("");

            // Null character input
            Assert.IsFalse(scanner.IsDigit('\0'));
        }


        [TestMethod]
        public void Scan_Should_ReturnTokenOnCorrect_Line()
        {
            string source = "\"Hello, World!\"\n123.5";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual("\"Hello, World!\"", tokens[0].Lexeme);
            Assert.AreEqual("Hello, World!", tokens[0].Literal);
            Assert.AreEqual(TokenType.STRING, tokens[0].Type);

            Assert.AreEqual("123.5", tokens[1].Lexeme);
            Assert.AreEqual(123.5D, tokens[1].Literal);
            Assert.AreEqual(TokenType.NUMBER, tokens[1].Type);
            Assert.AreEqual(2, tokens[1].Line);
        }

        [TestMethod]
        public void Scan_EmptySource_ReturnsEmptyTokens()
        {
            string source = "";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(0, tokens.Count);
        }

        [TestMethod]
        public void Scan_WhitespaceSource_ReturnsEmptyTokens()
        {
            string source = "    \t  \r";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(0, tokens.Count);
        }
        
        public void Scan_SpecialCharacters_ReturnsCorrectTokens()
        {
            string source = "( ) { } [ ] , ;";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual(TokenType.LEFT_PAREN, tokens[0].Type);
            Assert.AreEqual(TokenType.RIGHT_PAREN, tokens[1].Type);
            Assert.AreEqual(TokenType.LEFT_BRACE, tokens[2].Type);
            Assert.AreEqual(TokenType.RIGHT_BRACE, tokens[3].Type);
            Assert.AreEqual(TokenType.COMMA, tokens[6].Type);
        }

        
        public void Scan_Keywords_ReturnsCorrectTokens()
        {
            string source = "if else while";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual(TokenType.IF, tokens[0].Type);
            Assert.AreEqual(TokenType.ELSE, tokens[1].Type);
            Assert.AreEqual(TokenType.WHILE, tokens[2].Type);
        }

        public void Scan_MixOfTokens_ReturnsCorrectTokens()
        {
            string source = "var name = \"John\"; 123";
            var scanner = new ScannerV2(source);

            List<Token> tokens = scanner.Scan();

            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(TokenType.VAR, tokens[0].Type);
            Assert.AreEqual(TokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual(TokenType.EQUAL, tokens[2].Type);
            Assert.AreEqual(TokenType.STRING, tokens[3].Type);
            Assert.AreEqual(TokenType.SEMICOLON, tokens[4].Type);
        }
    }
}