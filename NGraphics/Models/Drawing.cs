using System;

namespace NGraphics.Models
{
    public class Drawing
    {
        public Drawing(Size size, DrawingFunc func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            this.size = size;
            this.func = func;
        }

        private readonly DrawingFunc func;
        private readonly Size size;
        private Graphic graphic;

        public Graphic Graphic
        {
            get
            {
                if (graphic == null)
                {
                    try
                    {
                        DrawGraphic();
                    }
                    catch (Exception ex)
                    {
                        graphic = new Graphic(size);
                        Log.Error(ex);
                    }
                }
                return graphic;
            }
        }

        public void Invalidate()
        {
            graphic = null;
        }

        private void DrawGraphic()
        {
            var c = new GraphicCanvas(size);
            if (func != null)
                func(c);
            graphic = c.Graphic;
        }
    }
}