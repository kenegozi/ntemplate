using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class PartialTemplateTests : EndToEndTests
	{
		[Test]
		public void PartialWithoutParameters()
		{
			engine.Compile("inner content", "/inner");

			var template = "outer <% RenderPartial(\"/inner\"); %> outer again";
			var expected = "outer inner content outer again";

			AssertRendered(template, expected);
		}

		[Test]
		public void PartialSeeItsParameters()
		{
			engine.Compile("in inner, param1=<%=model[\"param1\"]%>", "/inner");
			new System.Collections.Hashtable {{"d", 1}};
			var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase) { { "param1", 1 } };
			
			var template = @"
in outer
<% RenderPartial(""/inner"", new System.Collections.Hashtable{ { ""param1"", 1 } }); %>
";
			var expected = @"
in outer
in inner, param1=1
";

			AssertRendered(template, expected, parameters);
		}

		[Test]
		public void PartialSeeOuterParameters()
		{
			engine.Compile("in inner, param1=<%=model[\"param1\"]%>", "/inner");

			var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase) { { "param1", 1 } };

			var template = @"
in outer, param1=<%=model[""param1""]%>
<% RenderPartial(""/inner""); %>
";
			var expected = @"
in outer, param1=1
in inner, param1=1
";

			AssertRendered(template, expected, parameters);
		}

		[Test]
		public void PartialParametersHideParentParameters()
		{
			engine.Compile("in inner, param1=<%=model[\"param1\"]%>", "/inner");

			var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase) { { "param1", 1 } };

			var template = @"
in outer, param1=<%=model[""param1""]%>
<% RenderPartial(""/inner"", new System.Collections.Hashtable{ { ""param1"", 2 } }); %>
in outer, param1=<%=model[""param1""]%>
";
			var expected = @"
in outer, param1=1
in inner, param1=2
in outer, param1=1
";

			AssertRendered(template, expected, parameters);
		}
	}
}