using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;
using NTemplate.Compiler.PreCompiler.Steps;
using NTemplate;

namespace NTemplate.Compiler
{
	public class AspViewCompiler : AbstractCompiler
	{
		NTemplateEngine _engine;
		public AspViewCompiler(NTemplateEngine engine)
		{
			_engine = engine;
		}
		private static readonly string DefaultBaseClass = "NTemplate.Template";
		public override Type Compile(string name, string content)
		{
			var templateInfo = new TemplateCompilationInfo(name);
			var contentLines = content.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

			templateInfo.OriginalLines = new TemplateCompilationInfo.InputLine[contentLines.Length];
			for (var lineNumber = 1; lineNumber <= contentLines.Length; ++lineNumber)
			{
				templateInfo.OriginalLines[lineNumber - 1] = new TemplateCompilationInfo.InputLine
				{
					Content = contentLines[lineNumber - 1], 
					LineNumber = lineNumber
				};
			}

			string structure =
				@"#line hidden
{4}
#line hidden
public class {0} : 
#line {5}
{2} 
#line hidden
{{
protected override string Name {{ get {{ return ""{3}""; }} }}
public override void Render() {{
{1}
}}
}}
";
			new ExtractPageDirectiveStep().Execute(templateInfo);
			new DetermineBaseClassStep().Execute(templateInfo);
			new ImportDirectivesStep().Execute(templateInfo);
			var usingBlock = string.Empty;
			if (templateInfo.UsingDirectives.Count>0)
				usingBlock = templateInfo.UsingDirectives
					.Select(d => "#line " + d.OriginalLineNo + Environment.NewLine + "using " + d.Using + ";")
					.Aggregate((u1, u2) => u1 + Environment.NewLine + u2);
			var baseClass = templateInfo.CustomBaseClass ?? DefaultBaseClass;
			var baseClassLine = templateInfo.CustomBaseClass != null ? templateInfo.PageDirective.LineNo.ToString() : "hidden";
			var className = GetClassName(templateInfo.Name);
			new ProcessBlocksStep().Execute(templateInfo);
			var renderBody = templateInfo.Blocks.Select(b => b.GetCode()).Aggregate((b1, b2) => b1 + Environment.NewLine + b2);
			var generatedClass = string.Format(structure, className, renderBody, baseClass, name, usingBlock, baseClassLine);

			var codeProvider = GetCodeProvider();

			var parameters = new CompilerParameters();
			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Core.dll");
			foreach (var assembly in _engine.ReferencesAssemblies)
				parameters.ReferencedAssemblies.Add(assembly);
			var result = codeProvider.CompileAssemblyFromSource(parameters, generatedClass);
			if (result.Errors.Count > 0)
			{
				var messages = result.Errors.Cast<CompilerError>().Select(err => err.ToString());
				var message = messages.Aggregate("Could not compile "+name,(a, b) => a + Environment.NewLine + b);
				throw new Exception(message);
			}
			return result.CompiledAssembly.GetType(className);
		}




		private CodeDomProvider GetCodeProvider()
		{
			return new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v3.5" } });
		}
	}
}