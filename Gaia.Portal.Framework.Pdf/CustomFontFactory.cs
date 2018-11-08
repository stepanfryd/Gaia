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

using iText.Kernel.Font;
/*
namespace Gaia.Portal.Framework.Pdf
{
	public class CustomFontFactory
	{
		#region Fields and constants

		public const float DEFAULT_FONT_SIZE = 12;
		public const int DEFAULT_FONT_STYLE = 0;
		public static readonly BaseColor DEFAULT_FONT_COLOR = BaseColor.BLACK;

		#endregion

		#region Public members

		public string DefaultFontPath { get; }
		public string DefaultFontEncoding { get; }
		public bool DefaultFontEmbedding { get; }
		public float DefaultFontSize { get; }
		public int DefaultFontStyle { get; }
		public BaseColor DefaultFontColor { get; private set; }

		public bool ReplaceEncodingWithDefault { get; set; }
		public bool ReplaceEmbeddingWithDefault { get; set; }
		public bool ReplaceFontWithDefault { get; set; }
		public bool ReplaceSizeWithDefault { get; set; }
		public bool ReplaceStyleWithDefault { get; set; }
		public bool ReplaceColorWithDefault { get; set; }

		public BaseFont DefaultBaseFont { get; protected set; }

		#endregion

		#region Constructors

		public CustomFontFactory(
			string defaultFontFilePath,
			string defaultFontEncoding = BaseFont.IDENTITY_H,
			bool defaultFontEmbedding = BaseFont.EMBEDDED,
			float? defaultFontSize = null,
			int? defaultFontStyle = null,
			BaseColor defaultFontColor = null,
			bool automaticalySetReplacementForNullables = true)
		{
			//set default font properties
			DefaultFontPath = defaultFontFilePath;
			DefaultFontEncoding = defaultFontEncoding;
			DefaultFontEmbedding = defaultFontEmbedding;
			DefaultFontColor = defaultFontColor ?? DEFAULT_FONT_COLOR;
			DefaultFontSize = defaultFontSize ?? DEFAULT_FONT_SIZE;
			DefaultFontStyle = defaultFontStyle ?? DEFAULT_FONT_STYLE;

			//set default replacement options
			ReplaceFontWithDefault = false;
			ReplaceEncodingWithDefault = true;
			ReplaceEmbeddingWithDefault = false;

			if (automaticalySetReplacementForNullables)
			{
				ReplaceSizeWithDefault = defaultFontSize.HasValue;
				ReplaceStyleWithDefault = defaultFontStyle.HasValue;
				ReplaceColorWithDefault = defaultFontColor != null;
			}

			//define default font
			DefaultBaseFont = BaseFont.CreateFont(DefaultFontPath, DefaultFontEncoding, DefaultFontEmbedding);

			//register system fonts
			FontFactory.RegisterDirectories();
		}

		#endregion

		#region Private and protected

		protected PdfFont GetBaseFont(float size, int style, BaseColor color)
		{
			var baseFont = new PdfFont(DefaultBaseFont, size, style, color);

			return baseFont;
		}

		public override iText.Kernel.Font.PdfFont GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
			bool cached)
		{
			//eventually replace expected font properties
			size = ReplaceSizeWithDefault
				? DefaultFontSize
				: size;
			style = ReplaceStyleWithDefault
				? DefaultFontStyle
				: style;
			encoding = ReplaceEncodingWithDefault
				? DefaultFontEncoding
				: encoding;
			embedded = ReplaceEmbeddingWithDefault
				? DefaultFontEmbedding
				: embedded;

			//get font
			PdfFont font = null;
			if (ReplaceFontWithDefault)
			{
				font = GetBaseFont(size, style, color);
			}
			else
			{
				font = FontFactory.GetFont(fontname, encoding, embedded, size, style, color, cached);

				if (font.BaseFont == null)
					font = GetBaseFont(size, style, color);
			}

			return font;
		}

		#endregion
	}
}
*/