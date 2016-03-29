using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Gaia.Core.IoC;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity instance context extension.
	/// </summary>
	public sealed class IoCInstanceContextExtension : IExtension<InstanceContext>
	{
		#region Fields and constants

		#region Fields

		/// <summary>
		///   The child container.
		/// </summary>
		private IContainer _childContainer;

		#endregion

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///   Attaches the specified owner.
		/// </summary>
		/// <param name="owner">
		///   The owner.
		/// </param>
		public void Attach(InstanceContext owner)
		{
		}

		/// <summary>
		///   Detaches the specified owner.
		/// </summary>
		/// <param name="owner">
		///   The owner.
		/// </param>
		public void Detach(InstanceContext owner)
		{
		}

		/// <summary>
		///   Disposes the of child container.
		/// </summary>
		public void DisposeOfChildContainer()
		{
			_childContainer?.Dispose();
		}

		/// <summary>
		///   Gets the child container.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		/// <returns>
		///   The <see cref="IContainer" />.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///   container is null.
		/// </exception>
		public IContainer GetChildContainer(IContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			Contract.EndContractBlock();

			return _childContainer ?? (_childContainer = container.CreateChildContainer());
		}

		#endregion
	}
}