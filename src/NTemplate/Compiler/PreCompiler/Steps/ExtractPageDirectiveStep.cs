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
			var pageDirectives = (from l in templateCompilationInfo.OriginalLines
									  where l.Processed == false
									  let match = PageDirective.Match(l.Content)
									  where match.Success
									  select new { Line = l, Match = match })
								.ToArray();

			if (pageDirectives.Length==0)
				return;

			if (pageDirectives.Length > 1)
				throw new Exception("there can only be up to one Page directive in a template, sorry.");

			var pageDirective = pageDirectives[0];

			pageDirective.Line.Processed = true;
			string baseClass = null;
			if (pageDirective.Match.Groups["base"].Success)
				baseClass = pageDirective.Match.Groups["base"].Value;
			templateCompilationInfo.PageDirective = new PageDirective(pageDirective.Line, baseClass);
		}
	}
}