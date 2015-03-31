namespace NGraphics.Models
{
    public class BoundingBoxBuilder
    {
        private Rect bb;
        private int nbb;

        public Rect BoundingBox
        {
            get { return bb; }
        }

        public void Add(Point point)
        {
            if (nbb == 0)
            {
                bb = new Rect(point, Size.Zero);
            }
            else
            {
                bb = bb.Union(point);
            }
            nbb++;
        }
    }
}