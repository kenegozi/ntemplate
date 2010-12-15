using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class PageDirectiveTests : EndToEndTests
	{
		[Test]
		public void PageDirectiveDoesNotFailCompilation()
		{
			var template = @"<%@Page Language=""C#"" %>
123";
			var expected = @"123";
			AssertRendered(template, expected);
		}

		[Test]
		public void MissingPageDirectiveDoesNotFailCompilation()
		{
			var template = @"123";
			var expected = @"123";
			AssertRendered(template, expected);
		}

		[Test]
		public void BaseClassIsConsidered()
		{
			var template = @"<%@Page Language=""C#"" Inherits=""NTemplate.Tests.EndToEnd.CustomBaseClass""%>
<%=Foo()%>";
			var expected = @"bar";
			AssertRendered(template, expected);
		}
	}

	public abstract class CustomBaseClass : Template
	{
		protected string Foo()
		{
			return "bar";
		}
	}
}