using System;

namespace NTemplate.Compiler.Directives
{
	public class PageDirective
	{
		public int LineNo;
		public PageDirective(TemplateCompilationInfo.InputLine content, string baseClass)
		{
			LineNo = content.LineNumber;
			BaseClass = baseClass;
		}

		public string BaseClass { get; private set; }
	}

	public class ImportDirective
	{
		public int LineNo;
		public string Namespace;
	}
}