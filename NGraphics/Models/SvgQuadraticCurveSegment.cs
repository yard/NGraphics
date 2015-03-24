using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGraphics.Models
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        private Point _controlPoint;

        public Point ControlPoint
        {
            get { return this._controlPoint; }
            set { this._controlPoint = value; }
        }

        private Point FirstControlPoint
        {
            get
            {
                double x1 = Start.X + (this.ControlPoint.X - Start.X) * 2 / 3;
                double y1 = Start.Y + (this.ControlPoint.Y - Start.Y) * 2 / 3;

                return new Point(x1, y1);
            }
        }

        private Point SecondControlPoint
        {
            get
            {
                double x2 = this.ControlPoint.X + (this.End.X - this.ControlPoint.X) / 3;
                double y2 = this.ControlPoint.Y + (this.End.Y - this.ControlPoint.Y) / 3;

                return new Point(x2, y2);
            }
        }

        public SvgQuadraticCurveSegment(Point start, Point controlPoint, Point end)
        {
            this.Start = start;
            this._controlPoint = controlPoint;
            this.End = end;
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.CurveTo(this.Start, this.FirstControlPoint, this.SecondControlPoint, this.End);
        }

        public override string ToString()
        {
            return "Q" + this.ControlPoint.ToSvgString() + " " + this.End.ToSvgString();
        }

    }
}
