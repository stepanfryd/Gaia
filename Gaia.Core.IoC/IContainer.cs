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

		#endregion Public Properties

		#region Public Methods

		bool IsRegistered(Type typeToCheck);

		bool IsRegistered(Type typeToCheck, string nameToCheck);

		bool IsRegistered<T>();

		bool IsRegistered<T>(string nameToCheck);

		object Resolve(Type t, string namem, IDictionary<string, object> parameters = null);

		T Resolve<T>(IDictionary<string, object> parameters = null);

		T Resolve<T>(string name, IDictionary<string, object> parameters = null);

		object Resolve(Type t, IDictionary<string, object> parameters = null);

		IEnumerable<T> ResolveAll<T>(IDictionary<string, object> parameters = null);

		object ContainerInstance { get; }

		#endregion Public Methods
	}

	/// <summary>
	/// Interface specified container functions specification
	/// </summary>
	public interface IContainer<TContainer> : IContainer
	{
		#region Public Properties

		TContainer Instance { get; }

		#endregion Public Properties
	}
}