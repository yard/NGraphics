using System;
using System.Globalization;
using System.Runtime.InteropServices;
using NGraphics.Codes;

namespace NGraphics.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;

        public double Red
        {
            get { return R/255.0; }
            set { R = Round(value); }
        }

        public double Green
        {
            get { return G/255.0; }
            set { G = Round(value); }
        }

        public double Blue
        {
            get { return B/255.0; }
            set { B = Round(value); }
        }

        public double Alpha
        {
            get { return A/255.0; }
            set { A = Round(value); }
        }

        private static byte Round(double c)
        {
            return (byte) (Math.Min(255, Math.Max(0, (int) (c*255 + 0.5))));
        }

        public int Argb
        {
            get { return (A << 24) | (R << 16) | (G << 8) | B; }
        }

        private Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(double red, double green, double blue, double alpha)
        {
            R = (byte) (Math.Min(255, Math.Max(0, (int) (red*255 + 0.5))));
            G = (byte) (Math.Min(255, Math.Max(0, (int) (green*255 + 0.5))));
            B = (byte) (Math.Min(255, Math.Max(0, (int) (blue*255 + 0.5))));
            A = (byte) (Math.Min(255, Math.Max(0, (int) (alpha*255 + 0.5))));
        }

        public Color(double red, double green, double blue)
        {
            R = (byte) (Math.Min(255, Math.Max(0, (int) (red*255 + 0.5))));
            G = (byte) (Math.Min(255, Math.Max(0, (int) (green*255 + 0.5))));
            B = (byte) (Math.Min(255, Math.Max(0, (int) (blue*255 + 0.5))));
            A = 255;
        }

        public Color(double white, double alpha = 1.0)
        {
            var W = (byte) (Math.Min(255, Math.Max(0, (int) (white*255 + 0.5))));
            R = W;
            G = W;
            B = W;
            A = (byte) (Math.Min(255, Math.Max(0, (int) (alpha*255 + 0.5))));
        }

        public Color(string colorString)
        {
            Color color;
            if (Colors.TryParse(colorString, out color))
            {
                R = color.R;
                G = color.G;
                B = color.B;
                A = color.A;
            }
            else
            {
                throw new ArgumentException("Bad color string: " + colorString);
            }
        }

        public Color BlendWith(Color other, double otherWeight)
        {
            var t = otherWeight;
            var t1 = 1 - t;
            var r = Red*t1 + other.Red*t;
            var g = Green*t1 + other.Green*t;
            var b = Blue*t1 + other.Blue*t;
            var a = Alpha*t1 + other.Alpha*t;
            return new Color(r, g, b, a);
        }

        public Color WithAlpha(double alpha)
        {
            var a = (byte) (Math.Min(255, Math.Max(0, (int) (alpha*255 + 0.5))));
            return new Color(R, G, B, a);
        }

        public static Color FromHSB(double hue, double saturation, double brightness, double alpha = 1.0)
        {
            var c = saturation*brightness;
            var hp = hue;
            if (hp < 0)
                hp = 1 - ((-hp)%1);
            if (hp > 1)
                hp = hp%1;
            hp *= 6;
            var x = c*(1 - Math.Abs((hp%2) - 1));
            double r1, g1, b1;
            if (hp < 1)
            {
                r1 = c;
                g1 = x;
                b1 = 0;
            }
            else if (hp < 2)
            {
                r1 = x;
                g1 = c;
                b1 = 0;
            }
            else if (hp < 3)
            {
                r1 = 0;
                g1 = c;
                b1 = x;
            }
            else if (hp < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = c;
            }
            else if (hp < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = c;
            }
            else
            {
                r1 = c;
                g1 = 0;
                b1 = x;
            }
            var m = brightness - c;
            return new Color(r1 + m, g1 + m, b1 + m, alpha);
        }
        public static Color FromHSL(double hue, double saturation, double lightness, double alpha = 1.0)
        {
          var c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
          var hp = hue;
          if (hp < 0)
            hp = 1 - ((-hp) % 1);
          if (hp > 1)
            hp = hp % 1;
          hp *= 6;
          var x = c * (1 - Math.Abs((hp % 2) - 1));
          double r1, g1, b1;
          if (hp < 1)
          {
            r1 = c;
            g1 = x;
            b1 = 0;
          }
          else if (hp < 2)
          {
            r1 = x;
            g1 = c;
            b1 = 0;
          }
          else if (hp < 3)
          {
            r1 = 0;
            g1 = c;
            b1 = x;
          }
          else if (hp < 4)
          {
            r1 = 0;
            g1 = x;
            b1 = c;
          }
          else if (hp < 5)
          {
            r1 = x;
            g1 = 0;
            b1 = c;
          }
          else
          {
            r1 = c;
            g1 = 0;
            b1 = x;
          }
          var m = lightness - 0.5 * c;
          return new Color(r1 + m, g1 + m, b1 + m, alpha);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Color ({0}, {1}, {2}, {3})", Red, Green, Blue, Alpha);
        }
    }
}