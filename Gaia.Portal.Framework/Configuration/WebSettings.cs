using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Configuration
{
	[Serializable]
	[XmlRoot("web")]
	public class WebSettings : IWebSettings
	{
		#region  Public members

		[XmlAttribute("apiDependencyResolver")]
		public string ApiDependencyResolver { get; set; }

		[XmlAttribute("mvcDependencyResolver")]
		public string MvcDependencyResolver { get; set; }

		[XmlArray("filterProviders")]
		[XmlArrayItem("provider")]
		public List<string> FilterProviders { get; set; }

		#endregion
	}
}