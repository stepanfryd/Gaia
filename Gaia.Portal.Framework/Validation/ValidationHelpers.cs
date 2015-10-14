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
