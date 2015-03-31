using System;
using System.Collections.Generic;
using System.Globalization;
using NGraphics.Interfaces;
using NGraphics.Models.Brushes;
using NGraphics.Models.Operations;

namespace NGraphics.Models.Elements
{
    public class Path : Element
    {
        public Path(IEnumerable<PathOperation> operations, Pen pen = null, BaseBrush baseBrush = null)
            : base(pen, baseBrush)
        {
            Operations.AddRange(operations);
        }

        public Path(Pen pen = null, BaseBrush baseBrush = null)
            : base(pen, baseBrush)
        {
        }

        public readonly List<PathOperation> Operations = new List<PathOperation>();

        protected override void DrawElement(ICanvas canvas)
        {
            canvas.DrawPath(Operations, Pen, BaseBrush);
        }

        private void Add(PathOperation operation)
        {
            Operations.Add(operation);
        }

        public void MoveTo(Point point, bool isAbsolute)
        {
            Add(new MoveTo(point, isAbsolute));
        }

        public void MoveTo(double x, double y, bool isAbsolute)
        {
            Add(new MoveTo(x, y, isAbsolute));
        }

        public void MoveTo(Point start, Point end, bool isAbsolute)
        {
            Add(new MoveTo(start, end, isAbsolute));
        }

        public void LineTo(Point point)
        {
            Add(new LineTo(point));
        }

        public void LineTo(Point start, Point end)
        {
            Add(new LineTo(start, end));
        }

        public void LineTo(double x, double y, bool isAbsolute)
        {
            Add(new LineTo(x, y, isAbsolute));
        }

        public void ArcTo(Size radius, bool largeArc, bool sweepClockwise, Point point)
        {
            Add(new ArcTo(radius, largeArc, sweepClockwise, point));
        }

        public void CurveTo(Point control1, Point control2, Point point)
        {
            Add(new CurveTo(control1, control2, point));
        }

        public void CurveTo(Point control1, Point control2, Point point, Point point2)
        {
            Add(new CurveTo(control1, control2, point, point2));
        }

        public void Start()
        {
            Add(new StartFigure());
        }

        public void Close()
        {
            Add(new ClosePath());
        }

        public bool Contains(Point point)
        {
            var verts = new List<Point>();
            foreach (var o in Operations)
            {
                var mo = o as MoveTo;
                if (mo != null)
                {
                    verts.Add(mo.Start);
                    continue;
                }
                var lt = o as LineTo;
                if (lt != null)
                {
                    verts.Add(lt.Start);
                    continue;
                }
                var cp = o as ClosePath;
                if (cp != null)
                {
                    continue;
                }
                throw new NotSupportedException("Contains does not support " + o);
            }
            int i, j;
            var c = false;
            var nverts = verts.Count;
            var testx = point.X;
            var testy = point.Y;
            for (i = 0, j = nverts - 1; i < nverts; j = i++)
            {
                if (((verts[i].Y > testy) != (verts[j].Y > testy)) &&
                    (testx < (verts[j].X - verts[i].X)*(testy - verts[i].Y)/(verts[j].Y - verts[i].Y) + verts[i].X))
                    c = !c;
            }
            return c;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Path ([{0}])", Operations.Count);
        }
    }
}