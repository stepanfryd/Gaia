using System;
using System.Collections.Generic;

namespace Gaia.Core.IoC
{
	/// <summary>
	///   Interface specified container functions specification
	/// </summary>
	public interface IContainerProvider
	{
		/// <summary>
		///   Resolve an instance of the requested type with the given name from the container.
		/// </summary>
		/// <param name="t"><see cref="T:System.Type" /> of object to get from the container.</param>
		/// <param name="name">Name of the object to retrieve.</param>
		/// <returns>
		///   The retrieved object.
		/// </returns>
		object Resolve(Type t, string name);

		/// <summary>
		///   Return instances of all registered types requested.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method is useful if you've registered multiple types with the same
		///     <see cref="T:System.Type" /> but different names.
		///   </para>
		///   <para>
		///     Be aware that this method does NOT return an instance for the default (unnamed) registration.
		///   </para>
		/// </remarks>
		/// <param name="t">The type requested.</param>
		/// <returns>
		///   Set of objects of type <paramref name="t" />.
		/// </returns>
		IEnumerable<object> ResolveAll(Type t);


		/// <summary>
		///   Resolve an instance of the default requested type from the container.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.Type" /> of object to get from the container.</typeparam>
		/// <returns>
		///   The retrieved object.
		/// </returns>
		T Resolve<T>();

		/// <summary>
		///   Resolve an instance of the requested type with the given name from the container.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.Type" /> of object to get from the container.</typeparam>
		/// <param name="name">Name of the object to retrieve.</param>
		/// <returns>
		///   The retrieved object.
		/// </returns>
		T Resolve<T>(string name);

		/// <summary>
		///   Resolve an instance of the default requested type from the container.
		/// </summary>
		/// <param name="t"><see cref="T:System.Type" /> of object to get from the container.</param>
		/// <returns>
		///   The retrieved object.
		/// </returns>
		object Resolve(Type t);

		/// <summary>
		///   Return instances of all registered types requested.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method is useful if you've registered multiple types with the same
		///     <see cref="T:System.Type" /> but different names.
		///   </para>
		///   <para>
		///     Be aware that this method does NOT return an instance for the default (unnamed) registration.
		///   </para>
		/// </remarks>
		/// <typeparam name="T">The type requested.</typeparam>
		/// <returns>
		///   Set of objects of type <typeparamref name="T" />.
		/// </returns>
		IEnumerable<T> ResolveAll<T>();

		/// <summary>
		/// Instance of current container
		/// </summary>
		object ContainerInstance { get; }
	}
}