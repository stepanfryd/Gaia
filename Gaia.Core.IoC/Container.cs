/*
The MIT License (MIT)

Copyright (c) 2012 Stepan Fryd (stepan.fryd@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Gaia.Core.IoC
{
	/// <summary>
	///   Specifies the Unity configuration for the main container.
	/// </summary>
	public class Container
	{
		#region Fields and constants

		private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		#endregion

		#region Public members

		#region Unity Container

		/// <summary>
		///   Gets the configured Unity container.
		/// </summary>
		public static IUnityContainer Instance => LazyContainer.Value;

		#endregion

		#endregion

		#region Private and protected

		/// <summary>Registers the type mappings with the Unity container.</summary>
		/// <param name="unityContainer">The unity container to configure.</param>
		/// <remarks>
		///   There is no need to register concrete types such as controllers or API controllers (unless you want to
		///   change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
		/// </remarks>
		public static void RegisterTypes(IUnityContainer unityContainer)
		{
			unityContainer.LoadConfiguration();
		}

		#endregion
	}
}