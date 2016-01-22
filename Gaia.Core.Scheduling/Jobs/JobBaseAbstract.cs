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
using Quartz;

namespace Gaia.Core.Scheduling.Jobs
{
	/// <summary>
	///   Base job used for application scheduled tasks
	/// </summary>
	public abstract class JobBaseAbstract : IJob
	{
		/// <summary>
		/// Job base class which should be used for all scheduled tasks
		/// </summary>
		protected JobBaseAbstract()
		{
			Logger = LogManager.GetLogger(GetType());
		}

		/// <summary>
		/// Common logger
		/// </summary>
		protected ILog Logger { get; }


		/// <summary>
		/// Called by the <see cref="T:Quartz.IScheduler"/> when a <see cref="T:Quartz.ITrigger"/>
		///             fires that is associated with the <see cref="T:Quartz.IJob"/>.
		/// </summary>
		/// <remarks>
		/// The implementation may wish to set a  result object on the 
		///             JobExecutionContext before this method exits.  The result itself
		///             is meaningless to Quartz, but may be informative to 
		///             <see cref="T:Quartz.IJobListener"/>s or 
		///             <see cref="T:Quartz.ITriggerListener"/>s that are watching the job's 
		///             execution.
		/// </remarks>
		/// <param name="context">The execution context.</param>
		public void Execute(IJobExecutionContext context)
		{
			try
			{
				Logger.InfoFormat("Executing [{0}]", context.JobDetail.Key);
				ExecuteJob(context);
			}
			catch (Exception e)
			{
				Logger.ErrorFormat("Error while executing scheduled Job [{0}]", e, context.JobDetail.Key.Name);
			}
		}

		/// <summary>
		/// Abstract method executes particular job. Overriden method contains task execution
		/// </summary>
		/// <param name="context"></param>
		protected abstract void ExecuteJob(IJobExecutionContext context);
	}
}