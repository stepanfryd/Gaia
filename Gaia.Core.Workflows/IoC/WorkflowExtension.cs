/*
 Code copy from https://github.com/dimaKudr/Unity.WF
 */

using System.Activities;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Gaia.Core.Workflows.IoC
{
	public interface IWorkflowConfiguration : IUnityContainerExtensionConfigurator
	{
	}

	public sealed class WorkflowExtension : UnityContainerExtension, IWorkflowConfiguration
	{
		#region Private and protected

		protected override void Initialize()
		{
			Context.Strategies.Add(
				new WorkflowBuildStrategy(Context), UnityBuildStage.PostInitialization);
		}

		#endregion
	}

	internal sealed class WorkflowBuildStrategy : BuilderStrategy
	{
		#region Fields and constants

		private readonly IUnityContainer _container;

		#endregion

		#region Constructors

		public WorkflowBuildStrategy(ExtensionContext baseContext)
		{
			_container = baseContext.Container;
		}

		#endregion

		#region Private and protected

		public override void PostBuildUp(IBuilderContext context)
		{
			var derivedFromActivity = typeof (Activity).IsAssignableFrom(context.BuildKey.Type);
			if (!derivedFromActivity)
				return;

			var rootActivity = (Activity) context.Existing;
			BuildUpChildActivities(rootActivity);
		}

		private void BuildUpChildActivities(Activity root)
		{
			var activities =
				WorkflowInspectionServices.GetActivities(root);

			foreach (var activity in activities)
			{
				var type = activity.GetType();

				var systemActivities =
					type.Namespace.StartsWith("System.") || type.Namespace.StartsWith("Microsoft.");

				if (!systemActivities)
					_container.BuildUp(type, activity);

				BuildUpChildActivities(activity);
			}
		}

		#endregion
	}
}