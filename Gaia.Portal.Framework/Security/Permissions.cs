using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Security
{
	[Serializable]
	[XmlRoot("permissions")]
	public class Permissions
	{
		private Route[] _routes;

		[XmlArray("roles")]
		[XmlArrayItem("role")]
		public List<Role> Roles { get; set; }

		[XmlArray("routes")]
		[XmlArrayItem("route", IsNullable = false)]
		public Route[] Routes
		{
			get
			{
				if (_routes == null) _routes = new Route[0];

				return _routes.OrderByDescending(r => r.RouteValues.Count).ToArray();
			}
			set { _routes = value; }
		}
	}
}