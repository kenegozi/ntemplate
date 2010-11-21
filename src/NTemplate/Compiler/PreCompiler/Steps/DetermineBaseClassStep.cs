namespace NTemplate.Compiler.PreCompiler.Steps
{
	public class DetermineBaseClassStep : IPreCompilationStep
	{
		public void Execute(TemplateCompilationInfo templateCompilationInfo)
		{
			if (templateCompilationInfo.PageDirective != null)
				if (templateCompilationInfo.PageDirective.BaseClass != null)
					templateCompilationInfo.CustomBaseClass = templateCompilationInfo.PageDirective.BaseClass;
		}
	}
}