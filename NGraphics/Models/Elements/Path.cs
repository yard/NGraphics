using System;
using System.Collections.Generic;
using System.Globalization;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;
using NGraphics.Custom.Models.Operations;
using System.Linq;

namespace NGraphics.Custom.Models.Elements {
	
	/// <summary>
	/// Path.
	/// </summary>
    public class Path : Element {
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Path"/> class.
		/// </summary>
		/// <param name="operations">Operations.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Path(IEnumerable<PathOperation> operations, Pen pen = null, BaseBrush baseBrush = null) : base(pen, baseBrush) {
            Operations.AddRange(operations);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Path"/> class.
		/// </summary>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Path(Pen pen = null, BaseBrush baseBrush = null) : base(pen, baseBrush) {
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override IDrawable Clone() {
			return new Path(Operations.Select(op => op.Clone()), Pen.Clone(), Brush.Clone()) {
				Id = this.Id,
				Transform = this.Transform
			};
		}

		/// <summary>
		/// The operations.
		/// </summary>
        public readonly List<PathOperation> Operations = new List<PathOperation>();

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected override void DrawElement(ICanvas canvas) {
            canvas.DrawPath(Operations, Pen, Brush);
        }

		/// <summary>
		/// Add the specified operation.
		/// </summary>
		/// <param name="operation">Operation.</param>
        private void Add(PathOperation operation) {
            Operations.Add(operation);
        }

		/// <summary>
		/// Moves to.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public void MoveTo(Point point, bool isAbsolute) {
            Add(new MoveTo(point, isAbsolute));
        }

		/// <summary>
		/// Moves to.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public void MoveTo(double x, double y, bool isAbsolute) {
            Add(new MoveTo(x, y, isAbsolute));
        }

		/// <summary>
		/// Moves to.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public void MoveTo(Point start, Point end, bool isAbsolute) {
            Add(new MoveTo(start, end, isAbsolute));
        }

		/// <summary>
		/// Lines to.
		/// </summary>
		/// <param name="point">Point.</param>
        public void LineTo(Point point) {
            Add(new LineTo(point));
        }

		/// <summary>
		/// Lines to.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
        public void LineTo(Point start, Point end) {
            Add(new LineTo(start, end));
        }

		/// <summary>
		/// Lines to.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public void LineTo(double x, double y, bool isAbsolute) {
            Add(new LineTo(x, y, isAbsolute));
        }

		/// <summary>
		/// Arcs to.
		/// </summary>
		/// <param name="radius">Radius.</param>
		/// <param name="largeArc">If set to <c>true</c> large arc.</param>
		/// <param name="sweepClockwise">If set to <c>true</c> sweep clockwise.</param>
		/// <param name="point">Point.</param>
        public void ArcTo(Size radius, bool largeArc, bool sweepClockwise, Point point) {
            Add(new ArcTo(radius, largeArc, sweepClockwise, point));
        }

		/// <summary>
		/// Curves to.
		/// </summary>
		/// <param name="control1">Control1.</param>
		/// <param name="control2">Control2.</param>
		/// <param name="point">Point.</param>
        public void CurveTo(Point control1, Point control2, Point point) {
            Add(new CurveTo(control1, control2, point));
        }

		/// <summary>
		/// Curves to.
		/// </summary>
		/// <param name="control1">Control1.</param>
		/// <param name="control2">Control2.</param>
		/// <param name="point">Point.</param>
		/// <param name="point2">Point2.</param>
        public void CurveTo(Point control1, Point control2, Point point, Point point2) {
            Add(new CurveTo(control1, control2, point, point2));
        }

		/// <summary>
		/// Start this instance.
		/// </summary>
        public void Start() {
            Add(new StartFigure());
        }

		/// <summary>
		/// Close this instance.
		/// </summary>
        public void Close() {
            Add(new ClosePath());
        }

		/// <summary>
		/// Contains the specified point.
		/// </summary>
		/// <param name="point">Point.</param>
        public bool Contains(Point point) {
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

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Path"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Path"/>.</returns>
        public override string ToString() {
            return string.Format(CultureInfo.InvariantCulture, "Path ([{0}])", Operations.Count);
        }
    }
}