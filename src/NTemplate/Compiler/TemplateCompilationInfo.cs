using System;
using System.Collections.Generic;
using NTemplate.Compiler.Directives;

namespace NTemplate.Compiler
{
	public class TemplateCompilationInfo
	{
		public struct UsingDirective
		{
			public int OriginalLineNo;
			public string Using;
		}
		public abstract class Block
		{
			public int OriginalLineNo;
			public string Content;
			public abstract string GetCode();
		}
		public class MarkupBlock : Block
		{
			public override string GetCode()
			{
				return "#line " + OriginalLineNo + Environment.NewLine + "Write(@\"" + Content.Replace("\"", "\"\"") + "\");";
			}
		}
		public class CodeBlock : Block
		{
			public override string GetCode()
			{
				var content = Content;
				if (Content[0] == '=')
					content = "Write(" + content.Substring(1) + ");";
				return "#line " + OriginalLineNo + Environment.NewLine + content;
			}
		}
		public class InputLine
		{
			public string Content;
			public bool Processed;
		}

		public string Name;
		public string ViewDirectory = string.Empty;
		public InputLine[] OriginalLines;

		public List<UsingDirective> UsingDirectives = new List<UsingDirective>();
		public PageDirective PageDirective;
		public List<Block> Blocks = new List<Block>();
		public string CustomBaseClass;

		public TemplateCompilationInfo(string name)
		{
			Name = name;
			var lastSlashIndex = name.LastIndexOf('/');
			if (lastSlashIndex > -1)
			{
				ViewDirectory = name.Substring(0, lastSlashIndex);
			}
		}
	}
}