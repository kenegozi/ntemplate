using System.Collections;
using System.IO;
using NTemplate.Internal;

namespace NTemplate
{
	public abstract class Template
	{
		private NTemplateEngine _engine;

		protected abstract string Name { get; }

		protected string GetViewDirectory()
		{
			return Name.Substring(0, Name.LastIndexOf('/') + 1);
		}

		private IDictionary _properties;
		protected IDictionary model { get { return _properties; } }
		public void Initialize(IDictionary parentParameters, NTemplateEngine engine, TextWriter writer)
		{
			_engine = engine;
			_properties = new TemplateParameters(parentParameters);
			Writer = writer;
		}
		public abstract void Render();
		public TextWriter Writer { get; private set; }
		protected void Write(object content)
		{
			if (content == null) return;

			Writer.Write(content);
		}

		protected void RenderPartial(string name)
		{
			RenderPartial(name, null);
		}
		protected void RenderPartial(string name, IDictionary parameters)
		{
			Template partial = _engine.GetTemplate(GetRootedTemplateName(name), parameters, Writer);
			//subView.Initialize(viewEngine, writer, Context, Controller, controllerContext, Properties);

			// bring parameters to the subview
			if (parameters != null)
				foreach (DictionaryEntry item in (IDictionary)parameters)
					if (item.Value != null)
						partial._properties[item.Key] = item.Value;

			//PushCurrentView();
			partial.Render();
			//PopCurrentView();

			// allow CaptureFor generated content to bubble back up 
			//GatherBubblingPropertiesFrom(subView);

		}

		private string GetRootedTemplateName(string name)
		{
			if (name[0] == '/')
				return name;

			return GetViewDirectory() + name;
		}
	}
}