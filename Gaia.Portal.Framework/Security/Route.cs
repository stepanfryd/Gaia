using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Security
{
	[Serializable]
	[XmlRoot("route")]
	public class Route
	{
		public Route()
		{
			RouteValues = new Dictionary<string, IList<string>>();
		}

		[XmlIgnore]
		public IDictionary<string, IList<string>> RouteValues { get; set; }

		[XmlIgnore]
		public List<string> Roles { get; private set; }

		[XmlAttribute("roles")]
		public string RolesList
		{
			get { return string.Join(",", Roles); }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Roles = value.Split(',').Select(s => s.Trim()).ToList();
				}
			}
		}

		[XmlText]
		public string Value
		{
			get { return null; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				var rvals = value.ToLowerInvariant().Trim().Split(';').Select(s => s.Trim());
				foreach (var rv in rvals)
				{
					var rvLocal = rv.TrimEnd(']');
					var sep = rvLocal.IndexOf("[");
					if (sep < 0) continue;

					var key = rvLocal.Substring(0, sep);
					var values = rvLocal.Substring(sep + 1);

					if (!RouteValues.ContainsKey(key))
					{
						RouteValues.Add(key, values.Split(',').Select(s => s.Trim()).ToArray());
					}
				}
			}
		}
	}
}