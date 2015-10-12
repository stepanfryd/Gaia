using System.Collections.Generic;
using System.Web.Optimization;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Modules
{
	public class DefaultWebModule : IWebModule
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("area")]
		public string AreaName { get; set; }

		[JsonProperty("iocContainerConfig")]
		public string IocContainerConfig { get; set; }

		[JsonIgnore]
		public string IocContainerFullPath { get; set; }

		[JsonProperty("scripts")]
		public List<string> Scripts { get; set; }

		[JsonIgnore]
		public BundleCollection Bundles { get; set; }

		public DefaultWebModule()
		{
			Bundles = new BundleCollection();
		}
	}
}