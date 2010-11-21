﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;
using NTemplate.Compiler.PreCompiler.Steps;

namespace NTemplate.Compiler
{
	public class AspViewCompiler : AbstractCompiler
	{
		private static readonly string DefaultBaseClass = "NTemplate.Template";
		public override Type Compile(string name, string content)
		{
			var templateInfo = new TemplateCompilationInfo(name);
			templateInfo.OriginalLines = content
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Select(l => new TemplateCompilationInfo.InputLine { Content = l })
				.ToArray();

			string structure =
				@"
public class {0} : {2} {{
protected override string Name {{ get {{ return ""{3}""; }} }}
public override void Render() {{
{1}
}}
}}
";
			var baseClass = templateInfo.CustomBaseClass ?? DefaultBaseClass;
			var className = GetClassName(templateInfo.Name);
			new ProcessBlocksStep().Execute(templateInfo);
			var renderBody = templateInfo.Blocks.Select(b => b.GetCode()).Aggregate((b1, b2) => b1 + Environment.NewLine + b2);
			var generatedClass = string.Format(structure, className, renderBody, baseClass, name);

			var codeProvider = GetCodeProvider();

			var parameters = new CompilerParameters();
			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;
			parameters.ReferencedAssemblies.Add("NTemplate.dll");
			parameters.ReferencedAssemblies.Add("NTemplate.Tests.dll");
			var result = codeProvider.CompileAssemblyFromSource(parameters, generatedClass);
			return result.CompiledAssembly.GetType(className);
		}




		private CodeDomProvider GetCodeProvider()
		{
			return new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v3.5" } });
		}
	}
}