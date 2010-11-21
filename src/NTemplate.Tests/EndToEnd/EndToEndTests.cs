using System.Collections;
using NUnit.Framework;

namespace NTemplate.Tests.EndToEnd
{
	public abstract class EndToEndTests
	{
		protected NTemplateEngine engine;

		[SetUp]
		public void SetUp()
		{
			engine = new NTemplateEngine();

			engine.AddAssembly("NTemplate.Tests.dll");
		}

		protected void AssertRendered(string template, string expected)
		{
			AssertRendered(template, expected, null);
		}

		protected void AssertRendered(string template, string expected, IDictionary parameters)
		{
			var templateName = "/myTemplate";
			engine.Compile(template, templateName);
			var rendered = engine.Render(templateName, parameters);
			Assert.That(rendered, Is.EqualTo(expected));
		}
		
	}
}