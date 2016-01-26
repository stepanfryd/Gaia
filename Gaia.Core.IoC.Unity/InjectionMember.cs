using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Gaia.Core.IoC.Unity
{
	public class InjectionMember : Microsoft.Practices.Unity.InjectionMember
	{
		public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
		{
			
		}
	}
}