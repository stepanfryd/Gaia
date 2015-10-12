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
using System.Globalization;
using Common.Logging;
using Microsoft.Practices.Unity;
using Quartz;
using Quartz.Spi;

namespace Gaia.Service.Plugins.Scheduler.Unity
{
	/// <summary>
	///   Unity factory pro vytváření nových jobů scheduleru
	/// </summary>
	public class UnityJobFactory : IJobFactory
	{
		#region Fields

		private readonly IUnityContainer _container;

		private readonly ILog _log;

		#endregion

		#region Constructors

		/// <summary>
		///   Konstruktor vytvoření instance Job factory
		/// </summary>
		/// <param name="container">Instance Unity kontaineru</param>
		public UnityJobFactory(IUnityContainer container)
		{
			_container = container;
			_log = LogManager.GetLogger(GetType());
		}

		#endregion

		#region Public methods

		/// <summary>
		///   Called by the scheduler at the time of the trigger firing, in order to
		///   produce a <see cref="IJob" /> instance on which to call Execute.
		/// </summary>
		/// <remarks>
		///   It should be extremely rare for this method to throw an exception -
		///   basically only the the case where there is no way at all to instantiate
		///   and prepare the Job for execution.  When the exception is thrown, the
		///   Scheduler will move all triggers associated with the Job into the
		///   <see cref="TriggerState.Error" /> state, which will require human
		///   intervention (e.g. an application restart after fixing whatever
		///   configuration problem led to the issue wih instantiating the Job.
		/// </remarks>
		/// <param name="bundle">
		///   The TriggerFiredBundle from which the <see cref="IJobDetail" />
		///   and other info relating to the trigger firing can be obtained.
		/// </param>
		/// <param name="scheduler">a handle to the scheduler that is about to execute the job</param>
		/// <throws>  SchedulerException if there is a problem instantiating the Job. </throws>
		/// <returns>
		///   the newly instantiated Job
		/// </returns>
		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			IJobDetail jobDetail = bundle.JobDetail;
			try
			{
				return _container.Resolve(jobDetail.JobType) as IJob;
			}
			catch (Exception ex)
			{
				string message = String.Format(CultureInfo.InvariantCulture, "There is an issue to create job [{0}]. {1}",
					new object[] { jobDetail.JobType.FullName, ex.Message });
				_log.Error(message, ex);

				throw new SchedulerException(message, ex);
			}
		}

		/// <summary>
		///   Allows the the job factory to destroy/cleanup the job if needed.
		/// </summary>
		public void ReturnJob(IJob job)
		{
		}

		#endregion
	}
}