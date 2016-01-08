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
using Common.Logging;
using Gaia.Core.IoC;
using Gaia.Core.Services;
using Gaia.Service.Plugins.Scheduler.Unity;
using Microsoft.Practices.Unity;
using Quartz;

namespace Gaia.Service.Plugins.Scheduler
{
	[Serializable]
	public class SchedulerPlugin : IServicePlugin
	{
		#region Public members

		#region Public properties

		public string Name { get; set; }

		#endregion

		#endregion

		#region Interface Implementations

		#region Events

		public event EventHandler Initialized;

		#endregion

		#endregion

		#region Private members

		private readonly ILog _logger;
		private bool _disposed;
		private IScheduler _scheduler;

		#endregion

		#region Constructor and destructor

		public SchedulerPlugin()
		{
			_logger = LogManager.GetLogger(GetType());

			// TODO: remove hard link to Unity
			var container = (IUnityContainer)Container.Instance.ContainerInstance;

			var jobFactory = new UnityJobFactory(container);
			var schedulerFactory = new UnitySchedulerFactory(jobFactory);
			_scheduler = schedulerFactory.GetScheduler();
		}

		~SchedulerPlugin()
		{
			Dispose(false);
		}

		#endregion

		#region Public methods

		public void Initialize()
		{
			// TODO: add declarative configuration hardcoded for devleopment purppose

			if (!_scheduler.IsStarted || _scheduler.InStandbyMode)
			{
				_scheduler.Start();
				// TODO: implelent EVENT => Initialized(this, new EventArgs()); 
				_logger.Info("Scheduler has been initialized");
			}
		}

		public void Uninitialize()
		{
			_logger.Info("Scheduler is going to shutdown");
			_scheduler.Shutdown(true);
			_logger.Info("Scheduler has shutdown");
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (!_scheduler.IsShutdown)
					{
						Uninitialize();
					}
					_scheduler = null;
				}
				_disposed = true;
			}
		}

		#endregion
	}
}