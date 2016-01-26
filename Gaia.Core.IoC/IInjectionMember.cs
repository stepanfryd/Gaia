using System;

namespace Gaia.Core.IoC
{
	public interface IInjectionMember
	{
		Type Type { get; set; }
		object Object { get; set; }
		string Name { get; set; }
	}
}