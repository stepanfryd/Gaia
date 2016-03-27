using System;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// Type injection member
	/// </summary>
	public class InjectionMember : IInjectionMember
	{
		/// <summary>
		/// Constructor for injection member
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="objVal"></param>
		public InjectionMember(Type type, string name, object objVal)
		{
			Type = type;
			Name = name;
			Object = objVal;
		}

		/// <summary>
		/// Injection member type
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// Member name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Object value
		/// </summary>
		public object Object { get; set; }
	}
}