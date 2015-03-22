using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Parsers;

namespace NGraphics
{
    public class SvgReader
    {
        private static readonly char[] WSC = {',', ' ', '\t', '\n', '\r'};
        private static readonly char[] WS = {' ', '\t', '\n', '\r'};
        private readonly Dictionary<string, XElement> defs = new Dictionary<string, XElement>();
        private readonly Regex fillUrlRe = new Regex(@"url\s*\(\s*#([^\)]+)\)");
        private readonly IFormatProvider icult = CultureInfo.InvariantCulture;
        private readonly Regex keyValueRe = new Regex(@"\s*(\w+)\s*:\s*(.*)");
//		readonly XNamespace ns;

        public SvgReader(TextReader reader)
        {
            Read(XDocument.Load(reader));
        }

        public Graphic Graphic { get; private set; }

        private void Read(XDocument doc)
        {
            var svg = doc.Root;
            var ns = svg.Name.Namespace;

            //
            // Find the defs (gradients)
            //
            foreach (var d in svg.Descendants())
            {
                var idA = d.Attribute("id");
                if (idA != null)
                {
                    defs[ReadString(idA).Trim()] = d;
                }
            }

            //
            // Get the dimensions
            //
            var widthA = svg.Attribute("width");
            var heightA = svg.Attribute("height");
            var width = ReadNumber(widthA);
            var height = ReadNumber(heightA);
            var size = new Size(width, height);

            var viewBox = new Rect(size);
            var viewBoxA = svg.Attribute("viewBox") ?? svg.Attribute("viewPort");
            if (viewBoxA != null)
            {
                viewBox = ReadRectangle(viewBoxA.Value);
            }

            if (widthA != null && widthA.Value.Contains("%"))
            {
                size.Width *= viewBox.Width;
            }
            if (heightA != null && heightA.Value.Contains("%"))
            {
                size.Height *= viewBox.Height;
            }

            //
            // Add the elements
            //
            Graphic = new Graphic(size, viewBox);

            AddElements(Graphic.Children, svg.Elements(), null, null);
        }

        private void AddElements(IList<IDrawable> list, IEnumerable<XElement> es, Pen inheritPen, Brush inheritBrush)
        {
            foreach (var e in es)
                AddElement(list, e, inheritPen, inheritBrush);
        }

        private void AddElement(IList<IDrawable> list, XElement e, Pen inheritPen, Brush inheritBrush)
        {
            //
            // Style
            //
            Element r = null;
            Pen pen = null;
            Brush brush = null;
            ApplyStyle(e.Attributes().ToDictionary(k => k.Name.LocalName, v => v.Value), ref pen, ref brush);
            var style = ReadString(e.Attribute("style"));
            if (!string.IsNullOrWhiteSpace(style))
            {
                ApplyStyle(style, ref pen, ref brush);
            }
            pen = pen ?? inheritPen;
            brush = brush ?? inheritBrush;
            if (pen == null && brush == null)
            {
                brush = Brushes.Black;
            }
            //var id = ReadString (e.Attribute ("id"));

            //
            // Elements
            //
            switch (e.Name.LocalName)
            {
                case "text":
                {
                    var x = ReadNumber(e.Attribute("x"));
                    var y = ReadNumber(e.Attribute("y"));
                    var text = e.Value.Trim();
                    var font = new Font();
                    r = new Text(text, new Rect(new Point(x, y), new Size(double.MaxValue, double.MaxValue)), font,
                        TextAlignment.Left, pen, brush);
                }
                    break;
                case "rect":
                {
                    var x = ReadNumber(e.Attribute("x"));
                    var y = ReadNumber(e.Attribute("y"));
                    var width = ReadNumber(e.Attribute("width"));
                    var height = ReadNumber(e.Attribute("height"));
                    r = new Rectangle(new Point(x, y), new Size(width, height), pen, brush);
                }
                    break;
                case "ellipse":
                {
                    var cx = ReadNumber(e.Attribute("cx"));
                    var cy = ReadNumber(e.Attribute("cy"));
                    var rx = ReadNumber(e.Attribute("rx"));
                    var ry = ReadNumber(e.Attribute("ry"));
                    r = new Ellipse(new Point(cx - rx, cy - ry), new Size(2*rx, 2*ry), pen, brush);
                }
                    break;
                case "circle":
                {
                    var cx = ReadNumber(e.Attribute("cx"));
                    var cy = ReadNumber(e.Attribute("cy"));
                    var rr = ReadNumber(e.Attribute("r"));
                    r = new Ellipse(new Point(cx - rr, cy - rr), new Size(2*rr, 2*rr), pen, brush);
                }
                    break;
                case "path":
                {
                    var dA = e.Attribute("d");
                    if (dA != null && !string.IsNullOrWhiteSpace(dA.Value))
                    {
                        var p = new Path(pen, brush);
                        ReadPath(p, dA.Value);
                        r = p;
                    }
                }
                    break;
                case "g":
                {
                    var g = new Group();
                    AddElements(g.Children, e.Elements(), pen, brush);
                    r = g;
                }
                    break;
                case "use":
                {
                    var href = ReadString(e.Attributes().FirstOrDefault(x => x.Name.LocalName == "href"));
                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        XElement useE;
                        if (defs.TryGetValue(href.Trim().Replace("#", ""), out useE))
                        {
                            var useList = new List<IDrawable>();
                            AddElement(useList, useE, pen, brush);
                            r = useList.OfType<Element>().FirstOrDefault();
                        }
                    }
                }
                    break;
                case "title":
                    Graphic.Title = ReadString(e);
                    break;
                case "description":
                    Graphic.Description = ReadString(e);
                    break;
                case "defs":
                    // Already read in earlier pass
                    break;
                case "namedview":
                case "metadata":
                    // Ignore
                    break;
                default:
                    throw new NotSupportedException("SVG element \"" + e.Name.LocalName + "\" is not supported");
            }

