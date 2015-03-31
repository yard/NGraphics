using NGraphics.Models.Operations;
using NGraphics.Parsers;

namespace NGraphics.Models.Segments
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(Point moveTo)
        {
            Start = moveTo;
            End = moveTo;
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.MoveTo(Start, End, false);
        }

        public override string ToString()
        {
            return "M" + Start.ToSvgString();
        }
    }
}