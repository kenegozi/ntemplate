using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	[TestFixture]
	public class ParametersTests : EndToEndTests
	{
		[Test]
		public void SimpleStringParameter()
		{
			var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase) {{"name", "Ken"}};

			AssertRendered("hello <%=model[\"name\"]%>", "hello Ken", parameters);
		}

		[Test]
		public void SimpleIntegerParameter()
		{
			var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase) { { "age", 32 } };

			AssertRendered(
				"you are <%=model[\"age\"]%> years old, and will be <%=(int)model[\"age\"] + 1%> next year",
				"you are 32 years old, and will be 33 next year", 
				parameters);
		}
	}
}