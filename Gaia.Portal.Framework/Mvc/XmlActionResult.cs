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
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Mvc
{
	public class XmlActionResult<T> : ActionResult
	{
		private XmlSerializerNamespaces _namespaces;

		#region Public members

		/// <summary>
		///   Gets the object to be serialized to XML.
		/// </summary>
		public T ObjectToSerialize { get; }



		#endregion

		#region Constructors

		/// <summary>
		///   Initializes a new instance of the <see cref="XmlResult" /> class.
		/// </summary>
		/// <param name="objectToSerialize">The object to serialize to XML.</param>
		/// <param name="namespaces"></param>
		public XmlActionResult(T objectToSerialize, XmlSerializerNamespaces namespaces = null)
		{
			ObjectToSerialize = objectToSerialize;
			_namespaces = namespaces;	
		}

		#endregion

		#region Private and protected

		/// <summary>
		///   Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result
		///   stream.
		/// </summary>
		/// <param name="context">The controller context for the current request.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (ObjectToSerialize != null)
			{
				context.HttpContext.Response.Clear();
				var xs = new XmlSerializer(typeof (T));
				context.HttpContext.Response.ContentType = "text/xml";
				xs.Serialize(context.HttpContext.Response.Output, ObjectToSerialize, _namespaces);
			}
		}

		#endregion
	}
}