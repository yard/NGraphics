using NGraphics.Parsers;

namespace NGraphics.Models.Segments
{
    public sealed class SvgCubicCurveSegment : SvgPathSegment
    {
        private Point _firstControlPoint;
        private Point _secondControlPoint;

        public Point FirstControlPoint
        {
            get { return this._firstControlPoint; }
            set { this._firstControlPoint = value; }
        }

        public Point SecondControlPoint
        {
            get { return this._secondControlPoint; }
            set { this._secondControlPoint = value; }
        }

        public SvgCubicCurveSegment(Point start, Point firstControlPoint, Point secondControlPoint, Point end)
        {
            this.Start = start;
            this.End = end;
            this._firstControlPoint = firstControlPoint;
            this._secondControlPoint = secondControlPoint;
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.CurveTo(this.Start, this.FirstControlPoint, this.SecondControlPoint, this.End);
        }

        public override string ToString()
        {
            return "C" + this.FirstControlPoint.ToSvgString() + " " + this.SecondControlPoint.ToSvgString() + " " + this.End.ToSvgString();
        }
    }
}
