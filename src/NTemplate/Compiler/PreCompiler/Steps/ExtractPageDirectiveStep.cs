using System;
using System.Linq;
using System.Text.RegularExpressions;
using NTemplate.Compiler.Directives;

namespace NTemplate.Compiler.PreCompiler.Steps
{
	public class ExtractPageDirectiveStep : IPreCompilationStep
	{
		static readonly string BaseViewTypeExpression = @"(?<base>[\w.]+)(?:(?:<(?<view>[\w.<>]+)>)|(?:`1\[(?<view>[\w.`\[\]]+)\]))";
		static readonly Regex PageDirective = new Regex(
@"^\s*<%@\s*Page\s+Language\s*=\s*""c#""(?:\s+Inherits\s*=\s*""" + BaseViewTypeExpression + @"?\s*"")?.*%>\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public void Execute(TemplateCompilationInfo templateCompilationInfo)
		{
			var pageDirectiveLine = templateCompilationInfo.OriginalLines
				.Where(l => l.Processed == false)
				.Where(l => PageDirective.IsMatch(l.Content))
				.FirstOrDefault();

			if (pageDirectiveLine == null) return;

			pageDirectiveLine.Processed = true;
			templateCompilationInfo.PageDirective = new PageDirective(pageDirectiveLine.Content);
		}
	}
}