            if (r != null)
            {
                r.Transform = ReadTransform(ReadString(e.Attribute("transform")));
                list.Add(r);
            }
        }

        private void ApplyStyle(string style, ref Pen pen, ref Brush brush)
        {
            var d = new Dictionary<string, string>();
            var kvs = style.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var kv in kvs)
            {
                var m = keyValueRe.Match(kv);
                if (m.Success)
                {
                    var k = m.Groups[1].Value;
                    var v = m.Groups[2].Value;
                    d[k] = v;
                }
            }
            ApplyStyle(d, ref pen, ref brush);
        }

        private string GetString(Dictionary<string, string> style, string name, string defaultValue = "")
        {
            string v;
            if (style.TryGetValue(name, out v))
                return v;
            return defaultValue;
        }

        private void ApplyStyle(Dictionary<string, string> style, ref Pen pen, ref Brush brush)
        {
            //
            // Pen attributes
            //
            var strokeWidth = GetString(style, "stroke-width");
            if (!string.IsNullOrWhiteSpace(strokeWidth))
            {
                if (pen == null)
                    pen = new Pen();
                pen.Width = ReadNumber(strokeWidth);
            }

            var strokeOpacity = GetString(style, "stroke-opacity");
            if (!string.IsNullOrWhiteSpace(strokeOpacity))
            {
                if (pen == null)
                    pen = new Pen();
                pen.Color = pen.Color.WithAlpha(ReadNumber(strokeOpacity));
            }

            //
            // Pen
            //
            var stroke = GetString(style, "stroke").Trim();
            if (string.IsNullOrEmpty(stroke))
            {
                // No change
            }
            else if (stroke == "none")
            {
                pen = null;
            }
            else
            {
                if (pen == null)
                    pen = new Pen();
                Color color;
                if (Colors.TryParse(stroke, out color))
                {
                    if (pen.Color.Alpha == 1)
                        pen.Color = color;
                    else
                        pen.Color = color.WithAlpha(pen.Color.Alpha);
                }
            }

            //
            // Brush attributes
            //
            var fillOpacity = GetString(style, "fill-opacity");
            if (!string.IsNullOrWhiteSpace(fillOpacity))
            {
                if (brush == null)
                    brush = new SolidBrush();
                var sb = brush as SolidBrush;
                if (sb != null)
                    sb.Color = sb.Color.WithAlpha(ReadNumber(fillOpacity));
            }

            //
            // Brush
            //
            var fill = GetString(style, "fill").Trim();
            if (string.IsNullOrEmpty(fill))
            {
                // No change
            }
            else if (fill == "none")
            {
                brush = null;
            }
            else
            {
                Color color;
                if (Colors.TryParse(fill, out color))
                {
                    var sb = brush as SolidBrush;
                    if (sb == null)
                    {
                        brush = new SolidBrush(color);
                    }
                    else
                    {
                        if (sb.Color.Alpha == 1)
                            sb.Color = color;
                        else
                            sb.Color = color.WithAlpha(pen.Color.Alpha);
                    }
                }
                else
                {
                    var urlM = fillUrlRe.Match(fill);
                    if (urlM.Success)
                    {
                        var id = urlM.Groups[1].Value.Trim();
                        XElement defE;
                        if (defs.TryGetValue(id, out defE))
                        {
                            switch (defE.Name.LocalName)
                            {
                                case "linearGradient":
                                    brush = CreateLinearGradientBrush(defE);
                                    break;
                                case "radialGradient":
                                    brush = CreateRadialGradientBrush(defE);
                                    break;
                                default:
                                    throw new NotSupportedException("Fill " + defE.Name);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid fill url reference: " + id);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Fill " + fill);
                    }
                }
            }
        }

        private Transform ReadTransform(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            var s = raw.Trim();

            var calls = s.Split(new[] {')'}, StringSplitOptions.RemoveEmptyEntries);

            Transform t = null;

            foreach (var c in calls)
            {
                var args = c.Split(new[] {'(', ',', ' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                Transform nt = null;
                switch (args[0])
                {
                    case "matrix":
                        if (args.Length == 7)
                        {
                            var m = new MatrixTransform(t);
                            nt = new Translate(new Size(ReadNumber(args[1]), ReadNumber(args[2])), t);
                        }
                        else
                        {
                            throw new NotSupportedException("Matrices are expected to have 6 elements, this one has " +
                                                            (args.Length - 1));
                        }
                        break;
                    case "translate":
                        if (args.Length >= 3)
                        {
                            nt = new Translate(new Size(ReadNumber(args[1]), ReadNumber(args[2])), t);
                        }
                        else if (args.Length >= 2)
                        {
                            nt = new Translate(new Size(ReadNumber(args[1]), 0), t);
                        }
                        break;
                    case "scale":
                        if (args.Length >= 3)
                        {
                            nt = new Scale(new Size(ReadNumber(args[1]), ReadNumber(args[2])), t);
                        }
                        else if (args.Length >= 2)
                        {
                            var sx = ReadNumber(args[1]);
                            nt = new Scale(new Size(sx, sx), t);
                        }
                        break;
                    case "rotate":
                        var a = ReadNumber(args[1]);
                        if (args.Length >= 4)
                        {
                            var x = ReadNumber(args[2]);
                            var y = ReadNumber(args[3]);
                            var t1 = new Translate(new Size(x, y), t);
                            var t2 = new Rotate(a, t1);
                            var t3 = new Translate(new Size(-x, -y), t2);
                            nt = t3;
                        }
                        else
                        {
                            nt = new Rotate(a, t);
                        }
                        break;
                    default:
                        throw new NotSupportedException("Can't transform " + args[0]);
                }
                if (nt != null)
                {
                    t = nt;
                }
            }

            return t;
        }

        private void ReadPath(Path path, string pathDescriptor)
        {
            var functions = Regex.Split(pathDescriptor, @"(?=[A-Za-z])").Where(c => !string.IsNullOrEmpty(c));

            foreach (var function in functions)
            {
                var command = function[0];
                var args =
                    Regex.Split(function.Remove(0, 1), @"[\s,]|(?=-)")
                        .Where(c => !string.IsNullOrEmpty(c))
                        .Select(c => double.Parse(c))
                        .ToList();

                ProcessCommand(path, command.ToString(), args);
            }
        }

        private void ProcessCommand(Path path, string command, List<double> args)
        {
            var operation = OperationParser.Parse(command);

            switch (operation.Type)
            {
                case OperationType.MoveTo:
                {
                    path.MoveTo(args[0], args[1], operation.IsAbsolute);
                    break;
                }

                case OperationType.LineTo:
                {
                    path.LineTo(args[0], args[1], operation.IsAbsolute);
                    break;
                }
                case OperationType.CubicBezierCurve:
                {
                    path.CurveTo(new Point(args[0], args[1]), new Point(args[2], args[3]), new Point(args[4], args[5]));
                    break;
                }
                case OperationType.SmoothCubicBezierCurve:
                {
                    path.ContinueCurveTo(new Point(args[0], args[1]), new Point(args[2], args[3]));
                    break;
                }
                case OperationType.ArcTo:
                {
                    var largeArc = args[3] != 0;
                    var sweepClockwise = args[4] != 0;
                    path.ArcTo(new Size(args[0], args[1]), largeArc, sweepClockwise, new Point(args[5], args[6]));
                    break;
                }
                case OperationType.Close:
                {
                    path.Close();
                    break;
                }
                default:
                    throw new NotSupportedException(String.Format("Unknown path command - ({0})",
                        operation.OriginalValue));
            }


//        var pathSections = pathDescriptor.Split (WSC, StringSplitOptions.RemoveEmptyEntries);

//            var i = 0;
//            var n = pathSections.Length;

//            while (i < n) {
//                var pathSection = pathSections[i];

//              var operation = OperationParser.Parse(pathSection);

//        if (pathSection.Length == 1) {
//                    i++;
//                } else {
//                    pathSections [i] = pathSection.Substring (1);
//                }

//                //
//                // Execute
//                //
//                if (operation.Type == OperationType.MoveTo && i + 1 < n) {
//                    path.MoveTo (new Point (ReadNumber (pathSections [i]), ReadNumber (pathSections [i + 1])), operation.IsAbsolute);
//                    i += 2;
//                } 
//         else if (operation.Type == OperationType.LineTo && i + 1 < n)
//         {
//           path.LineTo(new Point(ReadNumber(pathSections[i]), ReadNumber(pathSections[i + 1])));
//           i += 2;
//         }
//         //else if (operation.Type == OperationType.CurveTo && i + 5 < n)
//         //{
//         //  var c1 = new Point(ReadNumber(pathSections[i]), ReadNumber(pathSections[i + 1]));
//         //  var c2 = new Point(ReadNumber(pathSections[i + 2]), ReadNumber(pathSections[i + 3]));
//         //  var pt = new Point(ReadNumber(pathSections[i + 4]), ReadNumber(pathSections[i + 5]));
//         //  path.CurveTo(c1, c2, pt);
//         //  i += 6;
//         //}
//         //else if (operation.Type == OperationType.ContinueCurveTo && i + 3 < n)
//         //{
//         //  var c = new Point(ReadNumber(pathSections[i]), ReadNumber(pathSections[i + 1]));
//         //  var pt = new Point(ReadNumber(pathSections[i + 2]), ReadNumber(pathSections[i + 3]));
//         //  path.ContinueCurveTo(c, pt);
//         //  i += 4;
//         //}
//         else if (operation.Type == OperationType.ArcTo && i + 6 < n)
//         {
//           var radius = new Size(ReadNumber(pathSections[i]), ReadNumber(pathSections[i + 1]));
////					var xr = ReadNumber (args [i + 2]);
//           var largeArc = ReadNumber(pathSections[i + 3]) != 0;
//           var sweepClockwise = ReadNumber(pathSections[i + 4]) != 0;
//           var point = new Point(ReadNumber(pathSections[i + 5]), ReadNumber(pathSections[i + 6]));
//           path.ArcTo(radius, largeArc, sweepClockwise, point);
//           i += 7;
//         }
//         else if (operation.Type == OperationType.Close)
//         {
//           path.Close();
//         }
//         else
//         {
//           throw new NotSupportedException("Path Operation " + operation);
//         }
//            }
//        }
        }

        //void generatePathElement()
        //{
        //  bool wasCubicBezierCurve = false;
        //  bool wasQuadraticBezierCurve = false;
        //  switch (this.mCommand)
        //  { // TODO Extract to constants
        //    case 'm':
        //      this.generateMove(false);
        //      break;
        //    case 'M':
        //      this.generateMove(true);
        //      break;
        //    case 'l':
        //      this.generateLine(false);
        //      break;
        //    case 'L':
        //      this.generateLine(true);
        //      break;
        //    case 'h':
        //      this.generateHorizontalLine(false);
        //      break;
        //    case 'H':
        //      this.generateHorizontalLine(true);
        //      break;
        //    case 'v':
        //      this.generateVerticalLine(false);
        //      break;
        //    case 'V':
        //      this.generateVerticalLine(true);
        //      break;
        //    case 'c':
        //      this.generateCubicBezierCurve(false);
        //      wasCubicBezierCurve = true;
        //      break;
        //    case 'C':
        //      this.generateCubicBezierCurve(true);
        //      wasCubicBezierCurve = true;
        //      break;
        //    case 's':
        //      this.generateSmoothCubicBezierCurve(false);
        //      wasCubicBezierCurve = true;
        //      break;
        //    case 'S':
        //      this.generateSmoothCubicBezierCurve(true);
        //      wasCubicBezierCurve = true;
        //      break;
        //    case 'q':
        //      this.generateQuadraticBezierCurve(false);
        //      wasQuadraticBezierCurve = true;
        //      break;
        //    case 'Q':
        //      this.generateQuadraticBezierCurve(true);
        //      wasQuadraticBezierCurve = true;
        //      break;
        //    case 't':
        //      this.generateSmoothQuadraticBezierCurve(false);
        //      wasQuadraticBezierCurve = true;
        //      break;
        //    case 'T':
        //      this.generateSmoothQuadraticBezierCurve(true);
        //      wasQuadraticBezierCurve = true;
        //      break;
        //    case 'a':
        //      this.generateArc(false);
        //      break;
        //    case 'A':
        //      this.generateArc(true);
        //      break;
        //    case 'z':
        //    case 'Z':
        //      this.generateClose();
        //      break;
        //    default:
        //      throw new InvalidOperationException("Unexpected SVG command: " + this.mCommand);
        //  }
        //  if (!wasCubicBezierCurve)
        //  {
        //    this.mLastCubicBezierX2 = this.mLastX;
        //    this.mLastCubicBezierY2 = this.mLastY;
        //  }
        //  if (!wasQuadraticBezierCurve)
        //  {
        //    this.mLastQuadraticBezierX2 = this.mLastX;
        //    this.mLastQuadraticBezierY2 = this.mLastY;
        //  }
        //}

        private string ReadString(XElement e, string defaultValue = "")
        {
            if (e == null)
                return defaultValue;
            return e.Value ?? defaultValue;
        }

        private string ReadString(XAttribute a, string defaultValue = "")
        {
            if (a == null)
                return defaultValue;
            return a.Value ?? defaultValue;
        }

        private RadialGradientBrush CreateRadialGradientBrush(XElement e)
        {
            var b = new RadialGradientBrush();

            b.RelativeCenter.X = ReadNumber(e.Attribute("cx"));
            b.RelativeCenter.Y = ReadNumber(e.Attribute("cy"));
            b.RelativeFocus.X = ReadNumber(e.Attribute("fx"));
            b.RelativeFocus.Y = ReadNumber(e.Attribute("fy"));
            b.RelativeRadius = ReadNumber(e.Attribute("r"));

            ReadStops(e, b.Stops);

            return b;
        }

        private LinearGradientBrush CreateLinearGradientBrush(XElement e)
        {
            var b = new LinearGradientBrush();

            b.RelativeStart.X = ReadNumber(e.Attribute("x1"));
            b.RelativeStart.Y = ReadNumber(e.Attribute("y1"));
            b.RelativeEnd.X = ReadNumber(e.Attribute("x2"));
            b.RelativeEnd.Y = ReadNumber(e.Attribute("y2"));

            ReadStops(e, b.Stops);

            return b;
        }

        private void ReadStops(XElement e, List<GradientStop> stops)
        {
            var ns = e.Name.Namespace;
            foreach (var se in e.Elements(ns + "stop"))
            {
                var s = new GradientStop();
                s.Offset = ReadNumber(se.Attribute("offset"));
                s.Color = ReadColor(se, "stop-color");
                stops.Add(s);
            }
            stops.Sort((x, y) => x.Offset.CompareTo(y.Offset));
        }

        private Color ReadColor(XElement e, string attrib)
        {
            var a = e.Attribute(attrib);
            if (a == null)
                return Colors.Black;
            return ReadColor(a.Value);
        }

        private Color ReadColor(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return Colors.Clear;

            var s = raw.Trim();

            if (s.Length == 7 && s[0] == '#')
            {
                var r = int.Parse(s.Substring(1, 2), NumberStyles.HexNumber, icult);
                var g = int.Parse(s.Substring(3, 2), NumberStyles.HexNumber, icult);
                var b = int.Parse(s.Substring(5, 2), NumberStyles.HexNumber, icult);

                return new Color(r/255.0, g/255.0, b/255.0, 1);
            }

            throw new NotSupportedException("Color " + s);
        }

        private double ReadNumber(XAttribute a)
        {
            if (a == null)
                return 0;
            return ReadNumber(a.Value);
        }

        private double ReadNumber(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return 0;

            var s = raw.Trim();
            var m = 1.0;

            if (s.EndsWith("px"))
            {
                s = s.Substring(0, s.Length - 2);
            }
            else if (s.EndsWith("%"))
            {
                s = s.Substring(0, s.Length - 1);
                m = 0.01;
            }

            double v;
            if (!double.TryParse(s, NumberStyles.Float, icult, out v))
            {
                v = 0;
            }
            return m*v;
        }

        private Rect ReadRectangle(string s)
        {
            var r = new Rect();
            var p = s.Split(WS, StringSplitOptions.RemoveEmptyEntries);
            if (p.Length > 0)
                r.X = ReadNumber(p[0]);
            if (p.Length > 1)
                r.Y = ReadNumber(p[1]);
            if (p.Length > 2)
                r.Width = ReadNumber(p[2]);
            if (p.Length > 3)
                r.Height = ReadNumber(p[3]);
            return r;
        }
    }
}