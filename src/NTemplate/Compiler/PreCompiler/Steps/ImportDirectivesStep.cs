using System.Linq;
using System.Text.RegularExpressions;

namespace NTemplate.Compiler.PreCompiler.Steps
{
	public class ImportDirectivesStep : IPreCompilationStep
	{
		static readonly Regex ImportDirective = new Regex(
			@"^\s*<%@\s*Import\s+Namespace\s*=\s*""(?<namespace>[\w.]+)""\s*%>\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public void Execute(TemplateCompilationInfo templateCompilationInfo)
		{
			var importDirectives = from l in templateCompilationInfo.OriginalLines
			                       where l.Processed == false
			                       let match = ImportDirective.Match(l.Content)
			                       where match.Success
			                       select new { Line = l, Match=match};

			foreach (var importDirective in importDirectives)
			{
				importDirective.Line.Processed = true;
				templateCompilationInfo.UsingDirectives.Add(new TemplateCompilationInfo.UsingDirective
				                                            	{
				                                            		OriginalLineNo=importDirective.Line.LineNumber,
				                                            		Using = importDirective.Match.Groups["namespace"].Value
				                                            	});
			}
		}
	}
}