using System;
using System.IO;

namespace NTemplate.Compiler
{
	public interface ITemplateSourceLoader
	{
		event EventHandler<FileSystemEventArgs> ViewChanged;
	}
}