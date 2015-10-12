using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Optimization;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Bundles
{
	public class Bundle
	{
		[JsonProperty("type")]
		public BundleType Type { get; set; }

		[JsonProperty("path")]
		public string Path { get; set; }

		[JsonProperty("orderer")]
		public Type OrdererType { get; set; }

		[JsonIgnore]
		public IBundleOrderer Orderer { get; set; }

		[JsonProperty("includes")]
		public List<string> Includes { get; set; }

		[OnDeserialized]
		internal void OnDeserialized(StreamingContext context)
		{
			if (OrdererType != null)
			{
				Orderer = Activator.CreateInstance(OrdererType) as IBundleOrderer;
			}
		}
	}
}