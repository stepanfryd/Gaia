using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Configuration
{
	/// <summary>
	/// Common application settings usually in web.config under gaia/settings
	/// </summary>
	[XmlRoot("settings")]
	public class Settings : ISettings
	{
		/// <summary>
		/// Application name
		/// </summary>
		[XmlElement("applicationName")]
		public string ApplicationName { get; set; }

		/// <summary>
		/// Modules location (usualy ~/Areas)
		/// </summary>
		[XmlElement("modulesRoot")]
		public string ModulesRoot { get; set; }

		/// <summary>
		/// Mask used for module includes defined in configuration file
		/// </summary>
		[XmlElement("moduleIncludeMask")]
		public string ModuleIncludeMask { get; set; }

		/// <summary>
		/// Name of JSON configuration file (module.json)
		/// </summary>
		[XmlElement("moduleConfigFile")]
		public string ModuleConfigFile { get; set; }

		/// <summary>
		/// Default mask for script module bundles
		/// </summary>
		[XmlElement("moduleScriptBundleMask")]
		public string ModuleScriptBundleMask { get; set; }

		/// <summary>
		/// Default mask for style module bundles
		/// </summary>
		[XmlElement("moduleStylesBundleMask")]
		public string ModuleStylesBundleMask { get; set; }

		/// <summary>
		/// Path to common application bundles configuration JSON file
		/// </summary>
		[XmlElement("bundlesConfig")]
		public string BundlesConfig { get; set; }
	}
}