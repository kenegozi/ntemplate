using System;

namespace NTemplate.Compiler.PreCompiler.Steps
{
	public class ProcessBlocksStep : IPreCompilationStep
	{
		public void Execute(TemplateCompilationInfo templateCompilationInfo)
		{
			var lineNo = 0;
			var inCodeBlock = false;
			foreach (var inputLine in templateCompilationInfo.OriginalLines)
			{
				++lineNo;
				if (inputLine.Processed) continue;
				var lastCharIx = -1;
				while (lastCharIx < inputLine.Content.Length)
				{
					if (inCodeBlock == false)
					{
						var nextCodeBlock = inputLine.Content.IndexOf("<%", lastCharIx + 1);
						if (nextCodeBlock <= lastCharIx)
						{
							var block = new TemplateCompilationInfo.MarkupBlock
							            	{
							            		Content = inputLine.Content.Substring(lastCharIx + 1),
							            		OriginalLineNo = lineNo
							            	};
							if (lineNo < templateCompilationInfo.OriginalLines.Length)
								block.Content += Environment.NewLine;
							templateCompilationInfo.Blocks.Add(block);
							lastCharIx = int.MaxValue;
							continue;
						}
						var markupBlockContent = inputLine.Content.Substring(lastCharIx + 1, nextCodeBlock - lastCharIx - 1);
						templateCompilationInfo.Blocks.Add(new TemplateCompilationInfo.MarkupBlock
						                                   	{
						                                   		Content = markupBlockContent,
						                                   		OriginalLineNo = lineNo
						                                   	});
						lastCharIx = nextCodeBlock - 1 + 2;
						inCodeBlock = true;
					}
					else
					{
						var endCodeBlock = inputLine.Content.IndexOf("%>", lastCharIx + 1);
						if (endCodeBlock <= lastCharIx)
						{
							templateCompilationInfo.Blocks.Add(new TemplateCompilationInfo.CodeBlock
							                                   	{
							                                   		Content = inputLine.Content.Substring(lastCharIx + 1),
							                                   		OriginalLineNo = lineNo
							                                   	});
							lastCharIx = int.MaxValue;
							continue;
						}
						var codeBlockContent = inputLine.Content.Substring(lastCharIx + 1, endCodeBlock - lastCharIx - 1);
						templateCompilationInfo.Blocks.Add(new TemplateCompilationInfo.CodeBlock
						                                   	{
						                                   		Content = codeBlockContent,
						                                   		OriginalLineNo = lineNo
						                                   	});
						lastCharIx = endCodeBlock - 1 + 2;
						inCodeBlock = false;
					}
				}
			}
		}
	}
}