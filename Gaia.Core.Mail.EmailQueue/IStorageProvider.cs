﻿/*
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
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gaia.Core.Mail.EmailQueue
{
	public interface IStorageProvider
	{
		/// <summary>
		///   Method stores message to storage
		/// </summary>
		/// <param name="messageId">Message unique identificator</param>
		/// <param name="message">Mail message to store</param>
		/// <param name="plannedtime">
		///   Planned time when to send. If plannedTime is null then message is ready to send in the next
		///   round
		/// </param>
		Task SaveMessageAsync(object messageId, MailMessage message, DateTime? plannedtime = null);
	}

	/// <summary>
	///   Interface defines mail storage provider
	/// </summary>
	public interface IStorageProvider<TMessage, TDeliveryInfo> : IStorageProvider
	{
		/// <summary>
		///   Method returns dictionary of messageId and messages which are ready to send.
		/// </summary>
		/// <param name="total"></param>
		/// <param name="plannedtime">Time when to send messages</param>
		/// <param name="skip"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<IDictionary<object, Tuple<TMessage, List<TDeliveryInfo>>>> GetMessagesAsync(DateTime? plannedtime = null);

		Task UpdateSentRecipientsAsync(object messageId, TDeliveryInfo deliveryInfo);

		Task FinishMailAsync(object messageId);

		Task UpdateMailAsync(object messageId, object status);
	}
}