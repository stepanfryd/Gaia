using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Gaia.Portal.Framework.Validation
{
	public static class ValidationHelpers
	{
		/// <summary>
		/// Clear for WebAPI
		/// </summary>
		/// <param name="mInfo"></param>
		/// <param name="modelState"></param>
		public static void ClearIgnoredProperties(MethodInfo mInfo, IDictionary<string, System.Web.Http.ModelBinding.ModelState> modelState)
		{
			foreach (var k in from param in mInfo.GetParameters()
												let atr = param.GetCustomAttribute<IgnoreAttribute>()
												where atr != null
												from k in atr.Properties.SelectMany(prp => (from k in modelState.Keys
																																		let mk = Regex.Replace(k, $"^{param.Name}\\.", "")
																																		where Regex.IsMatch(mk, prp)
																																		select k))
												select k)
			{
				modelState[k].Errors.Clear();
			}
		}

		/// <summary>
		/// Clear for MVC
		/// </summary>
		/// <param name="mInfo"></param>
		/// <param name="modelState"></param>
		public static void ClearIgnoredProperties(MethodInfo mInfo, IDictionary<string, System.Web.Mvc.ModelState> modelState)
		{
			foreach (var k in from param in mInfo.GetParameters()
												let atr = param.GetCustomAttribute<IgnoreAttribute>()
												where atr != null
												from k in atr.Properties.SelectMany(prp => (from k in modelState.Keys
																																		let mk = Regex.Replace(k, $"^{param.Name}\\.", "")
																																		where Regex.IsMatch(mk, prp)
																																		select k))
												select k)
			{
				modelState[k].Errors.Clear();
			}
		}
	}
}
