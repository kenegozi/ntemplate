
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NTemplate.Compiler
{
	public abstract class AbstractCompiler
	{
		public static string[] TemplateExtensions;
		public abstract Type Compile(string name, string content);

		static readonly Regex InvalidClassNameCharacters = new Regex("[^a-zA-Z0-9\\-*#=/\\\\_.]", RegexOptions.Compiled);
		public static string GetClassName(string fileName)
		{
			fileName = fileName.ToLowerInvariant();
			if (Path.HasExtension(fileName))
			{
				var lastDotIndex = fileName.LastIndexOf('.');
				fileName = fileName.Substring(0, lastDotIndex);
			}

			fileName = InvalidClassNameCharacters.Replace(fileName, "_");

			var className = fileName
				.Replace('\\', '_')
				.Replace('/', '_')
				.Replace("-", "_dash_")
				.Replace("=", "_equals_")
				.Replace("*", "_star_")
				.Replace("#", "_sharp_")
				.Replace("=", "_equals_")
				.TrimStart('_')
				.Replace('.', '_');

			return className;
		}
	}
}
