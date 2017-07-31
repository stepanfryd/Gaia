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
namespace Gaia.Core.Services
{
	/// <summary>
	/// Interface for windows service
	/// </summary>
	public interface IGaiaService
	{

		/// <summary>
		/// Service controller managing service
		/// </summary>
		IServiceController ServiceController { get; set; }

		/// <summary>
		/// Starts srevice
		/// </summary>
		void Start();

		/// <summary>
		/// Stops service
		/// </summary>
		void Stop();

		/// <summary>
		/// Let's service continues
		/// </summary>
		void Continue();

		/// <summary>
		/// Service executes custom command after it is received
		/// </summary>
		void CustomCommandReceived();

		/// <summary>
		/// Pauses service
		/// </summary>
		void Pause();

		/// <summary>
		/// Shutdown the service
		/// </summary>
		void Shutdown();
	}
}