using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Brushes;
using NGraphics.Models.Elements;
using NGraphics.Models.Transforms;
using Group = NGraphics.Models.Elements.Group;
using Path = NGraphics.Models.Elements.Path;

namespace NGraphics.Parsers
{
  public class SvgReader
  {
    private readonly IStylesParser _stylesParser;
    private readonly IValuesParser _valuesParser;
    //		readonly XNamespace ns;

    public SvgReader(TextReader reader, IStylesParser stylesParser, IValuesParser valuesParser)
    {
      _stylesParser = stylesParser;
      _valuesParser = valuesParser;
      Read(XDocument.Load(reader));
    }

    private static readonly char[] WS = {' ', '\t', '\n', '\r'};
    private readonly Dictionary<string, XElement> defs = new Dictionary<string, XElement>();
    private readonly Regex keyValueRe = new Regex(@"\s*(\w+)\s*:\s*(.*)");
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
      var width = _valuesParser.ReadNumber(widthA);
      var height = _valuesParser.ReadNumber(heightA);
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

    private void AddElements(IList<IDrawable> list, IEnumerable<XElement> es, Pen inheritPen,
      BaseBrush inheritBaseBrush)
    {
      foreach (var e in es)
        AddElement(list, e, inheritPen, inheritBaseBrush);
    }

    private void AddElement(IList<IDrawable> list, XElement e, Pen inheritPen, BaseBrush inheritBaseBrush)
    {
      Element element = null;

      var styleAttributedDictionary = e.Attributes().ToDictionary(k => k.Name.LocalName, v => v.Value);

      var pen = _stylesParser.GetPen(styleAttributedDictionary);
      var baseBrush = _stylesParser.GetBrush(styleAttributedDictionary,defs, pen);

      var style = ReadString(e.Attribute("style"));
      
      if (!string.IsNullOrWhiteSpace(style))
      {
        ApplyStyle(style, ref pen, ref baseBrush);
      }
      
      pen = pen ?? inheritPen;
      baseBrush = baseBrush ?? inheritBaseBrush;

      //var id = ReadString (e.Attribute ("id"));

      //
      // Elements
      //
      switch (e.Name.LocalName)
      {
        case "text":
        {
          var x = _valuesParser.ReadNumber(e.Attribute("x"));
          var y = _valuesParser.ReadNumber(e.Attribute("y"));
          var text = e.Value.Trim();
          var font = new Font();
          element = new Text(text, new Rect(new Point(x, y), new Size(double.MaxValue, double.MaxValue)), font,
            TextAlignment.Left, pen, baseBrush);
        }
          break;
        case "rect":
        {
          var x = _valuesParser.ReadNumber(e.Attribute("x"));
          var y = _valuesParser.ReadNumber(e.Attribute("y"));
          var width = _valuesParser.ReadNumber(e.Attribute("width"));
          var height = _valuesParser.ReadNumber(e.Attribute("height"));
          element = new Rectangle(new Point(x, y), new Size(width, height), pen, baseBrush);
        }
          break;
        case "ellipse":
        {
          var cx = _valuesParser.ReadNumber(e.Attribute("cx"));
          var cy = _valuesParser.ReadNumber(e.Attribute("cy"));
          var rx = _valuesParser.ReadNumber(e.Attribute("rx"));
          var ry = _valuesParser.ReadNumber(e.Attribute("ry"));
          element = new Ellipse(new Point(cx - rx, cy - ry), new Size(2*rx, 2*ry), pen, baseBrush);
        }
          break;
        case "circle":
        {
          var cx = _valuesParser.ReadNumber(e.Attribute("cx"));
          var cy = _valuesParser.ReadNumber(e.Attribute("cy"));
          var rr = _valuesParser.ReadNumber(e.Attribute("r"));
          element = new Ellipse(new Point(cx - rr, cy - rr), new Size(2*rr, 2*rr), pen, baseBrush);
        }
          break;
        case "path":
        {
          var dA = e.Attribute("d");
          if (dA != null && !string.IsNullOrWhiteSpace(dA.Value))
          {
            var p = new Path(pen, baseBrush);
            SvgPathParser.Parse(p, dA.Value);
            element = p;
          }
        }
          break;
        case "g":
        {
          var g = new Group();
          AddElements(g.Children, e.Elements(), pen, baseBrush);
          element = g;
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
              AddElement(useList, useE, pen, baseBrush);
              element = useList.OfType<Element>().FirstOrDefault();
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
        case "SVGTestCase":
          break;
        default:
          throw new NotSupportedException("SVG element \"" + e.Name.LocalName + "\" is not supported");
      }

      if (element != null)
      {
        element.Transform = ReadTransform(ReadString(e.Attribute("transform")));
        list.Add(element);
      }
    }

    private void ApplyStyle(string style, ref Pen pen, ref BaseBrush baseBrush)
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

      pen = _stylesParser.GetPen(d);
      baseBrush = _stylesParser.GetBrush(d,defs, pen);
    }
   
    private TransformBase ReadTransform(string raw)
    {
      if (string.IsNullOrWhiteSpace(raw))
        return null;

      var s = raw.Trim();

      var calls = s.Split(new[] {')'}, StringSplitOptions.RemoveEmptyEntries);

      TransformBase t = null;

      foreach (var c in calls)
      {
        var args = c.Split(new[] {'(', ',', ' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
        TransformBase nt = null;
        switch (args[0])
        {
          case "matrix":
            if (args.Length == 7)
            {
              var m = new MatrixTransform(t);
              nt = new Translate(new Size(_valuesParser.ReadNumber(args[1]), _valuesParser.ReadNumber(args[2])), t);
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
              nt = new Translate(new Size(_valuesParser.ReadNumber(args[1]), _valuesParser.ReadNumber(args[2])), t);
            }
            else if (args.Length >= 2)
            {
              nt = new Translate(new Size(_valuesParser.ReadNumber(args[1]), 0), t);
            }
            break;
          case "scale":
            if (args.Length >= 3)
            {
              nt = new Scale(new Size(_valuesParser.ReadNumber(args[1]), _valuesParser.ReadNumber(args[2])), t);
            }
            else if (args.Length >= 2)
            {
              var sx = _valuesParser.ReadNumber(args[1]);
              nt = new Scale(new Size(sx, sx), t);
            }
            break;
          case "rotate":
            var a = _valuesParser.ReadNumber(args[1]);
            if (args.Length >= 4)
            {
              var x = _valuesParser.ReadNumber(args[2]);
              var y = _valuesParser.ReadNumber(args[3]);
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

    private Rect ReadRectangle(string s)
    {
      var r = new Rect();
      var p = s.Split(WS, StringSplitOptions.RemoveEmptyEntries);
      if (p.Length > 0)
        r.X = _valuesParser.ReadNumber(p[0]);
      if (p.Length > 1)
        r.Y = _valuesParser.ReadNumber(p[1]);
      if (p.Length > 2)
        r.Width = _valuesParser.ReadNumber(p[2]);
      if (p.Length > 3)
        r.Height = _valuesParser.ReadNumber(p[3]);
      return r;
    }
  }
}