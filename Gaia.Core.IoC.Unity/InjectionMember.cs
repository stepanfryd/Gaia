using System;
using Unity.Policy;
using UnityRegistration = Unity.Registration;

namespace Gaia.Core.IoC.Unity
{
	public class InjectionMember : UnityRegistration.InjectionMember
	{
		public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
		{
			
		}
	}
}