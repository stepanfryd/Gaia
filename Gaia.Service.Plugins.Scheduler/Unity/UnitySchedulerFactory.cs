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
using Quartz;
using Quartz.Core;
using Quartz.Impl;

namespace Gaia.Service.Plugins.Scheduler.Unity
{
	/// <summary>
	///   Factory pro podporu Unity v Scheduleru
	/// </summary>
	public class UnitySchedulerFactory : StdSchedulerFactory
	{
		#region Fields

		private readonly UnityJobFactory _unityJobFactory;

		#endregion

		#region Constructors

		/// <summary>
		///   Konstruktor
		/// </summary>
		/// <param name="unityJobFactory"></param>
		public UnitySchedulerFactory(UnityJobFactory unityJobFactory)
		{
			_unityJobFactory = unityJobFactory;
		}

		#endregion

		#region Protected methods

		/// <summary>
		///   Vytvoří instanci Quartz scheduleru s podporou UnityJobFactory
		/// </summary>
		/// <param name="rsrcs"></param>
		/// <param name="qs"></param>
		/// <returns></returns>
		protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
		{
			qs.JobFactory = _unityJobFactory;
			return base.Instantiate(rsrcs, qs);
		}

		#endregion
	}
}