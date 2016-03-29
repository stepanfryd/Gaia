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

using System.ServiceModel;
using Gaia.Core.IoC;
using Gaia.Core.Wcf.Configuration;
using Gaia.Core.Wcf.IoC;

namespace Gaia.Core.Wcf
{
	/// <summary>
	///   Implementation of Unit IoC container service host factory
	/// </summary>
	public class IoCServiceFactory : ServiceHostFactoryBase
	{
		#region Private and protected

		/// <summary>
		///   This method creates instance of new WCF service host using Unity as IoC container
		/// </summary>
		/// <param name="hostconfig"></param>
		/// <returns></returns>
		public override ServiceHostBase CreateServiceHost(IServiceHostConfiguration hostconfig)
		{
			// TODO: remove hard link to Unity
			var container = (IContainer) Container.Instance.ContainerInstance;
			return new IoCServiceHost(container, hostconfig.ServiceType);
		}

		#endregion
	}
}