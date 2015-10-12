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
using System.Linq.Expressions;

namespace Gaia.Core.Workflows
{
	/// <summary>
	///   Class for Workflow result interpretation, see 
	///   <see>
	///     <cref>WorkflowInvoker.Invoke</cref>
	///   </see>
	/// </summary>
	public class WorkflowResult : Dictionary<string, object>
	{
		#region Constructors and Destructors

		public WorkflowResult(IEnumerable<KeyValuePair<string, object>> workflowArguments)
		{
			foreach (var arg in workflowArguments)
			{
				Add(arg.Key, arg.Value);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		///   Gets Out or InOut parameter value of the workflow
		/// </summary>
		/// <typeparam name="TResult">Argument type</typeparam>
		/// <param name="argumentName">Argument Name</param>
		/// <returns></returns>
		public TResult GetArgument<TResult>(string argumentName)
		{
			if (ContainsKey(argumentName) && this[argumentName].GetType() == typeof (TResult))
			{
				return (TResult) this[argumentName];
			}

			Type type = typeof (TResult);

			return type.IsValueType ? Activator.CreateInstance<TResult>() : default(TResult);
		}

		/// <summary>
		///   Gets Out or InOut parameter value of the workflow
		/// </summary>
		/// <typeparam name="TIn">Input type</typeparam>
		/// <typeparam name="TOut">Output type</typeparam>
		/// <param name="expression">Argument expression</param>
		/// <returns></returns>
		public TOut GetArgument<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
		{
			string body = expression.Body.ToString();
			return GetArgument<TOut>(body.Substring(body.IndexOf(".", StringComparison.Ordinal) + 1));
		}

		#endregion
	}
}