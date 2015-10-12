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
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gaia.Core.IoC;
using Microsoft.Practices.Unity;

namespace Gaia.Core.Workflows
{
	/// <summary>
	///   Workflow manager class is responsible for workflow invocation
	/// </summary>
	public class WorkflowManager : IWorkflowManager
	{
		/// <summary>
		///   Method invokes activity with arguments and extensions
		/// </summary>
		/// <param name="workflowActivity"></param>
		/// <param name="inputs"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow(Activity workflowActivity, IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null)
		{
			if (workflowActivity == null) throw new ArgumentNullException("workflowActivity");

			var invoker = new WorkflowInvoker(workflowActivity);

			var exts = new List<object>
			{
				new WorkflowInfo
				{
					WorkflowId = Guid.NewGuid(),
					StartTime = DateTime.Now
				}
			};

			if (extensions != null)
			{
				exts.AddRange(extensions);
			}

			foreach (object ext in exts)
				invoker.Extensions.Add(ext);

			if (inputs == null) inputs = new Dictionary<string, object>();
			return new WorkflowResult(invoker.Invoke(inputs));
		}

		/// <summary>
		///   Method invokes activity with arguments and extensions
		/// </summary>
		/// <param name="workflowActivity"></param>
		/// <param name="inputs"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow(Activity workflowActivity, object inputs = null,
			IEnumerable<object> extensions = null)
		{
			return InvokeWorkflow(workflowActivity, GetObjectDictionary(inputs), extensions);
		}

		/// <summary>
		///   Method invokes workflow defined by type and registration name in Unity config if exists
		/// </summary>
		/// <param name="typeRegistrationName">
		///   Unity registration type name ( /unity/container/register[@name] in unity
		///   configuration )
		/// </param>
		/// <param name="inputs">Workflow arguments</param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow(string typeRegistrationName, IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null)
		{
			ContainerRegistration type = Container.Instance.Registrations.SingleOrDefault(c => c.Name == typeRegistrationName);

			if (type == null)
				throw new NullReferenceException(String.Format("In Unity configuration doesn't exist [{0}] registration",
					typeRegistrationName));

			var activity = Container.Instance.Resolve(type.RegisteredType, typeRegistrationName) as Activity;
			return InvokeWorkflow(activity, inputs, extensions);
		}

		/// <summary>
		///   Method invokes workflow defined by type and registration name in Unity config if exists
		/// </summary>
		/// <param name="typeRegistrationName">
		///   Unity registration type name ( /unity/container/register[@name] in unity
		///   configuration )
		/// </param>
		/// <param name="inputs">Workflow arguments</param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow(string typeRegistrationName, object inputs = null,
			IEnumerable<object> extensions = null)
		{
			return InvokeWorkflow(typeRegistrationName, GetObjectDictionary(inputs), extensions);
		}

		/// <summary>
		///   Method invokes selected workflow with dictionary as input parameters
		/// </summary>
		/// <typeparam name="TWorkflowActivity">Workflow type activity to execute</typeparam>
		/// <param name="inputs">Input parameters</param>
		/// <param name="extensions">Workflow extensions</param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow<TWorkflowActivity>(IDictionary<string, object> inputs = null,
			IEnumerable<object> extensions = null)
		{
			return InvokeWorkflow(Container.Instance.Resolve<TWorkflowActivity>() as Activity, inputs, extensions);
		}

		/// <summary>
		///   Method invokes selected workflow with object as input parameters
		/// </summary>
		/// <typeparam name="TWorkflowActivity">Workflow type activity to execute</typeparam>
		/// <param name="inputs">Input parameters</param>
		/// <param name="extensions">Workflow extensions</param>
		/// <returns></returns>
		public WorkflowResult InvokeWorkflow<TWorkflowActivity>(object inputs = null, IEnumerable<object> extensions = null)
		{
			return InvokeWorkflow<TWorkflowActivity>(GetObjectDictionary(inputs), extensions);
		}

		/// <summary>
		///   Flat parameter copy from object to dictionary for processing in the workflow
		/// </summary>
		/// <param name="inputs">Workflow arguments</param>
		/// <returns></returns>
		private Dictionary<string, object> GetObjectDictionary(object inputs)
		{
			if (inputs == null) return null;

			Type inputsType = inputs.GetType();
			CustomAttributeData[] cattr = inputsType.CustomAttributes.ToArray();

			if (inputsType.Namespace == null && inputsType.BaseType == typeof (Object)
			    && inputsType.IsSealed && !inputsType.IsPublic && cattr.Length > 0
			    && cattr[0].AttributeType.Name == "DebuggerDisplayAttribute")
			{
				return inputsType.GetProperties().ToDictionary(pi => pi.Name, pi => pi.GetValue(inputs));
			}


			return (inputsType.GetProperties()
				.SelectMany(
					prop =>
						prop.GetCustomAttributes(true)
							.Where(a => a.GetType() == typeof (InArgumentAttribute) || a.GetType() == typeof (InOutArgumentAttribute)),
					(prop, a) => prop)).ToDictionary(prop => prop.Name, prop => prop.GetValue(inputs));
		}
	}
}