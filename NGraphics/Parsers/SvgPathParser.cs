using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NGraphics.Codes;
using NGraphics.ExtensionMethods;
using SharpVectors.Dom.Svg;

namespace NGraphics.Parsers
{
  /// <summary>
  /// Source from https://github.com/garuma/xamsvg
  /// </summary>
  public class SvgPathParser
  {
    private static Regex rePathCmd = new Regex(@"(?=[A-DF-Za-df-z])");
    private static Regex coordSplit = new Regex(@"(\s*,\s*)|(\s+)|((?<=[0-9])(?=-))", RegexOptions.ExplicitCapture);

    public Path TestParse(string d)
    {
      var path = new Path();
            //ISvgPathSeg seg;
            string[] segs = rePathCmd.Split(d);

            foreach (string s in segs)
            {
                string segment = s.Trim();
                if (segment.Length > 0)
                {
                    //char cmd = (char)segment.ToCharArray(0, 1)[0];
                  var operation = OperationParser.Parse(segment);
                    double[] coords = getCoords(segment);
                    int length = coords.Length;
                    switch (operation.Type)
                    {
                      #region moveto
                      case OperationType.MoveTo:
                        //for (int i = 0; i < length; i += 2)
                        //{
                          //if (i == 0)
                          //{
                          path.MoveTo(coords[0], coords[1], operation.IsAbsolute);
                        //}
                        break;

                        case OperationType.LineTo:
                      {
                        path.LineTo(coords[0],coords[1], operation.IsAbsolute);
                        break;
                      }
                        case OperationType.ArcTo:
                      {
                        for (int i = 0; i < length; i += 7)
                        {
                          ////path.ArcTo(
                          //  coords[i + 5],
                          //  coords[i + 6],
                          //  coords[i],
                          //  coords[i + 1],
                          //  coords[i + 2],
                          //  (coords[i + 3] != 0),
                          //  (coords[i + 4] != 0));
                        }
                        break;
                      }
                        case OperationType.CubicBezierCurve:
                      {
                        
                        path.CurveTo(new Point(coords[0],coords[1]),new Point(coords[2],coords[3]),new Point(coords[4],coords[5])  );
                        break;
                      }
                        case OperationType.SmoothCubicBezierCurve:
                      {
                        path.ContinueCurveTo(new Point(coords[0],coords[1]),new Point(coords[2],coords[3]) );
                        break;
                      }
                        case OperationType.VerticalLineTo:
                      {
                        path.LineTo(coords[0], 0, operation.IsAbsolute);
                        break;
                      }
                        case OperationType.HorizontalLineTo:
                      {
                        path.LineTo(0, coords[0], operation.IsAbsolute);
                        break;
                      }
                        case OperationType.Close:
                      {
                        path.Close();
                        break;
                      }

                          
                        //      seg = new SvgPathSegMovetoAbs(coords[i], coords[i + 1]);
                      //    }
                      //    else
                      //    {
                      //      seg = new SvgPathSegLinetoAbs(coords[i], coords[i + 1]);
                      //    }
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'm':
                      //  for (int i = 0; i < length; i += 2)
                      //  {
                      //    if (i == 0)
                      //    {
                      //      seg = new SvgPathSegMovetoRel(coords[i], coords[i + 1]);
                      //    }
                      //    else
                      //    {
                      //      seg = new SvgPathSegLinetoRel(coords[i], coords[i + 1]);
                      //    }
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      #endregion
                      //#region lineto
                      //case 'L':
                      //  for (int i = 0; i < length; i += 2)
                      //  {
                      //    seg = new SvgPathSegLinetoAbs(coords[i], coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'l':
                      //  for (int i = 0; i < length; i += 2)
                      //  {
                      //    seg = new SvgPathSegLinetoRel(coords[i], coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'H':
                      //  for (int i = 0; i < length; i++)
                      //  {
                      //    seg = new SvgPathSegLinetoHorizontalAbs(coords[i]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'h':
                      //  for (int i = 0; i < length; i++)
                      //  {
                      //    seg = new SvgPathSegLinetoHorizontalRel(coords[i]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'V':
                      //  for (int i = 0; i < length; i++)
                      //  {
                      //    seg = new SvgPathSegLinetoVerticalAbs(coords[i]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'v':
                      //  for (int i = 0; i < length; i++)
                      //  {
                      //    seg = new SvgPathSegLinetoVerticalRel(coords[i]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //#endregion
                      //#region beziers
                      //case 'C':
                      //  for (int i = 0; i < length; i += 6)
                      //  {
                      //    seg = new SvgPathSegCurvetoCubicAbs(
                      //        coords[i + 4],
                      //        coords[i + 5],
                      //        coords[i],
                      //        coords[i + 1],
                      //        coords[i + 2],
                      //        coords[i + 3]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'c':
                      //  for (int i = 0; i < length; i += 6)
                      //  {
                      //    seg = new SvgPathSegCurvetoCubicRel(
                      //        coords[i + 4],
                      //        coords[i + 5],
                      //        coords[i],
                      //        coords[i + 1],
                      //        coords[i + 2],
                      //        coords[i + 3]);

                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'S':
                      //  for (int i = 0; i < length; i += 4)
                      //  {
                      //    seg = new SvgPathSegCurvetoCubicSmoothAbs(
                      //        coords[i + 2],
                      //        coords[i + 3],
                      //        coords[i],
                      //        coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 's':
                      //  for (int i = 0; i < length; i += 4)
                      //  {
                      //    seg = new SvgPathSegCurvetoCubicSmoothRel(
                      //        coords[i + 2],
                      //        coords[i + 3],
                      //        coords[i],
                      //        coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'Q':
                      //  for (int i = 0; i < length; i += 4)
                      //  {
                      //    seg = new SvgPathSegCurvetoQuadraticAbs(
                      //        coords[i + 2],
                      //        coords[i + 3],
                      //        coords[i],
                      //        coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'q':
                      //  for (int i = 0; i < length; i += 4)
                      //  {
                      //    seg = new SvgPathSegCurvetoQuadraticRel(
                      //        coords[i + 2],
                      //        coords[i + 3],
                      //        coords[i],
                      //        coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 'T':
                      //  for (int i = 0; i < length; i += 2)
                      //  {
                      //    seg = new SvgPathSegCurvetoQuadraticSmoothAbs(
                      //        coords[i], coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //case 't':
                      //  for (int i = 0; i < length; i += 2)
                      //  {
                      //    seg = new SvgPathSegCurvetoQuadraticSmoothRel(
                      //        coords[i], coords[i + 1]);
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //#endregion
                      //#region arcs
                      //case 'A':
                      //case 'a':
                      //  for (int i = 0; i < length; i += 7)
                      //  {
                      //    if (cmd == 'A')
                      //    {
                      //      seg = new SvgPathSegArcAbs(
                      //          coords[i + 5],
                      //          coords[i + 6],
                      //          coords[i],
                      //          coords[i + 1],
                      //          coords[i + 2],
                      //          (coords[i + 3] != 0),
                      //          (coords[i + 4] != 0));
                      //    }
                      //    else
                      //    {
                      //      seg = new SvgPathSegArcRel(
                      //          coords[i + 5],
                      //          coords[i + 6],
                      //          coords[i],
                      //          coords[i + 1],
                      //          coords[i + 2],
                      //          (coords[i + 3] != 0),
                      //          (coords[i + 4] != 0));
                      //    }
                      //    AppendItem(seg);
                      //  }
                      //  break;
                      //#endregion
                      //#region close
                      //case 'z':
                      //case 'Z':
                      //  seg = new SvgPathSegClosePath();
                      //  AppendItem(seg);
                      //  break;
                      //#endregion
                      #region Unknown path command
                      default:
                        throw new NotSupportedException(String.Format("Unknown path command - ({0})", operation.OriginalValue));
                      #endregion
                    }
                }
            }

      return path;
    }


    private double[] getCoords(String segment)
    {
      double[] coords = new double[0];

      segment = segment.Substring(1);
      segment = segment.Trim();
      segment = segment.Trim(new char[] { ',' });

      if (segment.Length > 0)
      {
        string[] sCoords = coordSplit.Split(segment);

        coords = new double[sCoords.Length];
        for (int i = 0; i < sCoords.Length; i++)
        {
          coords[i] = SvgNumber.ParseNumber(sCoords[i]);
        }
      }
      return coords;
    }
    
  }
}