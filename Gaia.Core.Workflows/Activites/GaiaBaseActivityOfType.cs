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
using System.Activities;
using Common.Logging;
using Microsoft.Practices.Unity;

namespace Gaia.Core.Workflows.Activites
{
	/// <summary>
	///   Base generic activity
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class GaiaBaseActivity<T> : CodeActivity<T>
	{
		protected GaiaBaseActivity()
		{
			Log = LogManager.GetLogger(GetType());
		}

		/// <summary>
		///   Workflow aktivity context
		/// </summary>
		protected CodeActivityContext Context { get; private set; }

		/// <summary>
		///   When implemented in a derived class, performs the execution of the activity.
		/// </summary>
		/// <returns>
		///   The result of the activity’s execution.
		/// </returns>
		/// <param name="context">The execution context under which the activity executes.</param>
		protected override T Execute(CodeActivityContext context)
		{
			Context = context;

#if DEBUG
			PreExecute();
#endif

			var retVal = Execute();

#if DEBUG
			PostExecute();
#endif
			return retVal;
		}

		/// <summary>
		///   When implemented in a derived class, performs the execution of the activity.
		/// </summary>
		/// <returns>
		///   The result of the activity’s execution.
		/// </returns>
		protected abstract T Execute();

		private void PreExecute()
		{
			Log.Info($"ACTIVITY {this} - START");
		}

		private void PostExecute()
		{
			Log.Info($"ACTIVITY {this} - FINISH");
		}

		#region Private members

		#endregion

		#region Protected members

		protected ILog Log { get; set; }

		protected IUnityContainer Container => IoC.Container.Instance;

		#endregion
	}
}