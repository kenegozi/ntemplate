using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NTemplate.Compiler;

namespace NTemplate
{
	public partial class NTemplateEngine
	{
		public class Options
		{
			public CompilerOptions CompilerOptions;
		}
		public class CompilerOptions
		{
			public bool AutoRecompilation;
		}
	}
	public partial class NTemplateEngine
	{
		private Options options;
		private ITemplateSourceLoader templateSourceLoader;
		List<ICompilationContext> _compilationContexts = new List<ICompilationContext>();
		private bool _needsRecompile;
		private readonly List<string> _assemblies = new List<string>();

		public void AddAssembly(string assembly)
		{
			_assemblies.Add(assembly);
		}

		public void Initialize()
		{
			//InitializeOptionsIfNeeded();

			//InitializeCompilationContexts();

			//LoadCompiledTemplates();

			// SetAutoRecompilationListener()
		}

		void SetAutoRecompilationListener()
		{
			if (options.CompilerOptions.AutoRecompilation)
			{
				// invalidate compiled views cache on any change to the view sources
				templateSourceLoader.ViewChanged += (sender, e) =>
				{
					if (AbstractCompiler.TemplateExtensions.Any(extension => e.Name.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)))
					{
						_needsRecompile = true;
						return;
					}
				};
			}
		}

		private readonly AbstractCompiler _compiler = new AspViewCompiler();

		private Dictionary<string, Type> _compiledTemplates = new Dictionary<string, Type>();

		public void Compile(string templateContent, string templateName)
		{
			var compiled = _compiler.Compile(templateName, templateContent);
			_compiledTemplates[templateName] = compiled;
		}

		public Template GetTemplate(string templateName, IDictionary parameters, TextWriter writer)
		{
			var templateType = _compiledTemplates[templateName];
			var instance = (Template)FormatterServices.GetUninitializedObject(templateType);
			instance.Initialize(parameters, this, writer);
			return instance;
		}

		public string Render(string templateName)
		{
			return Render(templateName, null);
		}
		public string Render(string templateName, IDictionary parameters)
		{
			var buffer = new StringBuilder();
			using (var writer = new StringWriter(buffer))
			{
				var instance = GetTemplate(templateName, parameters, writer);
				instance.Render();
			}
			return buffer.ToString();
		}
	}
}

