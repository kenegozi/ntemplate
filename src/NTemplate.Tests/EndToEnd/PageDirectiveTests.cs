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
	}
}