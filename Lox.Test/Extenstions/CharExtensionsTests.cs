using Lox.Extensions;

namespace Lox.Test.Extenstions
{
    [TestClass]
    public class CharExtensionsTests
    {
        [TestMethod]
        public void IsDigit_Should_ReturnTrue_ForValidDigits()
        {
            Assert.IsTrue('0'.IsDigit());
            Assert.IsTrue('1'.IsDigit());
            Assert.IsTrue('2'.IsDigit());
            Assert.IsTrue('3'.IsDigit());
            Assert.IsTrue('4'.IsDigit());
            Assert.IsTrue('5'.IsDigit());
            Assert.IsTrue('6'.IsDigit());
            Assert.IsTrue('7'.IsDigit());
            Assert.IsTrue('8'.IsDigit());
            Assert.IsTrue('9'.IsDigit());
        }

        [TestMethod]
        public void IsDigit_Should_ReturnFalse_ForNonDigits()
        {

            // Non-digit characters
            Assert.IsFalse('A'.IsDigit());
            Assert.IsFalse('B'.IsDigit());
            Assert.IsFalse('C'.IsDigit());
            Assert.IsFalse('D'.IsDigit());
            Assert.IsFalse('E'.IsDigit());
            Assert.IsFalse('F'.IsDigit());
            Assert.IsFalse('G'.IsDigit());
            Assert.IsFalse('H'.IsDigit());
            Assert.IsFalse('I'.IsDigit());
            Assert.IsFalse('J'.IsDigit());
            Assert.IsFalse('K'.IsDigit());
            Assert.IsFalse('L'.IsDigit());
            Assert.IsFalse('M'.IsDigit());
            Assert.IsFalse('N'.IsDigit());
            Assert.IsFalse('O'.IsDigit());
            Assert.IsFalse('P'.IsDigit());
            Assert.IsFalse('Q'.IsDigit());
            Assert.IsFalse('R'.IsDigit());
            Assert.IsFalse('S'.IsDigit());
            Assert.IsFalse('T'.IsDigit());
            Assert.IsFalse('U'.IsDigit());
            Assert.IsFalse('V'.IsDigit());
            Assert.IsFalse('W'.IsDigit());
            Assert.IsFalse('X'.IsDigit());
            Assert.IsFalse('Y'.IsDigit());
            Assert.IsFalse('Z'.IsDigit());

            Assert.IsFalse('a'.IsDigit());
            Assert.IsFalse('b'.IsDigit());
            Assert.IsFalse('c'.IsDigit());
            Assert.IsFalse('d'.IsDigit());
            Assert.IsFalse('e'.IsDigit());
            Assert.IsFalse('f'.IsDigit());
            Assert.IsFalse('g'.IsDigit());
            Assert.IsFalse('h'.IsDigit());
            Assert.IsFalse('i'.IsDigit());
            Assert.IsFalse('j'.IsDigit());
            Assert.IsFalse('k'.IsDigit());
            Assert.IsFalse('l'.IsDigit());
            Assert.IsFalse('m'.IsDigit());
            Assert.IsFalse('n'.IsDigit());
            Assert.IsFalse('o'.IsDigit());
            Assert.IsFalse('p'.IsDigit());
            Assert.IsFalse('q'.IsDigit());
            Assert.IsFalse('r'.IsDigit());
            Assert.IsFalse('s'.IsDigit());
            Assert.IsFalse('t'.IsDigit());
            Assert.IsFalse('u'.IsDigit());
            Assert.IsFalse('v'.IsDigit());
            Assert.IsFalse('w'.IsDigit());
            Assert.IsFalse('x'.IsDigit());
            Assert.IsFalse('y'.IsDigit());
            Assert.IsFalse('z'.IsDigit());
            Assert.IsFalse('"'.IsDigit());
        }
        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForExclamationMark()
        {

            char token = '!';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForGreaterThan()
        {
            char token = '>';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForLessThan()
        {
            char token = '<';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForEqualSign()
        {
            char token = '=';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForPlus()
        {
            char token = '+';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForMinus()
        {
            char token = '-';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForSlash()
        {
            char token = '/';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnTrue_ForAsterisk()
        {
            char token = '*';
            bool result = token.IsOperator();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOperator_Should_ReturnFalse_ForOtherCharacters()
        {
            char token = 'A'; // Any character that's not in the switch statement
            bool result = token.IsOperator();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDigit_Should_ReturnFalse_ForNullCharacter()
        {
            // Null character input
            Assert.IsFalse('\0'.IsDigit());
        }
        
        [TestMethod]
        public void IsAlpha_Should_ReturnTrue_ForAlphabeticCharacters()
        {
            // Test lowercase alphabets
            for (char c = 'a'; c <= 'z'; c++)
            {
                Assert.IsTrue(c.IsAlpha());
            }

            // Test uppercase alphabets
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Assert.IsTrue(c.IsAlpha());
            }

            // Test underscore
            Assert.IsTrue('_'.IsAlpha());
        }

        [TestMethod]
        public void IsAlpha_Should_ReturnFalse_ForNonAlphabeticCharacters()
        {
            // Test digits
            for (char c = '0'; c <= '9'; c++)
            {
                Assert.IsFalse(c.IsAlpha());
            }

            // Test non-alphabetic characters
            Assert.IsFalse(' '.IsAlpha());
            Assert.IsFalse('$'.IsAlpha());
            Assert.IsFalse('%'.IsAlpha());
            Assert.IsFalse('*'.IsAlpha());
            // Add more test cases for non-alphabetic characters as needed
        }

        [TestMethod]
        public void IsAlphaNumeric_Should_ReturnTrue_ForAlphanumericCharacters()
        {
            // Test alphabets
            for (char c = 'a'; c <= 'z'; c++)
            {
                Assert.IsTrue(c.IsAlphaNumeric());
            }

            // Test digits
            for (char c = '0'; c <= '9'; c++)
            {
                Assert.IsTrue(c.IsAlphaNumeric());
            }

            // Test underscore
            Assert.IsTrue('_'.IsAlphaNumeric());
        }

        [TestMethod]
        public void IsAlphaNumeric_Should_ReturnFalse_ForNonAlphanumericCharacters()
        {
            // Test non-alphanumeric characters
            Assert.IsFalse(' '.IsAlphaNumeric());
            Assert.IsFalse('$'.IsAlphaNumeric());
            Assert.IsFalse('%'.IsAlphaNumeric());
            Assert.IsFalse('*'.IsAlphaNumeric());
            // Add more test cases for non-alphanumeric characters as needed
        }
    }
}
