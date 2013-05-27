using System;
using System.Globalization;
using NUnit.Framework;
using Digbyswift.Utils.Extensions;

namespace Digbyswift.Utils.Tests
{
    [TestFixture]
    public class StringExtensionTests
    {

        #region Methods: ToTitleCase

        [Test]
        public void ToTitleCase_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            var culture = new CultureInfo("en-GB");
            // Act
            var valueToTest = String.Empty.ToTitleCase(culture);

            // Assert
            Assert.That(valueToTest, Is.Empty);
        }

        [Test]
        public void ToTitleCase_Null_ReturnsNull()
        {
            // Arrange
            string value = null;
            var culture = new CultureInfo("en-GB");
            // Act
            value = value.ToTitleCase(culture);

            // Assert
            Assert.IsNull(value);
        }

        [TestCase("Lorem ipsum dolor sit amet")]
        [TestCase("lorem ipsum dolor sit amet")]
        [TestCase("LoReM ipSuM doLor sit amEt")]
        public void ToTitleCase_ValidString_ReturnsTitleCase(string value)
        {
            // Arrange
            var culture = new CultureInfo("en-GB");
            // Act
            var valueToTest = value.ToTitleCase(culture);

            // Assert
            Assert.That(valueToTest, Is.EqualTo("Lorem Ipsum Dolor Sit Amet"));
        }

        #endregion

        #region Methods: Substring, Trim, Replace

        [Test]
        public void SubPhrase_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            // Act
            var valueToTest = String.Empty.SubPhrase(0,10,',');

            // Assert
            Assert.That(valueToTest, Is.Empty);
        }

        [Test]
        public void SubPhrase_Null_ReturnsNull()
        {
            // Arrange
            string value = null;
            // Act
            var valueToTest = value.SubPhrase(0, 10, ',');
            // Assert
            Assert.IsNull(valueToTest);
        }

        [Test]
        public void SubPhrase_StartIndexZero_ReturnsSubPhrase()
        {
            // Arrange
            // Act
            var valueToTest = "Lorem ipsum dolor sit amet".SubPhrase(0, 6);

            // Assert
            Assert.That(valueToTest,Is.EqualTo("Lorem"));
        }

        
        [TestCase("Lorem ipsum dolor sit amet", 6, 3, ExpectedResult = "ips")]
        [TestCase("Lorem ipsum dolor sit amet", 7, 4, ExpectedResult = "psum")]
        public string SubPhrase_ValidStartIndexInRange_ReturnsSubPhrase(string value,int startIndex,int length)
        {
            return value.SubPhrase(startIndex, length);
        }

        [Test]
        public void SubPhrase_DilimiterInRange_ReturnsSubPhrase()
        {
            // Arrange
            // Act
            var valueToTest = "Lorem, ipsum dolor sit amet".SubPhrase(0, 10, ',');

            // Assert
            Assert.That(valueToTest, Is.EqualTo("Lorem"));
        }

        [Test]
        public void SubPhrase_DilimiterOutOfSelectionRange_ReturnsSubPhrase()
        {
            // Arrange
            // Act
            var valueToTest = "Lorem, ipsum dolor sit amet".SubPhrase(0, 4, ',');

            // Assert
            Assert.That(valueToTest, Is.EqualTo("Lore"));
        }

        [Test]
        public void SubPhrase_StartIndexLowerThanRange_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Lorem, ipsum dolor sit amet".SubPhrase(-1, 10));
        }
        
        [Test]
        public void SubPhrase_StartIndexGreaterThanRange_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Lorem, ipsum dolor sit amet".SubPhrase(-500, 10));
        }

        [Test]
        public void SubPhrase_StartIndexLastCharAndLengthIsGreaterThanOne_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Lorem ipsum dolor sit amet".SubPhrase(25, 2));
        }

        [Test]
        public void SubPhrase_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Lorem Ipsum".SubPhrase(0, -5));
        }

        #endregion

    }
}
