using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NGraphics.Codes;
using NGraphics.Models;
using NGraphics.Models.Brushes;

namespace NGraphics.Parsers
{
  public class StylesParser : IStylesParser
  {
    private readonly IFormatProvider icult = CultureInfo.InvariantCulture;
    private readonly Regex _fillUrlRe = new Regex(@"url\s*\(\s*#([^\)]+)\)");
    private readonly Regex _styleValuesRegEx = new Regex(@"\s*(\w+)\s*:\s*(.*)");
    private readonly IValuesParser _valuesParser;

    public StylesParser(IValuesParser valuesParser)
    {
      _valuesParser = valuesParser;
    }

    public Pen GetPen(Dictionary<string, string> styleAttributes)
    {
      Pen pen = null;

      var strokeWidth = GetString(styleAttributes, "stroke-width");
      if (!string.IsNullOrWhiteSpace(strokeWidth))
      {
        if (pen == null)
          pen = new Pen();
        pen.Width = _valuesParser.ReadNumber(strokeWidth);
      }

      var strokeOpacity = GetString(styleAttributes, "stroke-opacity");
      if (!string.IsNullOrWhiteSpace(strokeOpacity))
      {
        if (pen == null)
          pen = new Pen();
        pen.Color = pen.Color.WithAlpha(_valuesParser.ReadNumber(strokeOpacity));
      }

      var linejoin = GetString(styleAttributes, "stroke-linejoin");
      if (!string.IsNullOrWhiteSpace(linejoin))
      {
        if (pen == null)
          pen = new Pen();

        switch (linejoin)
        {
          case "round":
            pen.LineJoin = SvgStrokeLineJoin.Round;
            break;
          case "bevel":
            pen.LineJoin = SvgStrokeLineJoin.Bevel;
            break;
          case "miter":
            pen.LineJoin = SvgStrokeLineJoin.Miter;
            break;
        }
      }

      var lineCap = GetString(styleAttributes, "stroke-linecap");
      if (!string.IsNullOrWhiteSpace(lineCap))
      {
        if (pen == null)
          pen = new Pen();

        switch (lineCap)
        {
          case "round":
            pen.LineCap = SvgStrokeLineCap.Round;
            break;
          case "butt":
            pen.LineCap = SvgStrokeLineCap.Butt;
            break;
        }
      }

      var stroke = GetString(styleAttributes, "stroke").Trim();

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

      return pen;
    }

    public BaseBrush GetBrush(Dictionary<string, string> styleAttributes,Dictionary<string, XElement> defs, Pen pen)
    {
      BaseBrush baseBrush = null;

      var fillOpacity = GetString(styleAttributes, "fill-opacity");
      if (!string.IsNullOrWhiteSpace(fillOpacity))
      {
        if (baseBrush == null)
          baseBrush = new SolidBrush();
        var sb = baseBrush as SolidBrush;
        if (sb != null)
          sb.Color = sb.Color.WithAlpha(_valuesParser.ReadNumber(fillOpacity));
      }

      var fillRule = GetString(styleAttributes, "fill-rule");
      if (!string.IsNullOrWhiteSpace(fillRule))
      {
        if (baseBrush == null)
          baseBrush = new SolidBrush();
        var sb = baseBrush as SolidBrush;
        if (sb != null)
        {
          if (fillRule.Equals("evenodd"))
          {
            sb.FillMode = FillMode.EvenOdd;
          }
        }
      }

      var fill = GetString(styleAttributes, "fill").Trim();
      if (string.IsNullOrEmpty(fill))
      {
        // No change
      }
      else if (fill == "none")
      {
        baseBrush = null;
      }
      else
      {
        Color color;
        if (Colors.TryParse(fill, out color))
        {
          var sb = baseBrush as SolidBrush;
          if (sb == null)
          {
            baseBrush = new SolidBrush(color);
          }
          else
          {
            if (sb.Color.Alpha == 1 || pen == null)
              sb.Color = color;
            else
              sb.Color = color.WithAlpha(sb.Color.Alpha);
          }
        }
        else
        {
          var urlM = _fillUrlRe.Match(fill);
          if (urlM.Success)
          {
            var id = urlM.Groups[1].Value.Trim();
            XElement defE;
            if (defs.TryGetValue(id, out defE))
            {
              switch (defE.Name.LocalName)
              {
                case "linearGradient":
                  baseBrush = CreateLinearGradientBrush(defE);
                  break;
                case "radialGradient":
                  baseBrush = CreateRadialGradientBrush(defE);
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

      return baseBrush;
    }

      public Dictionary<string, string> ParseStyleValues(string styleString)
      {
          var stylesDictionary = new Dictionary<string, string>();
          var keyValuePairs = styleString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

          foreach (var keyValuePair in keyValuePairs)
          {
              var m = _styleValuesRegEx.Match(keyValuePair);

              if (m.Success)
              {
                  var styleKeyValue = keyValuePair.Split(':');
                  var key = styleKeyValue[0];
                  var value = styleKeyValue[1];
                 
                  stylesDictionary[key] = value;
              }
          }

          return stylesDictionary;
      }

    private string GetString(Dictionary<string, string> style, string name, string defaultValue = "")
    {
      string v;
      if (style.TryGetValue(name, out v))
        return v;
      return defaultValue;
    }

    private void ReadStops(XElement e, List<GradientStop> stops)
    {
      var ns = e.Name.Namespace;
      foreach (var se in e.Elements(ns + "stop"))
      {
        var s = new GradientStop();
        s.Offset = _valuesParser.ReadNumber(se.Attribute("offset"));
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

        return new Color(r / 255.0, g / 255.0, b / 255.0, 1);
      }

      throw new NotSupportedException("Color " + s);
    }

    private RadialGradientBrush CreateRadialGradientBrush(XElement e)
    {
      var b = new RadialGradientBrush();

      b.RelativeCenter.X = _valuesParser.ReadNumber(e.Attribute("cx"));
      b.RelativeCenter.Y = _valuesParser.ReadNumber(e.Attribute("cy"));
      b.RelativeFocus.X = _valuesParser.ReadNumber(e.Attribute("fx"));
      b.RelativeFocus.Y = _valuesParser.ReadNumber(e.Attribute("fy"));
      b.RelativeRadius = _valuesParser.ReadNumber(e.Attribute("r"));

      ReadStops(e, b.Stops);

      return b;
    }

    private LinearGradientBrush CreateLinearGradientBrush(XElement e)
    {
      var b = new LinearGradientBrush();

      b.RelativeStart.X = _valuesParser.ReadNumber(e.Attribute("x1"));
      b.RelativeStart.Y = _valuesParser.ReadNumber(e.Attribute("y1"));
      b.RelativeEnd.X = _valuesParser.ReadNumber(e.Attribute("x2"));
      b.RelativeEnd.Y = _valuesParser.ReadNumber(e.Attribute("y2"));

      ReadStops(e, b.Stops);

      return b;
    }
  }
}
