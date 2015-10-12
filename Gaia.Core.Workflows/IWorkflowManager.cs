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
using System.Activities;
using System.Collections.Generic;

namespace Gaia.Core.Workflows
{
	/// <summary>
	///   Interface for workflow manager
	/// </summary>
	public interface IWorkflowManager
	{
		/// <summary>
		///   Method invokes activity with arguments and extensions
		/// </summary>
		/// <param name="workflowActivity"></param>
		/// <param name="inputs"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow(Activity workflowActivity, IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null);

		/// <summary>
		///   Method invokes activity with arguments and extensions
		/// </summary>
		/// <param name="workflowActivity"></param>
		/// <param name="inputs"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow(Activity workflowActivity, object inputs = null,
			IEnumerable<object> extensions = null);


		/// <summary>
		///   Method invokes workflow defined by type and registration name in Unity config if exists
		/// </summary>
		/// <param name="typeRegistrationName">Unity registration type name ( /unity/container/register[@name] in unity configuration )</param>
		/// <param name="inputs">Workflow arguments</param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow(string typeRegistrationName, IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null);

		/// <summary>
		///   Method invokes workflow defined by type and registration name in Unity config if exists
		/// </summary>
		/// <param name="typeRegistrationName">Unity registration type name ( /unity/container/register[@name] in unity configuration )</param>
		/// <param name="inputs">Workflow arguments</param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow(string typeRegistrationName, object inputs = null,
			IEnumerable<object> extensions = null);

		/// <summary>
		///   Method for workflow invoking
		/// </summary>
		/// <typeparam name="TWorkflowActivity">Workflow activity type</typeparam>
		/// <param name="inputs">Input arguments in dictionary</param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow<TWorkflowActivity>(IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null);

		/// <summary>
		///   Method for workflow invoking
		/// </summary>
		/// <typeparam name="TWorkflowActivity">Workflow activity type</typeparam>
		/// <param name="inputs">Input arguments in dictionary as object. Each argument is a property of a nobject</param>
		/// <param name="extensions">Workflow extensions</param>
		/// <returns></returns>
		WorkflowResult InvokeWorkflow<TWorkflowActivity>(object inputs = null, IEnumerable<object> extensions = null);
	}
}