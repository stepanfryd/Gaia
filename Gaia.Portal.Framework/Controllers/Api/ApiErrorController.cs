using System.Net;
using System.Net.Http;
using System.Resources;
using System.Web.Http;
using System.Web.Http.Description;
using Gaia.Portal.Framework.Resources;

namespace Gaia.Portal.Framework.Controllers.Api
{
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ApiErrorController : ApiController
	{
		[HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions, AcceptVerbs("PATCH")]
		public HttpResponseMessage HandleError(HttpStatusCode statusCode)
		{
			var responseMessage = new HttpResponseMessage(statusCode);
			var msgResMngr = new ResourceManager(typeof (HttpStatusMessages));

			responseMessage.ReasonPhrase = msgResMngr.GetString(statusCode.ToString());
			return responseMessage;
		}
	}
}