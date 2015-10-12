namespace Gaia.Portal.Framework.Configuration
{
	public interface ISettings
	{
		string ApplicationName { get; set; }
		string ModulesRoot { get; set; }
		string ModuleIncludeMask { get; set; }
		string ModuleConfigFile { get; set; }
		string ModuleScriptBundleMask { get; set; }
		string ModuleStylesBundleMask { get; set; }
		string BundlesConfig { get; set; }
	}
}