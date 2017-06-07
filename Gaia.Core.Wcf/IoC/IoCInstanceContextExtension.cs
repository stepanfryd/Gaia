using System.ServiceModel;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity instance context extension.
	/// </summary>
	public sealed class IoCInstanceContextExtension : IExtension<InstanceContext>
	{
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

		#endregion
	}
}