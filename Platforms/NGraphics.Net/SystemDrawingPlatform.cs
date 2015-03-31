using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Operations;
using BoundingBoxBuilder = NGraphics.Models.BoundingBoxBuilder;
using Font = NGraphics.Models.Font;
using GradientStop = NGraphics.Models.GradientStop;
using LinearGradientBrush = NGraphics.Models.LinearGradientBrush;
using Pen = NGraphics.Models.Pen;
using Point = NGraphics.Models.Point;
using Rect = NGraphics.Models.Rect;
using Size = NGraphics.Models.Size;
using SolidBrush = NGraphics.Models.SolidBrush;
using TextAlignment = NGraphics.Models.TextAlignment;

namespace NGraphics.Net
{
	public class SystemDrawingPlatform : IPlatform
	{
		public string Name { get { return "Net"; } }

		public IImageCanvas CreateImageCanvas (Models.Size size, double scale = 1.0, bool transparency = true)
		{
			var pixelWidth = (int)Math.Ceiling (size.Width * scale);
			var pixelHeight = (int)Math.Ceiling (size.Height * scale);
			var format = transparency ? PixelFormat.Format32bppPArgb : PixelFormat.Format24bppRgb;
			var bitmap = new Bitmap (pixelWidth, pixelHeight, format);
			return new BitmapCanvas (bitmap, scale);
		}

		public IImage LoadImage (Stream stream)
		{
			var image = Image.FromStream (stream);
			return new ImageImage (image);
		}

		public IImage LoadImage (string path)
		{
			var image = Image.FromFile (path);
			return new ImageImage (image);
		}

		public IImage CreateImage (Models.Color[] colors, int width, double scale = 1.0)
		{
			var pixelWidth = width;
			var pixelHeight = colors.Length / width;
			var format = PixelFormat.Format32bppArgb;
			Bitmap bitmap;
			unsafe {
				fixed (Models.Color *c = colors) {
					bitmap = new Bitmap (pixelWidth, pixelHeight, pixelWidth*4, format, new IntPtr (c));
				}
			}
			return new ImageImage (bitmap);
		}
	}

	public class ImageImage : IImage
	{
		readonly Image image;

		public Image Image {
			get {
				return image;
			}
		}

		public ImageImage (Image image)
		{
			this.image = image;
		}

		public void SaveAsPng (string path)
		{
			image.Save (path, ImageFormat.Png);
		}
	}

	public class BitmapCanvas : GraphicsCanvas, IImageCanvas
	{
		readonly Bitmap bitmap;
		readonly double scale;

		public BitmapCanvas (Bitmap bitmap, double scale = 1.0)
			: base (Graphics.FromImage (bitmap))
		{
			this.bitmap = bitmap;
			this.scale = scale;

			graphics.ScaleTransform ((float)scale, (float)scale);
		}

		public IImage GetImage ()
		{
			return new ImageImage (bitmap);
		}
	}

	public class GraphicsCanvas : ICanvas
	{
		protected readonly Graphics graphics;
		readonly Stack<GraphicsState> stateStack = new Stack<GraphicsState> ();

		public GraphicsCanvas (Graphics graphics)
		{
			this.graphics = graphics;

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
		}

		public void SaveState ()
		{
			var s = graphics.Save ();
			stateStack.Push (s);
		}
		public void Transform (Transform transform)
		{
			var t = transform;
			var stack = new Stack<Transform> ();
			while (t != null) {
				stack.Push (t);
				t = t.Previous;
			}
			while (stack.Count > 0) {
				t = stack.Pop ();

				var rt = t as Rotate;
				if (rt != null) {
					graphics.RotateTransform ((float)rt.Angle);
					t = t.Previous;
					continue;
				}
				var tt = t as Translate;
				if (tt != null) {
					graphics.TranslateTransform ((float)tt.Size.Width, (float)tt.Size.Height);
					t = t.Previous;
					continue;
				}
                var st = t as Scale;
                if (st != null) {
                    graphics.ScaleTransform ((float)st.Size.Width, (float)st.Size.Height);
                    t = t.Previous;
                    continue;
                }
                throw new NotSupportedException ("Transform " + t);
			}
		}
		public void RestoreState ()
		{
			if (stateStack.Count > 0) {
				var s = stateStack.Pop ();
				graphics.Restore (s);
			}
		}

