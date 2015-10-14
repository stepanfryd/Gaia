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