using NGraphics.Models.Elements;

namespace NGraphics.Models.Segments
{
    public abstract class SvgPathSegment
    {
        protected SvgPathSegment()
        {
        }

        protected SvgPathSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Point Start { get; set; }
        public Point End { get; set; }
        public abstract void AddToPath(Path graphicsPath);

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }
    }
}