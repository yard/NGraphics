using NGraphics.Models.Operations;
using NGraphics.Parsers;

namespace NGraphics.Models.Segments
{
    public sealed class SvgCubicCurveSegment : SvgPathSegment
    {
        public SvgCubicCurveSegment(Point start, Point firstControlPoint, Point secondControlPoint, Point end)
        {
            Start = start;
            End = end;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

        public Point FirstControlPoint { get; set; }
        public Point SecondControlPoint { get; set; }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.CurveTo(Start, FirstControlPoint, SecondControlPoint, End);
        }

        public override string ToString()
        {
            return "C" + FirstControlPoint.ToSvgString() + " " + SecondControlPoint.ToSvgString() + " " +
                   End.ToSvgString();
        }
    }
}