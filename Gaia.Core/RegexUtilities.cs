using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Gaia.Core
{
	/// <summary>
	///   Base regex utilities
	/// </summary>
	public class RegexUtilities
	{
		#region Fields and constants

		private readonly string _emailPattern =
			@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

		private bool _invalidEmail;

		#endregion

		#region Private and protected

		/// <summary>
		///   Check if string is valid email address
		/// </summary>
		/// <param name="strIn"></param>
		/// <returns></returns>
		public bool IsValidEmail(string strIn)
		{
			_invalidEmail = false;
			if (string.IsNullOrEmpty(strIn))
				return false;

			// Use IdnMapping class to convert Unicode domain names.
			try
			{
				strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
					RegexOptions.None, TimeSpan.FromMilliseconds(200));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}

			if (_invalidEmail)
				return false;

			// Return true if strIn is in valid e-mail format.
			try
			{
				return Regex.IsMatch(strIn, _emailPattern, RegexOptions.IgnoreCase);
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}

		private string DomainMapper(Match match)
		{
			// IdnMapping class with default property values.
			var idn = new IdnMapping();

			var domainName = match.Groups[2].Value;
			try
			{
				domainName = idn.GetAscii(domainName);
			}
			catch (ArgumentException)
			{
				_invalidEmail = true;
			}
			return match.Groups[1].Value + domainName;
		}

		#endregion
	}
}