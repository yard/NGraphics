using System;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;
using NGraphics.Custom.Models.Transforms;

namespace NGraphics.Custom.Models.Elements
{
    public abstract class Element : IDrawable
    {
        public string Id { get; set; }
        public Transform Transform { get; set; }
        public Pen Pen { get; set; }
        public BaseBrush Brush { get; set; }

        public Element(Pen pen, BaseBrush brush)
        {
            Id = Guid.NewGuid().ToString();
            Pen = pen;
            Brush = brush;
            Transform = Transform.Identity;
        }

        protected abstract void DrawElement(ICanvas canvas);

        #region IDrawable implementation

        public void Draw(ICanvas canvas)
        {
            var t = Transform;
            var pushedState = false;
            try
            {
                if (t != Transform.Identity)
                {
                    canvas.SaveState();
                    pushedState = true;
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
    }
}