using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Gaia.Portal.Framework.Configuration.EntLib
{
	/// <summary>
	///   Enterprise bootstrapper
	/// </summary>
	public class EnterpriseLibrary : IEnterpriseLibrary
	{

		#region Private static fields

		private static bool _isWriterSet;

		private static readonly Lazy<IConfigurationSource> _configuration =
			new Lazy<IConfigurationSource>(ConfigurationSourceFactory.Create);

		private static readonly Lazy<LogWriterFactory> _logWriterFactory =
			new Lazy<LogWriterFactory>(() => new LogWriterFactory(_configuration.Value));

		private static readonly Lazy<ExceptionPolicyFactory> _exceptionFactory = new Lazy<ExceptionPolicyFactory>(() =>
		{
			var factory = new ExceptionPolicyFactory(_configuration.Value);
			factory.InitializeExceptionPolicy();
			return factory;
		});

		#endregion

		#region Constructor

		public EnterpriseLibrary()
		{
			if (!_isWriterSet)
			{
				Logger.SetLogWriter(LogWriterFactory.Create(), false);
			}
		}

		#endregion

		#region Public properties

		public ExceptionPolicyFactory ExceptionFactory => _exceptionFactory.Value;
		public LogWriterFactory LogWriterFactory => _logWriterFactory.Value;
		public ExceptionManager ExceptionManager => ExceptionFactory.CreateManager();

		#endregion

	}
}