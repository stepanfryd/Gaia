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