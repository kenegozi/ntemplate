using System;
using System.Linq;
using NTemplate.Compiler;
using NTemplate.Compiler.PreCompiler.Steps;
using NUnit.Framework;

namespace NTemplate.Tests.Compiler.PreCompilationSteps
{
	public abstract class StepTests
	{
		protected static TemplateCompilationInfo.InputLine[] ToInputLines(params string[] lines)
		{
			var inputLines = lines.Select(l => new TemplateCompilationInfo.InputLine { Content = l }).ToArray();
			for (var lineNumber = 1; lineNumber <= lines.Length; ++lineNumber)
			{
				inputLines[lineNumber - 1].LineNumber = lineNumber;
			}
			return inputLines;
		}
	}

	[TestFixture]
	public class ExtractPageDirectiveStepTests : StepTests
	{
		[Test]
		public void DummyPageDirective_FoundAndProcessed()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(@"<%@Page Language=""C#""%>");

			var step = new ExtractPageDirectiveStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.True);
			Assert.That(compilationInfo.PageDirective, Is.Not.Null);
			Assert.That(compilationInfo.PageDirective.LineNo, Is.EqualTo(1));
			Assert.That(compilationInfo.PageDirective.BaseClass, Is.Null);
		}

		[Test]
		public void NoPageDirective_NothingWrong()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(@"");

			var step = new ExtractPageDirectiveStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.False);
			Assert.That(compilationInfo.PageDirective, Is.Null);
		}

		[Test]
		public void TwoPageDirectives_Throws()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(
				@"<%@Page Language=""C#""%>",
				@"<%@Page Language=""C#""%>");

			var step = new ExtractPageDirectiveStep();

			Assert.Throws<Exception>(() =>
				step.Execute(compilationInfo)
			);
		}
		[Test]
		public void PageDirectivewithInheritsAttribute_BaseClassIsSet()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(@"<%@Page Language=""C#"" Inherits=""Foo""%>");

			var step = new ExtractPageDirectiveStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.True);
			Assert.That(compilationInfo.PageDirective, Is.Not.Null);
			Assert.That(compilationInfo.PageDirective.LineNo, Is.EqualTo(1));
			Assert.That(compilationInfo.PageDirective.BaseClass, Is.EqualTo("Foo"));
		}
	}

	public class ImportDirectivesStepTests : StepTests
	{
		[Test]
		public void NoDirective_NothingHappened()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(@"bla");

			var step = new ImportDirectivesStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.UsingDirectives.Count, Is.EqualTo(0));
			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.False);
		}

		public void SingleDirective_Processed()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(@"<%@ Import Namespace=""Larry.Bird.Is.The.Best.Forward.In.History""%>");

			var step = new ImportDirectivesStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.UsingDirectives.Count, Is.EqualTo(1));
			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.True);
			Assert.That(compilationInfo.UsingDirectives[0].OriginalLineNo, Is.EqualTo(1));
			Assert.That(compilationInfo.UsingDirectives[0].Using, Is.EqualTo("Larry.Bird.Is.The.Best.Forward.In.History"));
		}
		public void MultipleDirectives_Processed()
		{
			var compilationInfo = new TemplateCompilationInfo("name");
			compilationInfo.OriginalLines = ToInputLines(
				@"<%@ Import Namespace=""Larry.Bird.Is.The.Best.Forward.In.History""%>",
				@"<%@ Import Namespace=""Tom.Chambres.Is.Not""%>");

			var step = new ImportDirectivesStep();

			step.Execute(compilationInfo);

			Assert.That(compilationInfo.UsingDirectives.Count, Is.EqualTo(2));
			Assert.That(compilationInfo.OriginalLines[0].Processed, Is.True);
			Assert.That(compilationInfo.UsingDirectives[0].OriginalLineNo, Is.EqualTo(1));
			Assert.That(compilationInfo.UsingDirectives[0].Using, Is.EqualTo("Larry.Bird.Is.The.Best.Forward.In.History"));
			Assert.That(compilationInfo.OriginalLines[1].Processed, Is.True);
			Assert.That(compilationInfo.UsingDirectives[1].OriginalLineNo, Is.EqualTo(2));
			Assert.That(compilationInfo.UsingDirectives[1].Using, Is.EqualTo("Tom.Chambres.Is.Not"));
		}
	}
}