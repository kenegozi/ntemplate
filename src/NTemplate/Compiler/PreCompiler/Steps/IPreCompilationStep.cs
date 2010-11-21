namespace NTemplate.Compiler.PreCompiler.Steps
{
	public interface IPreCompilationStep
	{
		void Execute(TemplateCompilationInfo templateCompilationInfo);
	}
}