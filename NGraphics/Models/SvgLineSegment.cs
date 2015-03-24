namespace NGraphics.Models
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        public override void AddToPath(Path graphicsPath)
        {
            graphicsPath.LineTo(this.Start, this.End);
        }

        public override string ToString()
        {
            return "L" + this.End.ToSvgString();
        }

    }
}
