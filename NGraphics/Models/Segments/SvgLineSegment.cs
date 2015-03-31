using NGraphics.Parsers;

namespace NGraphics.Models.Segments
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.LineTo(Start, End);
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}