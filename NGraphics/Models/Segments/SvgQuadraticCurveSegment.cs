using NGraphics.Models.Elements;
using NGraphics.Parsers;

namespace NGraphics.Models.Segments
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        public SvgQuadraticCurveSegment(Point start, Point controlPoint, Point end)
        {
            Start = start;
            ControlPoint = controlPoint;
            End = end;
        }

        public Point ControlPoint { get; set; }

        private Point FirstControlPoint
        {
            get
            {
                var x1 = Start.X + (ControlPoint.X - Start.X)*2/3;
                var y1 = Start.Y + (ControlPoint.Y - Start.Y)*2/3;

                return new Point(x1, y1);
            }
        }

        private Point SecondControlPoint
        {
            get
            {
                var x2 = ControlPoint.X + (End.X - ControlPoint.X)/3;
                var y2 = ControlPoint.Y + (End.Y - ControlPoint.Y)/3;

                return new Point(x2, y2);
            }
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.CurveTo(Start, FirstControlPoint, SecondControlPoint, End);
        }

        public override string ToString()
        {
            return "Q" + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}