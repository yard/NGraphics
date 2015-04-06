﻿using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Text;
using System.IO;
using System.Threading.Tasks;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Brushes;
using NGraphics.Models.Operations;
using NGraphics.Models.Transforms;
using Point = NGraphics.Models.Point;
using Rect = NGraphics.Models.Rect;

namespace NGraphics
{
    public class AndroidPlatform : IPlatform
    {
        public string Name { get { return "Android"; } }

        public IImageCanvas CreateImageCanvas(Size size, double scale = 1.0, bool transparency = true)
        {
            var pixelWidth = (int)Math.Ceiling(size.Width * scale);
            var pixelHeight = (int)Math.Ceiling(size.Height * scale);
            var bitmap = Bitmap.CreateBitmap(pixelWidth, pixelHeight, Bitmap.Config.Argb8888);
            if (!transparency)
            {
                bitmap.EraseColor(Colors.Black.Argb);
            }
            return new BitmapCanvas(bitmap, scale);
        }

        public IImage LoadImage(Stream stream)
        {
            var bitmap = BitmapFactory.DecodeStream(stream);
            return new BitmapImage(bitmap);
        }

        public IImage LoadImage(string path)
        {
            var bitmap = BitmapFactory.DecodeFile(path);
            return new BitmapImage(bitmap);
        }

        public IImage CreateImage(Models.Color[] colors, int width, double scale = 1.0)
        {
            var pixelWidth = width;
            var pixelHeight = colors.Length / width;
            var acolors = new int[pixelWidth * pixelHeight];
            for (var i = 0; i < colors.Length; i++)
            {
                acolors[i] = colors[i].Argb;
            }
            var bitmap = Bitmap.CreateBitmap(acolors, pixelWidth, pixelHeight, Bitmap.Config.Argb8888);
            return new BitmapImage(bitmap, scale);
        }
    }

    public class BitmapImage : IImage
    {
        readonly Bitmap bitmap;
        //		readonly double scale;

        public Bitmap Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        public BitmapImage(Bitmap bitmap, double scale = 1.0)
        {
            this.bitmap = bitmap;
            //			this.scale = scale;
        }

        public Size Size
        {
            get { throw new NotImplementedException(); }
        }

        public double Scale
        {
            get { throw new NotImplementedException(); }
        }

