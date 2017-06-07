using System;
using System.Collections.Generic;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// Interface specified container functions specification
	/// </summary>
	public interface IContainer : IDisposable
	{
		#region Public Properties

		string ConfigSource { get; set; }
		ConfigSourceType ConfigSourceType { get; set; }
		object ContainerInstance { get; }

		#endregion Public Properties

		#region Public Methods

		bool IsRegistered(Type typeToCheck);
		bool IsRegistered(Type typeToCheck, string nameToCheck);
		bool IsRegistered<T>();
		bool IsRegistered<T>(string nameToCheck);
		object Resolve(Type t, string name);
		T Resolve<T>();
		T Resolve<T>(string name);
		object Resolve(Type t);
		IEnumerable<T> ResolveAll<T>();

		#endregion Public Methods
	}
}