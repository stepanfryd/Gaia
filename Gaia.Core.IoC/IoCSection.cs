using System.Xml.Serialization;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// IoC configuration section under gaia/settings
	/// </summary>
	[XmlRoot("ioc")]
	public class IoCSection
	{
		/// <summary>
		/// Container provider type name (eg. "Gaia.Core.IoC.Unity.Container, Gaia.Core.IoC.Unity")
		/// </summary>
		[XmlAttribute("containerProvider")]
		public string ContainerProviderTypeName { get; set; }
	}
}