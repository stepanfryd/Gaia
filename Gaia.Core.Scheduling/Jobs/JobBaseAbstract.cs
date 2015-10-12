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
		protected JobBaseAbstract()
		{
			Logger = LogManager.GetLogger(GetType());
		}

		protected ILog Logger { get; }

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

		protected abstract void ExecuteJob(IJobExecutionContext context);
	}
}