        public void SaveAsPng(string path)
        {
            using (var f = System.IO.File.OpenWrite(path))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, f);
            }
        }

        public void SaveAsPng(Stream stream)
        {
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }
    }

    public class BitmapCanvas : CanvasCanvas, IImageCanvas
    {
        readonly Bitmap bitmap;
        readonly double scale;

        public Size Size { get { return new Size(bitmap.Width / scale, bitmap.Height / scale); } }
        public double Scale { get { return scale; } }

        public BitmapCanvas(Bitmap bitmap, double scale = 1.0)
            : base(new Canvas(bitmap))
        {
            this.bitmap = bitmap;
            this.scale = scale;

            graphics.Scale((float)scale, (float)scale);
        }

        public IImage GetImage()
        {
            return new BitmapImage(bitmap, scale);
        }
    }

    public class CanvasCanvas : ICanvas
    {
        protected readonly Canvas graphics;

        public CanvasCanvas(Canvas graphics)
        {
            this.graphics = graphics;
        }

        public void SaveState()
        {
            graphics.Save(SaveFlags.Matrix | SaveFlags.Clip);
        }
        public void Transform(Transform transform)
        {
            var t = new Matrix();
            t.SetValues(new[] {
				(float)transform.A, (float)transform.C, (float)transform.E,
				(float)transform.B, (float)transform.D, (float)transform.F,
				0, 0, 1,
			});
            t.PostConcat(graphics.Matrix);
            graphics.Matrix = t;
        }
        public void RestoreState()
        {
            graphics.Restore();
        }

        TextPaint GetFontPaint(Font font, TextAlignment alignment)
        {
            var paint = new TextPaint(PaintFlags.AntiAlias);
            paint.TextAlign = Paint.Align.Left;
            if (alignment == TextAlignment.Center)
                paint.TextAlign = Paint.Align.Left;
            else if (alignment == TextAlignment.Right)
                paint.TextAlign = Paint.Align.Right;

            paint.TextSize = (float)font.Size;
            var typeface = Typeface.Create(font.Family, TypefaceStyle.Normal);
            paint.SetTypeface(typeface);

            return paint;
        }
        Paint GetImagePaint(double alpha)
        {
            var paint = new Paint(PaintFlags.AntiAlias);
            paint.FilterBitmap = true;
            paint.Alpha = (int)(alpha * 255);
            return paint;
        }
        Paint GetPenPaint(Pen pen)
        {
            var paint = new Paint(PaintFlags.AntiAlias);
            paint.SetStyle(Paint.Style.Stroke);
            paint.SetARGB(pen.Color.A, pen.Color.R, pen.Color.G, pen.Color.B);
            paint.StrokeWidth = (float)pen.Width;
            return paint;
        }
        Paint GetBrushPaint(BaseBrush brush, Rect frame)
        {
            var paint = new Paint(PaintFlags.AntiAlias);
            AddBrushPaint(paint, brush, frame);
            return paint;
        }
        void AddBrushPaint(Paint paint, BaseBrush brush, Rect bb)
        {
            paint.SetStyle(Paint.Style.Fill);

            var sb = brush as SolidBrush;
            if (sb != null)
            {
                paint.SetARGB(sb.Color.A, sb.Color.R, sb.Color.G, sb.Color.B);
                return;
            }

            var lgb = brush as LinearGradientBrush;
            if (lgb != null)
            {
                var n = lgb.Stops.Count;
                if (n >= 2)
                {
                    var locs = new float[n];
                    var comps = new int[n];
                    for (var i = 0; i < n; i++)
                    {
                        var s = lgb.Stops[i];
                        locs[i] = (float)s.Offset;
                        comps[i] = s.Color.Argb;
                    }
                    var p1 = lgb.Absolute ? lgb.Start : bb.Position + lgb.Start * bb.Size;
                    var p2 = lgb.Absolute ? lgb.End : bb.Position + lgb.End * bb.Size;
                    var lg = new LinearGradient(
                                (float)p1.X, (float)p1.Y,
                                (float)p2.X, (float)p2.Y,
                                comps,
                                locs,
                                Shader.TileMode.Clamp);
                    paint.SetShader(lg);
                }
                return;
            }

            var rgb = brush as RadialGradientBrush;
            if (rgb != null)
            {
                var n = rgb.Stops.Count;
                if (n >= 2)
                {
                    var locs = new float[n];
                    var comps = new int[n];
                    for (var i = 0; i < n; i++)
                    {
                        var s = rgb.Stops[i];
                        locs[i] = (float)s.Offset;
                        comps[i] = s.Color.Argb;
                    }
                    var p1 = rgb.GetAbsoluteCenter(bb);
                    var r = rgb.GetAbsoluteRadius(bb);
                    var rg = new RadialGradient(
                                (float)p1.X, (float)p1.Y,
                                (float)r.Max,
                                comps,
                                locs,
                                Shader.TileMode.Clamp);

                    paint.SetShader(rg);
                }
                return;
            }

            throw new NotSupportedException("Brush " + brush);
        }

        public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, BaseBrush brush = null)
        {
            if (brush == null)
                return;

            var paint = GetFontPaint(font, alignment);
            var w = paint.MeasureText(text);
            var fm = paint.GetFontMetrics();
            var h = fm.Ascent + fm.Descent;
            var point = frame.Position;
            var fr = new Rect(point, new Size(w, h));
            AddBrushPaint(paint, brush, fr);
            graphics.DrawText(text, (float)point.X, (float)point.Y, paint);
        }
        public void DrawPath(IEnumerable<PathOperation> ops, Pen pen = null, BaseBrush brush = null)
        {
            if (pen == null && brush == null)
                return;

            using (var path = new global::Android.Graphics.Path())
            {

                var bb = new BoundingBoxBuilder();

                foreach (var op in ops)
                {
                    var mt = op as MoveTo;
                    if (mt != null)
                    {
                        var p = mt.Start;
                        path.MoveTo((float)p.X, (float)p.Y);
                        bb.Add(p);
                        continue;
                    }
                    var lt = op as LineTo;
                    if (lt != null)
                    {
                        var p = lt.Start;
                        path.LineTo((float)p.X, (float)p.Y);
                        bb.Add(p);
                        continue;
                    }
                    var at = op as ArcTo;
                    if (at != null)
                    {
                        var p = at.Point;
                        path.LineTo((float)p.X, (float)p.Y);
                        bb.Add(p);
                        continue;
                    }
                    var ct = op as CurveTo;
                    if (ct != null)
                    {
                        var p = ct.Start;
                        var c1 = ct.FirstControlPoint;
                        var c2 = ct.SecondControlPoint;
                        path.CubicTo((float)c1.X, (float)c1.Y, (float)c2.X, (float)c2.Y, (float)p.X, (float)p.Y);
                        bb.Add(p);
                        bb.Add(c1);
                        bb.Add(c2);
                        continue;
                    }
                    var cp = op as ClosePath;
                    if (cp != null)
                    {
                        path.Close();
                        continue;
                    }

                    throw new NotSupportedException("Path Op " + op);
                }

                var frame = bb.BoundingBox;

                if (brush != null)
                {
                    var paint = GetBrushPaint(brush, frame);
                    graphics.DrawPath(path, paint);
                }
                if (pen != null)
                {
                    var paint = GetPenPaint(pen);
                    graphics.DrawPath(path, paint);
                }
            }
        }
        public void DrawRectangle(Rect frame, Pen pen = null, BaseBrush brush = null)
        {
            if (brush != null)
            {
                var paint = GetBrushPaint(brush, frame);
                graphics.DrawRect((float)(frame.X), (float)(frame.Y), (float)(frame.X + frame.Width), (float)(frame.Y + frame.Height), paint);
            }
            if (pen != null)
            {
                var paint = GetPenPaint(pen);
                graphics.DrawRect((float)(frame.X), (float)(frame.Y), (float)(frame.X + frame.Width), (float)(frame.Y + frame.Height), paint);
            }

        }
        public void DrawEllipse(Rect frame, Pen pen = null, BaseBrush brush = null)
        {
            if (brush != null)
            {
                var paint = GetBrushPaint(brush, frame);
                graphics.DrawOval(Conversions.GetRectF(frame), paint);
            }
            if (pen != null)
            {
                var paint = GetPenPaint(pen);
                graphics.DrawOval(Conversions.GetRectF(frame), paint);
            }
        }
        public void DrawImage(IImage image, Rect frame, double alpha = 1.0)
        {
            var ii = image as BitmapImage;
            if (ii != null)
            {
                var paint = GetImagePaint(alpha);
                var isize = new Size(ii.Bitmap.Width, ii.Bitmap.Height);
                var scale = frame.Size / isize;
                var m = new Matrix();
                m.PreTranslate((float)frame.X, (float)frame.Y);
                m.PreScale((float)scale.Width, (float)scale.Height);
                graphics.DrawBitmap(ii.Bitmap, m, paint);
            }
        }
    }

    public static class Conversions
    {
        public static PointF GetPointF(this Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }

        public static RectF GetRectF(this Rect frame)
        {
            return new RectF((float)frame.X, (float)frame.Y, (float)(frame.X + frame.Width), (float)(frame.Y + frame.Height));
        }
    }
}

