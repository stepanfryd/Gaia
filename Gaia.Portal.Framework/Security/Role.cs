using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Security
{
	[Serializable]
	[XmlRoot("role")]
	public class Role
	{
		public Role()
		{
			Members = new List<string>();
		}

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlIgnore]
		public List<string> Members { get; private set; }

		[XmlAttribute("members")]
		public string MembersList
		{
			get { return string.Join(";", Members); }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Members = value.Split(';').Select(s => s.Trim()).ToList();
				}
			}
		}
	}
}