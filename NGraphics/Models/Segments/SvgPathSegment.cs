namespace NGraphics.Models.Segments
{
    public abstract class SvgPathSegment
    {
        private Point _start;
        private Point _end;

        public Point Start
        {
            get { return this._start; }
            set { this._start = value; }
        }

        public Point End
        {
            get { return this._end; }
            set { this._end = value; }
        }

        protected SvgPathSegment()
        {
        }

        protected SvgPathSegment(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        public abstract void AddToPath(Path graphicsPath);

        public SvgPathSegment Clone()
        {
            return this.MemberwiseClone() as SvgPathSegment;
        }
    }
}
