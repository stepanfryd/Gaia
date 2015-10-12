using System;

namespace Gaia.Portal.Framework.Configuration.Modules
{
	[AttributeUsage(AttributeTargets.Class)]
	public class WebModuleConfigurationAttribute : Attribute
	{
		public WebModuleConfigurationAttribute(string moduleName)
		{
			ModuleName = moduleName;
		}

		public string ModuleName { get; private set; }
	}
}