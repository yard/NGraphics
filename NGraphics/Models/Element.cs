using System;
using NGraphics.Interfaces;

namespace NGraphics.Models
{
    public abstract class Element : IDrawable
    {
        public Element(Pen pen, BaseBrush baseBrush)
        {
            Id = Guid.NewGuid().ToString();
            Pen = pen;
            BaseBrush = baseBrush;
        }

        public string Id { get; set; }
        public Transform Transform { get; set; }
        public Pen Pen { get; set; }
        public BaseBrush BaseBrush { get; set; }

        #region IDrawable implementation

        public void Draw(ICanvas canvas)
        {
            var t = Transform;
            var pushedState = false;
            if (t != null)
            {
                canvas.SaveState();
                pushedState = true;
            }
            try
            {
                if (t != null)
                {
                    canvas.Transform(t);
                }
                DrawElement(canvas);
            }
            finally
            {
                if (pushedState)
                {
                    canvas.RestoreState();
                }
            }
        }

        #endregion

        protected abstract void DrawElement(ICanvas canvas);
    }
}