using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class LanguageFeaturesTests : EndToEndTests
	{
		[Test]
		public void UsingAnonymousObject()
		{
			var template = @"
<% 	var temp = new{ a=1,foo=""bar"" }; %>
<%=temp.a%>
<%=temp.foo%>
";
			var expected = @"

1
bar
";
			AssertRendered(template, expected);
		}

		[Test]
		public void UsingCollectionInitializer()
		{
			var template = @"
<%
	var temp = new[] { 1,2,3 }; 
	foreach (var x in temp) { Write(x); }
%>";
			var expected = @"
123";
			AssertRendered(template, expected);
		}
	}
}