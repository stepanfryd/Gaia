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
using Gaia.Service.Plugins.Scheduler.IoC;
using Quartz;
using System.Collections;
using System.Collections.Generic;

namespace Gaia.Service.Plugins.Scheduler
{
	/// <summary>
	///   Gaia schedulerl service plugin
	/// </summary>
	[Serializable]
	public class SchedulerPlugin : IServicePlugin
	{
		private static readonly object syncRoot = new object();

		private static IList<IScheduler> _schedulers;

		public static IList<IScheduler> Schedulers {
			get {
				if(_schedulers==null) {
					lock(syncRoot) {
						if(_schedulers==null) {
							_schedulers = new List<IScheduler>();
						}
					}
				}
				
				return _schedulers;
			}
		}

		#region Public properties

		/// <summary>
		/// Scheduler name
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Interface Implementations

		#region Events

		/// <summary>
		///   Scheduler plugin has been initialized
		/// </summary>
		public event EventHandler Initialized;

		#endregion

		#endregion

		#region Private and protected

		/// <summary>
		///   Initialized event
		/// </summary>
		protected virtual void OnInitialized()
		{
			Initialized?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Private members

		private readonly ILog _logger;
		private bool _disposed;
		private IScheduler _scheduler;

		#endregion

		#region Constructor and destructor

		/// <summary>
		///   Gaia schedulerl service plugin constructor
		/// </summary>
		public SchedulerPlugin()
		{
			_logger = LogManager.GetLogger(GetType());
			var jobFactory = new IoCJobFactory(Container.Instance);
			var schedulerFactory = new IoCSchedulerFactory(jobFactory);
			_scheduler = schedulerFactory.GetScheduler();
			Schedulers.Add(_scheduler);
		}

		/// <summary>
		///   Gaia schedulerl service plugin destructor
		/// </summary>
		~SchedulerPlugin()
		{
			Dispose(false);
		}

		#endregion

		#region Public methods

		/// <summary>
		///   Plugin has been initialized
		/// </summary>
		public void Initialize()
		{
			// TODO: add declarative configuration hardcoded for devleopment purppose

			if (!_scheduler.IsStarted || _scheduler.InStandbyMode)
			{
				_scheduler.Start();
				// TODO: implelent EVENT => Initialized(this, new EventArgs()); 
				_logger.Info("Scheduler has been initialized");
				OnInitialized();
			}
		}

		/// <summary>
		///   Scheduler plugin has been uninitialized
		/// </summary>
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

					if(Schedulers.Contains(_scheduler)) {
						Schedulers.Remove(_scheduler);
					}
					
					_scheduler = null;
				}
				_disposed = true;
			}
		}

		#endregion
	}
}