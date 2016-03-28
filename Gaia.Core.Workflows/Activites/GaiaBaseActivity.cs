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
using Gaia.Core.IoC;

namespace Gaia.Core.Workflows.Activites
{
	/// <summary>
	///   Base activity
	/// </summary>
	public abstract class GaiaBaseActivity : CodeActivity
	{
		#region Public members

		/// <summary>
		///   Workflow kontext aktivity
		/// </summary>
		protected CodeActivityContext Context { get; private set; }

		/// <summary>
		///   Systémový newLine
		/// </summary>
		protected string NewLine => Environment.NewLine;

		#endregion

		#region Constructors

		/// <summary>
		///   Base activit abstract constructor
		/// </summary>
		protected GaiaBaseActivity()
		{
			Log = LogManager.GetLogger(GetType());
		}

		#endregion

		#region Private and protected

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
			Log.Info($"ACTIVITY {this} - START");
		}

		private void PostExecute()
		{
			Log.Info($"ACTIVITY {this} - FINISH");
		}

		#endregion

		#region Private members

		#endregion

		#region Protected members

		/// <summary>
		///   Logging providers
		/// </summary>
		protected ILog Log { get; set; }

		/// <summary>
		///   IoC container reference
		/// </summary>
		protected IContainer Container => IoC.Container.Instance;

		#endregion
	}
}