// 
// TextLayout.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
//       Lytico (http://limada.sourceforge.net)
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using Xwt.Backends;
using System.Collections.Generic;

namespace Xwt.Drawing
{
	public sealed class TextLayout: XwtObject, IDisposable
	{
		TextLayoutBackendHandler handler;

		Font font;
		string text;
		double width = -1;
		double height = -1;
		TextTrimming textTrimming;
		List<TextAttribute> attributes;

		public TextLayout ()
		{
			handler = ToolkitEngine.TextLayoutBackendHandler;
			Backend = handler.Create ();
			Font = Font.SystemFont;
		}

		public TextLayout (Canvas canvas)
		{
			ToolkitEngine = canvas.Surface.ToolkitEngine;
			handler = ToolkitEngine.TextLayoutBackendHandler;
			Backend = handler.Create ();
			Font = canvas.Font;
		}

		internal TextLayout (Toolkit tk)
		{
			ToolkitEngine = tk;
			handler = ToolkitEngine.TextLayoutBackendHandler;
			Backend = handler.Create ();
		}

		internal TextLayoutData GetData ()
		{
			return new TextLayoutData () {
				Width = width,
				Height = height,
				Text = text,
				Font = font,
				TextTrimming = textTrimming,
				Attributes = attributes != null ? new List<TextAttribute> (attributes) : null
			};
		}

		public Font Font {
			get { return font; }
			set { font = value; handler.SetFont (Backend, value); }
		}

		public string Text {
			get { return text; }
			set { text = value;
				handler.SetText (Backend, text); }
		}

		/// <summary>
		/// Gets or sets the desired width.
		/// </summary>
		/// <value>
		/// The width. A value of -1 uses GetSize().Width on drawings
		/// </value>
		public double Width {
			get { return width; }
			set { width = value; handler.SetWidth (Backend, value); }
		}

		/// <summary>
		/// Gets or sets desired Height.
		/// </summary>
		/// <value>
		/// The Height. A value of -1 uses GetSize().Height on drawings
		/// </value>
		public double Height {
			get { return this.height; }
			set { this.height = value; handler.SetHeight (Backend, value); }
		}

		/// <summary>
		/// measures the text
		/// if Width is other than -1, it measures the height according to Width
		/// Height is ignored
		/// </summary>
		/// <returns>
		/// The size.
		/// </returns>
		public Size GetSize ()
		{
			return handler.GetSize (Backend);
		}

		public TextTrimming Trimming {
			get { return textTrimming; }
			set { textTrimming = value; handler.SetTrimming (Backend, value); }
		}

		/// <summary>
		/// Converts from a X and Y position within the layout to the character at this position.
		/// </summary>
		/// <returns>The index of the character.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public int GetIndexFromCoordinates (double x, double y)
		{
			return handler.GetIndexFromCoordinates (Backend, x, y);
		}

		/// <summary>
		/// Converts from a Position within the layout to the character at this position.
		/// </summary>
		/// <returns>The index of the character.</returns>
		/// <param name="p">The position.</param>
		public int GetIndexFromCoordinates (Point p)
		{
			return handler.GetIndexFromCoordinates (Backend, p.X, p.Y);
		}

		/// <summary>
		/// Obtains the graphical coordinate of an character in the layout.
		/// </summary>
		/// <returns>The extends from the character at index.</returns>
		/// <param name="index">The index of the character.</param>
		public Point GetCoordinateFromIndex (int index)
		{
			return handler.GetCoordinateFromIndex (Backend, index);
		}

		List<TextAttribute> Attributes {
			get {
				if (attributes == null)
					attributes = new List<TextAttribute> ();
				return attributes;
			}
		}

		/// <summary>
		/// Sets the foreground color of a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="color">The color of the text.</param>
		/// <param name="startIndex">Start index of the first character to apply the foreground color to.</param>
		/// <param name="count">The number of characters to apply the foreground color to.</param>
		public void SetForeground (Color color, int startIndex, int count)
		{
			Attributes.Add (new ColorTextAttribute () { StartIndex = startIndex, Count = count, Color = color });
			handler.SetForeground (Backend, color, startIndex, count);
		}

		/// <summary>
		/// Sets the background color of a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="color">The color of the text background.</param>
		/// <param name="startIndex">Start index of the first character to apply the background color to.</param>
		/// <param name="count">The number of characters to apply the background color to.</param>
		public void SetBackgound (Color color, int startIndex, int count)
		{
			Attributes.Add (new BackgroundTextAttribute () { StartIndex = startIndex, Count = count, Color = color });
			handler.SetBackgound (Backend, color, startIndex, count);
		}

		/// <summary>
		/// Sets the font weight of a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="weight">The font weight of the text.</param>
		/// <param name="startIndex">Start index of the first character to apply the font weight to.</param>
		/// <param name="count">The number of characters to apply the font weight to.</param>
		public void SetFontWeight (FontWeight weight, int startIndex, int count)
		{
			Attributes.Add (new FontWeightTextAttribute () { StartIndex = startIndex, Count = count, Weight = weight });
			handler.SetFontWeight (Backend, weight, startIndex, count);
		}

		/// <summary>
		/// Sets the font style of a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="style">The font style of the text.</param>
		/// <param name="startIndex">Start index of the first character to apply the font style to.</param>
		/// <param name="count">The number of characters to apply the font style to.</param>
		public void SetFontStyle (FontStyle style, int startIndex, int count)
		{
			Attributes.Add (new FontStyleTextAttribute () { StartIndex = startIndex, Count = count, Style = style });
			handler.SetFontStyle (Backend, style, startIndex, count);
		}

