using System;
using NParametrizer;

namespace Gaia.Portal.Framework.Configuration
{
	public class Configuration : ParametersBase, IConfiguration
	{
		[Config("gaia/settings")]
		public ISettings ApplicationSettings { get; set; }

		protected override void ValidateArguments()
		{
		}
	}
}