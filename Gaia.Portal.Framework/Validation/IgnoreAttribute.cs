using System;
using System.Collections.Generic;

namespace Gaia.Portal.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class IgnoreAttribute : Attribute
	{
		public IgnoreAttribute(params string[] properties)
		{
			Properties = new List<string>(properties);
		}

		public List<string> Properties { get; private set; }
	}
}