		/// <summary>
		/// Underlines a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="startIndex">Start index of the first character to underline.</param>
		/// <param name="count">The number of characters to underline.</param>
		public void SetUnderline (int startIndex, int count)
		{
			Attributes.Add (new UnderlineTextAttribute () { StartIndex = startIndex, Count = count});
			handler.SetUnderline (Backend, startIndex, count);
		}

		/// <summary>
		/// Adds a strike-through to a part of text inside the <see cref="T:Xwt.Drawing.TextLayout"/> object.
		/// </summary>
		/// <param name="startIndex">Start index of the first character to strike-through.</param>
		/// <param name="count">The number of characters to strike-through.</param>
		public void SetStrikethrough (int startIndex, int count)
		{
			Attributes.Add (new StrikethroughTextAttribute () { StartIndex = startIndex, Count = count});
			handler.SetStrikethrough (Backend, startIndex, count);
		}

		public void Dispose ()
		{
			handler.DisposeBackend (Backend);
		}
	}

	public enum TextTrimming {
		Word,
		WordElipsis
	}
	
	class TextLayoutData
	{
		public double Width = -1;
		public double Height = -1;
		public string Text;
		public Font Font;
		public TextTrimming TextTrimming;
		public List<TextAttribute> Attributes;

		public void InitLayout (TextLayout la)
		{
			if (Width != -1)
				la.Width = Width;
			if (Height != -1)
				la.Height = Height;
			if (Text != null)
				la.Text = Text;
			if (Font != null)
				la.Font = Font;
			if (TextTrimming != default(TextTrimming))
				la.Trimming = TextTrimming;
			if (Attributes != null) {
				foreach (var at in Attributes)
					at.Apply (la);
			}
		}

		public bool Equals (TextLayoutData other)
		{
			if (Width != other.Width || Height != other.Height || Text != other.Text || Font != other.Font || TextTrimming != other.TextTrimming)
				return false;
			if (Attributes == null && other.Attributes == null)
				return true;
			if (Attributes != null || other.Attributes != null)
				return false;
			if (Attributes.Count != other.Attributes.Count)
				return false;
			for (int n=0; n<Attributes.Count; n++)
				if (!Attributes [n].Equals (other.Attributes [n]))
					return false;
			return true;
		}
	}

	public abstract class TextAttribute
	{
		public int StartIndex { get; set; }
		public int Count { get; set; }

		internal abstract void Apply (TextLayout la);

		public virtual bool Equals (TextAttribute t)
		{
			return t.StartIndex == StartIndex && t.Count == Count;
		}
	}

	public class BackgroundTextAttribute: TextAttribute
	{
		public Color Color { get; set; }
		
		internal override void Apply (TextLayout la)
		{
			la.SetBackgound (Color, StartIndex, Count);
		}
		
		public override bool Equals (TextAttribute t)
		{
			var ot = t as BackgroundTextAttribute;
			return ot != null && Color.Equals (ot.Color) && base.Equals (t);
		}
	}
	
	public class FontWeightTextAttribute: TextAttribute
	{
		public FontWeight Weight { get; set; }
		
		internal override void Apply (TextLayout la)
		{
			la.SetFontWeight (Weight, StartIndex, Count);
		}
		
		public override bool Equals (TextAttribute t)
		{
			var ot = t as FontWeightTextAttribute;
			return ot != null && Weight.Equals (ot.Weight) && base.Equals (t);
		}
	}
	
	public class FontStyleTextAttribute: TextAttribute
	{
		public FontStyle Style { get; set; }
		
		internal override void Apply (TextLayout la)
		{
			la.SetFontStyle (Style, StartIndex, Count);
		}
		
		public override bool Equals (TextAttribute t)
		{
			var ot = t as FontStyleTextAttribute;
			return ot != null && Style.Equals (ot.Style) && base.Equals (t);
		}
	}
	
	public class UnderlineTextAttribute: TextAttribute
	{
		internal override void Apply (TextLayout la)
		{
			la.SetUnderline (StartIndex, Count);
		}
		
		public override bool Equals (TextAttribute t)
		{
			var ot = t as UnderlineTextAttribute;
			return ot != null && base.Equals (t);
		}
	}
	
	public class StrikethroughTextAttribute: TextAttribute
	{
		internal override void Apply (TextLayout la)
		{
			la.SetStrikethrough (StartIndex, Count);
		}
		
		public override bool Equals (TextAttribute t)
		{
			var ot = t as StrikethroughTextAttribute;
			return ot != null && base.Equals (t);
		}
	}

	public class FontTextAttribute: TextAttribute
	{
		public Font Font { get; set; }

		internal override void Apply (TextLayout la)
		{
			throw new NotImplementedException ();
		}

		public override bool Equals (TextAttribute t)
		{
			var ot = t as FontTextAttribute;
			return ot != null && Font.Equals (ot.Font) && base.Equals (t);
		}
	}
	
	public class FontSizeTextAttribute: TextAttribute
	{
		public double Size { get; set; }

		internal override void Apply (TextLayout la)
		{
			throw new NotImplementedException ();
		}

		public override bool Equals (TextAttribute t)
		{
			var ot = t as FontSizeTextAttribute;
			return ot != null && Size == ot.Size && base.Equals (t);
		}
	}
	
	public class ColorTextAttribute: TextAttribute
	{
		public Color Color { get; set; }

		internal override void Apply (TextLayout la)
		{
			throw new NotImplementedException ();
		}

		public override bool Equals (TextAttribute t)
		{
			var ot = t as ColorTextAttribute;
			return ot != null && Color == ot.Color && base.Equals (t);
		}
	}
}

