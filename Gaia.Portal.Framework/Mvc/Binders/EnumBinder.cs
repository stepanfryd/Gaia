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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gaia.Portal.Framework.Mvc.Binders
{
	public class EnumBinder<T> : IModelBinder
	{
		#region Interface Implementations

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
			{
				return Task.FromResult(default(T));
			}

			try
			{
				return Task.FromResult((T) Enum.Parse(typeof(T), valueProviderResult.FirstValue));
			}
			catch
			{
				return Task.FromResult(default(T));
			}
		}

		#endregion
	}

	public class EnumBinderNullableBinder<T> : IModelBinder
	{
		#region Interface Implementations

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
			{
				return null;
			}

			try
			{
				return Task.FromResult((T) Enum.Parse(typeof(T), valueProviderResult.FirstValue));
			}
			catch
			{
				return Task.FromResult(default(T));
			}
		}

		#endregion
	}
}