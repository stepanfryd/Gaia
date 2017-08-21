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
using System.Collections.Generic;

namespace Gaia.Service.Plugins.Scheduler
{
	/// <summary>
	///   Gaia schedulerl service plugin
	/// </summary>
	[Serializable]
	public class SchedulerPlugin : IServicePlugin
	{
		private static readonly object SyncRoot = new object();
		private static IList<IScheduler> _schedulers;

		public static IList<IScheduler> Schedulers {
			get {
				if(_schedulers==null) {
					lock(SyncRoot) {
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

		/// <summary>
		/// Scheduler registered name
		/// </summary>
		public string SchedulerName { get; set; }

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
		private readonly IoCSchedulerFactory _schedulerFactory;

		#endregion

		#region Constructor and destructor

		/// <summary>
		///   Gaia schedulerl service plugin constructor
		/// </summary>
		public SchedulerPlugin()
		{
			_logger = LogManager.GetLogger(GetType());
			var jobFactory = new IoCJobFactory(Container.Instance);
			_schedulerFactory = new IoCSchedulerFactory(jobFactory);
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
			_logger.Info("Scheduler has been initialized");
			OnInitialized();
		}

		/// <summary>
		///   Scheduler plugin has been uninitialized
		/// </summary>
		public void Uninitialize()
		{		
			Stop();
			_logger.Info("Scheduler has been uninitialized");
		}

		/// <summary>
		/// Start service plugin
		/// </summary>
		public void Start()
		{
			try
			{
				_scheduler = _schedulerFactory.GetScheduler();		
				SchedulerName = _scheduler.SchedulerName;
				Schedulers.Add(_scheduler);
				_scheduler.Start();
				_logger.Info($"Scheduler [{SchedulerName}] has been succesfully started");
			}
			catch (Exception e)
			{
				_logger.Error($"Scheduler [{SchedulerName}] start error", e);
			}
		}

		/// <summary>
		/// Pause service plugin. Scheduler goes to Stand By mode. Continue action executes Start action on scheduler
		/// </summary>
		public void Pause()
		{
			try
			{
				if (_scheduler != null && _scheduler.IsStarted)
				{
					_scheduler.Standby();
				}
				_logger.Info($"Scheduler [{SchedulerName}] has been succesfully paused (stand by mode)");
			}
			catch (Exception e)
			{
				_logger.Error($"Scheduler [{SchedulerName}] pause error", e);
			}
		}

		/// <summary>
		/// Stop service plugin
		/// </summary>
		public void Stop()
		{
			try
			{
				if (_scheduler != null)
				{
					_scheduler.Shutdown();

					if (Schedulers.Contains(_scheduler))
					{
						Schedulers.Remove(_scheduler);
					}

					_scheduler = null;
				}
				_logger.Info($"Scheduler [{SchedulerName}] has been succesfully terminated (stop action)");
			}
			catch (Exception e)
			{
				_logger.Error($"Scheduler [{SchedulerName}] stop error", e);
			}
		}

		/// <summary>
		/// Continue with service plugin. Scheduler 
		/// </summary>
		public void Continue()
		{
			try
			{
				if (_scheduler != null && _scheduler.InStandbyMode)
				{
					_scheduler.Start();
					_logger.Info($"Scheduler [{SchedulerName}] has been started from stand by mode");
				}
				else
				{
					_logger.Warn($"Scheduler [{SchedulerName}] has not been in stand by mode. Can't start. Service must be stop and start again.");
				}
				
			}
			catch (Exception e)
			{
				_logger.Error($"Scheduler [{SchedulerName}] continue error", e);
			}
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
					Uninitialize();
				}
				_disposed = true;
			}
		}

		#endregion
	}
}