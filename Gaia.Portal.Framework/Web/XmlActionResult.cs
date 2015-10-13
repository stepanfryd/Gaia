using System.Web.Mvc;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Web
{
	public class XmlActionResult<T> : ActionResult
	{
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
		public XmlActionResult(T objectToSerialize)
		{
			ObjectToSerialize = objectToSerialize;
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
				xs.Serialize(context.HttpContext.Response.Output, ObjectToSerialize);
			}
		}

		#endregion
	}
}