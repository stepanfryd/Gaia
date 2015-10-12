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
using Common.Logging;
using Gaia.Core.Workflows;
using Microsoft.Practices.Unity;

namespace Gaia.Core.Wcf.Services
{
	public class MonitoringService : IMonitoringService
	{
		private ILog _log;

		public MonitoringService()
		{
			_log = LogManager.GetLogger("MonitoringService");
		}

		[Dependency]
		public IWorkflowManager WorkflowManager { get; set; }

		public ServiceStatus GetStatus(int input)
		{
			return new ServiceStatus
			{
				Result = "Some result",
				IsRunning = true,
				Plugins = new List<string>
				{
					"Service 1",
					"Service 2",
					"Service 3"
				},
				WcfServices = new List<string>
				{
					"Wcf1",
					"Wcf2",
					"Wcf3"
				}
			};
		}
	}
}