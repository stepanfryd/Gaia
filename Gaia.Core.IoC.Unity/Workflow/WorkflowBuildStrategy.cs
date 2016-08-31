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
/*
 Code copy from https://github.com/dimaKudr/Unity.WF
 */
using System.Activities;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Gaia.Core.IoC.Unity.Workflow
{
	internal sealed class WorkflowBuildStrategy : BuilderStrategy {
		#region Fields and constants

		private readonly IUnityContainer _container;

		#endregion

		#region Constructors

		public WorkflowBuildStrategy(ExtensionContext baseContext) {
			_container = baseContext.Container;
		}

		#endregion

		#region Private and protected

		public override void PostBuildUp(IBuilderContext context) {
			var derivedFromActivity = typeof(Activity).IsAssignableFrom(context.BuildKey.Type);
			if (!derivedFromActivity)
				return;

			var rootActivity = (Activity)context.Existing;
			BuildUpChildActivities(rootActivity);
		}

		private void BuildUpChildActivities(Activity root) {
			var activities =
				WorkflowInspectionServices.GetActivities(root);

			foreach (var activity in activities) {
				var type = activity.GetType();

				var systemActivities = type.Namespace != null && (type.Namespace.StartsWith("System.") || type.Namespace.StartsWith("Microsoft."));

				if (!systemActivities)
					_container.BuildUp(type, activity);

				BuildUpChildActivities(activity);
			}
		}

		#endregion
	}
}