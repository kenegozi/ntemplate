using System;

namespace NTemplate.Compiler.Directives
{
	public class PageDirective
	{
		public PageDirective(string content)
		{
			throw new Exception("page directive is not yet supported");
			//		Content = content;

		}

		public string BaseClass { get; private set; }
	}
}