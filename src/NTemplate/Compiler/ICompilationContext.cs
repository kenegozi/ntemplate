namespace NTemplate.Compiler
{
	public interface ICompilationContext
	{
		string BinFolder { get; }
		string TemporarySourceFilesDirectory { get; }
	}
}