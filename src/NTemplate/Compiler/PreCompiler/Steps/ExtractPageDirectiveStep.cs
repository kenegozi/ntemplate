using System;
using System.Linq;
using NTemplate.Compiler.Directives;

namespace NTemplate.Compiler.PreCompiler.Steps
{
	public class ExtractPageDirectiveStep : IPreCompilationStep
	{
		public void Execute(TemplateCompilationInfo templateCompilationInfo)
		{
			var pageDirectiveLine = templateCompilationInfo.OriginalLines
				.Where(l => l.Processed == false)
				.Where(l => l.Content.StartsWith("<%@page", StringComparison.InvariantCultureIgnoreCase))
				.FirstOrDefault();

			if (pageDirectiveLine == null) return;

			pageDirectiveLine.Processed = true;
			templateCompilationInfo.PageDirective = new PageDirective(pageDirectiveLine.Content);
		}
	}
}