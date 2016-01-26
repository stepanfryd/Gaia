using System;

namespace Gaia.Core.IoC
{
	public class InjectionMember : IInjectionMember
	{
		public InjectionMember(Type type, string name, object objVal)
		{
			Type = type;
			Name = name;
			Object = objVal;
		}

		public Type Type { get; set; }
		public string Name { get; set; }
		public object Object { get; set; }
	}
}