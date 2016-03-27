using NParametrizer;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// Container configuration settings
	/// </summary>
	public class Settings : ParametersBase
	{
		#region Public members

		/// <summary>
		///   Configuration section
		/// </summary>
		[Config("gaia/ioc")]
		public IoCSection ContainerSection { get; set; }

		#endregion

		#region Private and protected

		/// <summary>
		///   Custom parameter validation
		/// </summary>
		protected override void ValidateArguments()
		{
		}

		#endregion
	}
}