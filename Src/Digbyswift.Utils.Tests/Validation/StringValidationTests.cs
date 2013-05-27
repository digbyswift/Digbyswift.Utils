using System;
using Digbyswift.Utils.Extensions.Validation;
using NUnit.Framework;

namespace Digbyswift.Utils.Tests.Validation
{
	[TestFixture]
	public class StringValidationTests
	{

		#region Methods: IsCommaSeparated

		[Test]
		public void IsCommaDelimited_Null_Throws()
		{
			Assert.That(() => StringValidation.IsCommaSeparated(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[TestCase("")]
		[TestCase("-")]
		[TestCase("-a")]
		[TestCase("-0")]
		[TestCase("abc!")]
		[TestCase("a\"i().;'#")]
		[TestCase(",")]
		[TestCase(",a")]
		[TestCase("a,")]
		public void IsCommaDelimited_NonCommaDelimitedText_ReturnsFalse(string value)
		{
			Assert.That(value.IsCommaSeparated(), Is.False);
		}

		[TestCase("a")]
		[TestCase("abc")]
		[TestCase("1")]
		[TestCase("123")]
		[TestCase("1a2bc3def")]
		public void IsCommaDelimited_SingleAlphanumericValue_ReturnsTrue(string value)
		{
			Assert.That(value.IsCommaSeparated(), Is.True);
		}

		[TestCase("a,a")]
		[TestCase("a,bc,d")]
		[TestCase("1,1")]
		[TestCase("1,23")]
		[TestCase("1a,2bc,3,de,f")]
		public void IsCommaDelimited_CommaDelimitedValues_ReturnsTrue(string value)
		{
			Assert.That(value.IsCommaSeparated(), Is.True);
		}

		#endregion

		#region Methods: IsEmail

		[Test]
		public void IsEmail_NullValue_Throws()
		{
			Assert.That(() => StringValidation.IsEmail(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[TestCase("")]
		[TestCase("a")]
		[TestCase("1")]
		[TestCase("!")]
		[TestCase("test@")]
		[TestCase("@test.com")]
		[TestCase("a@a.c")]
		[TestCase("ab@ab.c")]
		[TestCase("ab@abc.d")]
		[TestCase("ab@.co")]
		[TestCase("ab@.co")]
		[TestCase("ab@c.d.e")]
		[TestCase("a@b@c.d.e")]
		[TestCase("_asd@.co.uk")]
		public void IsEmail_NonEmailValues_ReturnsFalse(string value)
		{
			Assert.That(value.IsEmail(), Is.False);
		}

		[TestCase("test@test.co.uk")]
		[TestCase("test.test.test@test.test.co.uk")]
		[TestCase("123.123.123@123.123")]
		[TestCase("asd_@asd.co.uk")]
		[TestCase("asd_asd@asd.co.uk")]
		[TestCase("._asd@asd.co.uk")]
		[TestCase("a._asd@asd.co.uk")]
		public void IsEmail_ValidEmailValues_ReturnsTrue(string value)
		{
			Assert.That(value.IsEmail(), Is.True);
		}

		#endregion

	}
}