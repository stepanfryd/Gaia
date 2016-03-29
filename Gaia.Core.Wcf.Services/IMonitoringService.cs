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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Gaia.Core.Wcf.Services
{
	/// <summary>
	/// Monitoring service interface definition
	/// </summary>
	[ServiceContract(Name = "IMonitoringService", Namespace = Constants.WCF_NAMESPACE)]
	public interface IMonitoringService
	{
		/// <summary>
		/// Gets services status
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[OperationContract]
		ServiceStatus GetStatus(int input);

		// TODO: Add your service operations here
	}

	/// <summary>
	/// Service status data contract
	/// </summary>
	[DataContract]
	public class ServiceStatus
	{
		/// <summary>
		/// Status result
		/// </summary>
		[DataMember]
		public string Result { get; set; }

		/// <summary>
		/// Service is running
		/// </summary>
		[DataMember]
		public bool IsRunning { get; set; }

		/// <summary>
		/// List of running pluggins
		/// </summary>
		[DataMember]
		public List<string> Plugins { get; set; }

		/// <summary>
		/// List of Wcf services
		/// </summary>
		[DataMember]
		public List<string> WcfServices { get; set; }
	}
}