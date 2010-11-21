using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class MarkupAndCodeBlocksTests : EndToEndTests
	{
		[Test]
		public void MarkupOnly()
		{
			AssertRendered("Hello", "Hello");
		}
		[Test]
		public void CodeBlock()
		{
			AssertRendered("<%Write(1+1);%>", "2");
		}
		[Test]
		public void OutputCodeBlock()
		{
			AssertRendered("<%=1+1", "2");
		}
		[Test]
		public void MarkupAndThenCodeOnTheSameLine()
		{
			AssertRendered("Hello <%Write(\"2\");%>", "Hello 2");
		}
		[Test]
		public void MarkupCodeMarkup()
		{
			AssertRendered("Hello <%Write(\"2\");%>1", "Hello 21");
		}
		[Test]
		public void CodeMarkupCode()
		{
			AssertRendered("<%Write(\"Hello\");%> to <%Write(\"2\");%>", "Hello to 2");
		}

		[Test]
		public void MultilineMarkup()
		{
			AssertRendered("hello\r\nworld", "hello\r\nworld");
		}

		[Test]
		public void MixedLineEndsWithMarkupThenAnotherLine_NewLineIsPreserved()
		{
			AssertRendered("hello<%Write(1);%>2\r\nworld", "hello12\r\nworld");
		}
		[Test]
		public void MixedLineEndsWithCodeThenAnotherLine_NewLineIsPreserved()
		{
			AssertRendered("hello<%Write(1);%>\r\nworld", "hello1\r\nworld");
		}
		[Test]
		public void MultilineCode_NoNewLines()
		{
			AssertRendered("<%Write(1);\r\nWrite(2);%>", "12");
		}

		[Test]
		public void Complex_WithMultilines_Etc()
		{
			var template = @"<% var x = new[] {1,2,3};
foreach (var i in x) { %>
  <div><%Write(i);%></div>
<%}%>";
			var expected = @"
  <div>1</div>

  <div>2</div>

  <div>3</div>
";
			AssertRendered(template, expected);
		}
	}
}
