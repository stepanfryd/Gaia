using System;

namespace Gaia.Core.Scheduling.Jobs
{
	public class TriggerDataKeyAttribute: Attribute
	{
		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public string DefaultValue { get; set; }
		public string Description { get; set; }
	}
}