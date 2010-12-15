using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class ImportDirectivesTests : EndToEndTests
	{
		[Test]
		public void ImportDirectives_GenerateTheRequiredUsing()
		{
			var template = @"<%@Page Language=""C#"" %>
<%@Import namespace=""NTemplate.Tests.EndToEnd"" %>
<%=Foo.Bar()%>";
			var expected = @"bar";
			AssertRendered(template, expected);
		}
	}
	public class Foo
	{
		public static string Bar()
		{
			return "bar";
		}
	}
}