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

namespace Gaia.Core.Services
{
	/// <summary>
	///   Attribute hold information about system service
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ServiceInfoAttribute : Attribute
	{
		/// <summary>
		///   Service name
		/// </summary>
		public string ServiceName { get; set; }

		/// <summary>
		///   Service display name
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		///   Service description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///   Service info attribute contstructor
		/// </summary>
		/// <param name="serviceName">Service name as is in the service list registrations</param>
		/// <param name="displayName">Service display name</param>
		/// <param name="description">Service description</param>
		public ServiceInfoAttribute(string serviceName, string displayName, string description)
		{
			ServiceName = serviceName;
			DisplayName = displayName;
			Description = description;
		}
	}
}