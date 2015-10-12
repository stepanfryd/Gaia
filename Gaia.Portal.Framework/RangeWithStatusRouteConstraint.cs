using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Gaia.Portal.Framework
{
	public class RangeWithStatusRouteConstraint : IHttpRouteConstraint
	{
		private readonly int _from;
		private readonly HttpStatusCode _statusCode;
		private readonly int _to;

		public RangeWithStatusRouteConstraint(int from, int to, string statusCode)
		{
			_from = from;
			_to = to;
			if (!Enum.TryParse(statusCode, true, out _statusCode))
			{
				_statusCode = HttpStatusCode.NotFound;
			}
		}

		public RangeWithStatusRouteConstraint(int from, int to, HttpStatusCode statusCode)
		{
			_from = from;
			_to = to;
			_statusCode = statusCode;
		}

		public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName,
			IDictionary<string, object> values, HttpRouteDirection routeDirection)
		{
			object value;
			if (values.TryGetValue(parameterName, out value) && value != null)
			{
				var stringValue = value as string;
				var intValue = 0;

				if (stringValue != null && int.TryParse(stringValue, out intValue))
				{
					if (intValue >= _from && intValue <= _to)
					{
						return true;
					}

					//only throw if we had the expected type of value
					//but it fell out of range
					throw new HttpResponseException(_statusCode);
				}
			}

			return false;
		}
	}
}