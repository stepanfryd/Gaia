/*
The MIT License (MIT)

Copyright (c) 2012 Stepan Fryd (stepan.fryd@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/
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