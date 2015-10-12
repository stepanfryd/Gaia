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
using System.Activities;
using Common.Logging;

namespace Gaia.Core.Workflows.Activites
{
	/// <summary>
	///   Base activity
	/// </summary>
	public abstract class GaiaBaseActivity : CodeActivity
	{
		#region Private members

		#endregion

		#region Protected members

		protected ILog Log { get; set; }

		#endregion

		protected GaiaBaseActivity()
		{
			Log = LogManager.GetLogger(GetType());
		}

		/// <summary>
		///   Workflow kontext aktivity
		/// </summary>
		protected CodeActivityContext Context { get; private set; }

		/// <summary>
		///   Systémový newLine
		/// </summary>
		protected string NewLine
		{
			get { return Environment.NewLine; }
		}

		/// <summary>
		///   When implemented in a derived class, performs the execution of the activity.
		/// </summary>
		/// <param name="context">The execution context under which the activity executes.</param>
		protected override void Execute(CodeActivityContext context)
		{
			Context = context;
#if DEBUG
			PreExecute();
#endif

			Execute();

#if DEBUG
			PostExecute();
#endif
		}

		/// <summary>
		///   When implemented in a derived class, performs the execution of the activity.
		/// </summary>
		protected abstract void Execute();

		private void PreExecute()
		{
			Log.Info(String.Format("ACTIVITY {0} - START", this));
		}

		private void PostExecute()
		{
			Log.Info(String.Format("ACTIVITY {0} - FINISH", this));
		}
	}
}