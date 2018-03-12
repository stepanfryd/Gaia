using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using IAutofacContainer = Autofac.IContainer;

namespace Gaia.Core.IoC.Autofac
{
	public class Container : IContainer<IAutofacContainer>
	{
		private static string _configSource;
		private static ConfigSourceType _configSourceType = ConfigSourceType.Xml;

		private static readonly Lazy<IAutofacContainer> LazyContainer = new Lazy<IAutofacContainer>(() =>
		{
			// Add the configuration to the ConfigurationBuilder.
			var config = new ConfigurationBuilder();
			// config.AddJsonFile comes from Microsoft.Extensions.Configuration.Json
			// config.AddXmlFile comes from Microsoft.Extensions.Configuration.Xml

			if (!String.IsNullOrEmpty(_configSource))
			{
				config.AddXmlFile(_configSource);
			}
			else
			{
				config.AddXmlFile($"autofac.{_configSourceType.ToString().ToLower()}");
			}

			var module = new ConfigurationModule(config.Build());
			var builder = new ContainerBuilder();

			builder.RegisterModule(module);

			return builder.Build();
		});

		public string ConfigSource {
			get => _configSource;
			set => _configSource = value;
		}

		public object ContainerInstance => Instance;
		public IAutofacContainer Instance => LazyContainer.Value;

		public ConfigSourceType ConfigSourceType {
			get => _configSourceType;
			set => _configSourceType = value;
		}

		public bool IsRegistered(Type typeToCheck)
		{
			return Instance.IsRegistered(typeToCheck);
		}

		public bool IsRegistered(Type typeToCheck, string nameToCheck)
		{
			return Instance.IsRegisteredWithName(nameToCheck, typeToCheck);
		}

		public bool IsRegistered<T>()
		{
			return IsRegistered(typeof(T));
		}

		public bool IsRegistered<T>(string nameToCheck)
		{
			return IsRegistered(typeof(T), nameToCheck);
		}

		public object Resolve(Type t, string name, IDictionary<string, object> parameters = null)
		{
			return Instance.ResolveNamed(name, t, GetParameters(parameters));
		}

		public T Resolve<T>(IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve<T>(GetParameters(parameters));
		}

		public T Resolve<T>(string name, IDictionary<string, object> parameters = null)
		{
			return Instance.ResolveNamed<T>(name, GetParameters(parameters));
		}

		public object Resolve(Type t, IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve(t, GetParameters(parameters));
		}

		public IEnumerable<T> ResolveAll<T>(IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve<IEnumerable<T>>(GetParameters(parameters));
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Container() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion IDisposable Support

		private IEnumerable<NamedParameter> GetParameters(IDictionary<string, object> parameters)
		{
			if (parameters == null || parameters.Count == 0) return null;

			return parameters.Select(i => new NamedParameter(i.Key, i.Value));
		}
	}
}