		public void DrawText (string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, BaseBrush baseBrush = null)
		{
			if (baseBrush == null)
				return;
			var sdfont = new System.Drawing.Font (font.Family, (float)font.Size, FontStyle.Regular, GraphicsUnit.Pixel);
			var sz = graphics.MeasureString (text, sdfont);
			var point = frame.Position;
            var fr = new Rect (point, new Size (sz.Width, sz.Height));
            graphics.DrawString (text, sdfont, Conversions.GetBrush (baseBrush, fr), Conversions.GetPointF (point - new Point (0, sdfont.Height)));
		}
		public void DrawPath (IEnumerable<PathOperation> ops, Pen pen = null, BaseBrush baseBrush = null)
		{
			using (var path = new GraphicsPath ()) {

                var bb = new BoundingBoxBuilder ();

				var position = Point.Zero;

				foreach (var op in ops)
				{
				    var start = op as StartFigure;

				    if (start != null)
				    {
				        path.StartFigure();
                        continue;
				    }

					var mt = op as MoveTo;
					if (mt != null) {
						var p = mt.Start;
						position = p;
                        bb.Add (p);
						continue;
					}
					var lt = op as LineTo;
					if (lt != null) {
						var p = lt.Start;
						path.AddLine (Conversions.GetPointF (lt.Start), Conversions.GetPointF (lt.End));
						position = p;
                        bb.Add (p);
                        continue;
					}
                    var at = op as ArcTo;
                    if (at != null) {
                        var p = at.Point;
                        path.AddLine (Conversions.GetPointF (position), Conversions.GetPointF (p));
                        position = p;
                        bb.Add (p);
                        continue;
                    }
                    var ct = op as CurveTo;
                    if (ct != null) {
                      
                        path.AddBezier (Conversions.GetPointF (ct.Start), Conversions.GetPointF (ct.FirstControlPoint),
                            Conversions.GetPointF (ct.SecondControlPoint), Conversions.GetPointF (ct.End));
                        bb.Add(ct.Start);
                        bb.Add(ct.FirstControlPoint);
                        bb.Add(ct.SecondControlPoint);
                        bb.Add(ct.End);
                        continue;
                    }
                    var cp = op as ClosePath;
					if (cp != null) {
						path.CloseFigure ();
						continue;
					}

					throw new NotSupportedException ("Path Op " + op);
				}

                var frame = bb.BoundingBox;
                if (baseBrush != null)
                {
                    graphics.FillPath(baseBrush.GetBrush(frame), path);
                }
                if (pen != null)
                {
                    var r = Conversions.GetRectangleF(frame);
                    graphics.DrawPath(pen.GetPen(), path);
                }
			}
		}
		public void DrawRectangle (Rect frame, Pen pen = null, BaseBrush baseBrush = null)
		{
			if (baseBrush != null) {
				graphics.FillRectangle (baseBrush.GetBrush (frame), Conversions.GetRectangleF (frame));
			}
			if (pen != null) {
				var r = Conversions.GetRectangleF (frame);
				graphics.DrawRectangle (pen.GetPen (), r.X, r.Y, r.Width, r.Height);
			}
		}
		public void DrawEllipse (Rect frame, Pen pen = null, BaseBrush baseBrush = null)
		{
			if (baseBrush != null) {
				graphics.FillEllipse (baseBrush.GetBrush (frame), Conversions.GetRectangleF (frame));
			}
			if (pen != null) {
				graphics.DrawEllipse (pen.GetPen (), Conversions.GetRectangleF (frame));
			}
		}
		public void DrawImage (IImage image, Rect frame)
		{
			var ii = image as ImageImage;
			if (ii != null) {
                graphics.DrawImage (ii.Image, Conversions.GetRectangleF (frame));
			}
		}
	}

	public static class Conversions
	{
		public static System.Drawing.Color GetColor (this Models.Color color)
		{
			return System.Drawing.Color.FromArgb (color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Pen GetPen (this Pen pen)
		{
			return new System.Drawing.Pen (GetColor (pen.Color), (float)pen.Width);
		}

        static ColorBlend BuildBlend (List<GradientStop> stops, bool reverse = false)
        {
            if (stops.Count < 2)
                return null;

            var s1 = stops[0];
            var s2 = stops[stops.Count - 1];

            // Build the blend
            var n = stops.Count;
            var an = 0;
            if (s1.Offset != 0) {
                an++;
            }
            if (s2.Offset != 1) {
                an++;
            }
            var blend = new System.Drawing.Drawing2D.ColorBlend (n + an);
            var o = 0;
            if (s1.Offset != 0) {
                blend.Colors[0] = GetColor (s1.Color);
                blend.Positions[0] = 0;
                o = 1;
            }
            for (var i = 0; i < n; i++) {
                blend.Colors[i + o] = GetColor (stops[i].Color);
                blend.Positions[i + o] = (float)stops[i].Offset;
            }
            if (s2.Offset != 1) {
                blend.Colors[n + an - 1] = GetColor (s1.Color);
                blend.Positions[n + an - 1] = 1;
            }

            if (reverse) {
                for (var i = 0; i < blend.Positions.Length; i++) {
                    blend.Positions[i] = 1 - blend.Positions[i];
                }
                Array.Reverse (blend.Positions);
                Array.Reverse (blend.Colors);
            }

            return blend;
        }

		public static System.Drawing.Brush GetBrush (this BaseBrush baseBrush, Rect frame)
		{
			var cb = baseBrush as SolidBrush;
			if (cb != null) {
				return new System.Drawing.SolidBrush (cb.Color.GetColor ());
			}

            var lgb = baseBrush as LinearGradientBrush;
            if (lgb != null) {
                var s = frame.Position + lgb.RelativeStart * frame.Size;
                var e = frame.Position + lgb.RelativeEnd * frame.Size;
                var b = new System.Drawing.Drawing2D.LinearGradientBrush (GetPointF (s), GetPointF (e), System.Drawing.Color.Black, System.Drawing.Color.Black);
                var bb = BuildBlend (lgb.Stops);
                if (bb != null) {
                    b.InterpolationColors = bb;
                }
                return b;
            }

            var rgb = baseBrush as RadialGradientBrush;
            if (rgb != null) {
                var r = rgb.RelativeRadius * frame.Size.Max;
                var c = frame.Position + rgb.RelativeCenter * frame.Size;
                var path = new GraphicsPath ();
                path.AddEllipse (GetRectangleF (new Rect (c - r, new Size (2*r))));
                var b = new PathGradientBrush (path);
                var bb = BuildBlend (rgb.Stops, true);
                if (bb != null) {
                    b.InterpolationColors = bb;
                }
                return b;
            }

			throw new NotImplementedException ("Brush " + baseBrush);
		}

        public static PointF GetPointF (Point point)
        {
            return new PointF ((float)point.X, (float)point.Y);
        }

		public static RectangleF GetRectangleF (Rect frame)
		{
			return new RectangleF ((float)frame.X, (float)frame.Y, (float)frame.Width, (float)frame.Height);
		}
	}
}

