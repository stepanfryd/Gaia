using System;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// Interface for Injection member identification
	/// </summary>
	public interface IInjectionMember
	{
		/// <summary>
		/// Injection member type
		/// </summary>
		Type Type { get; set; }

		/// <summary>
		/// Object value
		/// </summary>
		object Object { get; set; }

		/// <summary>
		/// Member name
		/// </summary>
		string Name { get; set; }
	}
}