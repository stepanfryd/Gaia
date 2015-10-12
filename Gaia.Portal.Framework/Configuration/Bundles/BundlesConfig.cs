using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Bundles
{
	public class BundlesConfig
	{
		[JsonProperty("variables")]
		public Dictionary<string, string> Variables { get; set; }

		[JsonProperty("bundles")]
		public List<Bundle> Bundles { get; set; }

		[OnDeserialized]
		internal void OnDeserialize(StreamingContext context)
		{
			if (Variables != null && Bundles != null)
			{
				foreach (var bund in Bundles)
				{
					var pre =
						bund.Includes.Select(inc => Variables.Aggregate(inc, (current, vr) => current.Replace($"{{{vr.Key}}}", vr.Value)))
							.ToList();
					bund.Includes = pre;
				}
			}
		}
	}
}