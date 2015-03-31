using System.Collections.Generic;
using System.Globalization;
using NGraphics.Models;

namespace NGraphics.Codes
{
    public static class Colors
    {
        static Colors()
        {
            names["clear"] = Clear;
            names["black"] = Black;
            names["darkgray"] = DarkGray;
            names["gray"] = Gray;
            names["lightgray"] = LightGray;
            names["white"] = White;
            names["red"] = Red;
            names["orange"] = Orange;
            names["yellow"] = Yellow;
            names["green"] = Green;
            names["blue"] = Blue;
            names["transparent"] = Clear;
        }

        public static readonly Color Clear = new Color(0, 0, 0, 0);
        public static readonly Color Black = new Color(0, 0, 0, 1);
        public static readonly Color DarkGray = new Color(0.25, 0.25, 0.25, 1);
        public static readonly Color Gray = new Color(0.5, 0.5, 0.5, 1);
        public static readonly Color LightGray = new Color(0.75, 0.75, 0.75, 1);
        public static readonly Color White = new Color(1, 1, 1, 1);
        public static readonly Color Red = new Color(1, 0, 0, 1);
        public static readonly Color Orange = new Color(1, 0xA5/255.0, 0, 1);
        public static readonly Color Yellow = new Color(1, 1, 0, 1);
        public static readonly Color Green = new Color(0, 1, 0, 1);
        public static readonly Color Blue = new Color(0, 0, 1, 1);
        private static readonly Dictionary<string, Color> names = new Dictionary<string, Color>();

        public static bool TryParse(string colorString, out Color color)
        {
            if (string.IsNullOrWhiteSpace(colorString))
            {
                color = Clear;
                return false;
            }

            var s = colorString.Trim();

            if (s.Length == 7 && s[0] == '#')
            {
                var icult = CultureInfo.InvariantCulture;

                var r = int.Parse(s.Substring(1, 2), NumberStyles.HexNumber, icult);
                var g = int.Parse(s.Substring(3, 2), NumberStyles.HexNumber, icult);
                var b = int.Parse(s.Substring(5, 2), NumberStyles.HexNumber, icult);

                color = new Color(r/255.0, g/255.0, b/255.0, 1);
                return true;
            }

            Color nc;
            if (names.TryGetValue(s.ToLowerInvariant(), out nc))
            {
                color = nc;
                return true;
            }

            color = Clear;
            return false;
        }
    }
}