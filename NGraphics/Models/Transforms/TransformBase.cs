namespace NGraphics.Models.Transforms
{
    public abstract class TransformBase
    {
        protected TransformBase(TransformBase previous = null)
        {
            Previous = previous;
        }

        public TransformBase Previous;
        protected abstract string ToCode();

        public override string ToString()
        {
            var s = ToCode();
            if (Previous != null)
            {
                s = Previous + " " + s;
            }
            return s;
        }
    }
}