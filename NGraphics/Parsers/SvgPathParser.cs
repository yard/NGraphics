﻿using System;
using System.Collections.Generic;
using System.Linq;
using NGraphics.Custom.Models;
using NGraphics.Custom.Models.Elements;
using NGraphics.Custom.Models.Segments;

namespace NGraphics.Custom.Parsers
{
    public static class PointFExtensions
    {
        public static string ToSvgString(this Point p)
        {
            return p.X + " " + p.Y;
        }
    }

    public class SvgPathParser
    {
        /// <summary>
        ///     Parses the specified string into a collection of path segments.
        /// </summary>
        /// <param name="path">A <see cref="string" /> containing path data.</param>
        public static void Parse(Path path, string pathString)
        {
            if (string.IsNullOrEmpty(pathString))
            {
                throw new ArgumentNullException("pathString");
            }

            var segments = new SvgPathSegmentList();

            try
            {
                char command;
                bool isRelative;

                foreach (var commandSet in SplitCommands(pathString.TrimEnd(null)))
                {
                    command = commandSet[0];
                    isRelative = char.IsLower(command);
                    // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

                    CreatePathSegment(command, segments, new CoordinateParser(commandSet.Trim()), isRelative);
                }

                foreach (var segment in segments)
                {
                    segment.AddToPath(path);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error parsing path \"{0}\": {1}", path, exc.Message));
            }

            //return segments;
        }

        public static void ParseFromPolygon(Path path, string polygonString) {
            if (string.IsNullOrEmpty(polygonString)) {
                throw new ArgumentNullException("polygonString");
            }

            var segments = new SvgPathSegmentList();

            try {
                string[] coordinates = polygonString.Split(' ');

                for (int i = 0; i < coordinates.Length / 2; i++) {
                    CreatePathSegment(i == 0 ? 'M' : 'L', segments, new CoordinateParser(coordinates[2 * i] + " " + coordinates[2 * i + 1]), false);
                }
                segments.Add(new SvgClosePathSegment());

                foreach (var segment in segments) {
                    segment.AddToPath(path);
                }
            } catch (Exception exc) {
                throw new Exception(string.Format("Error parsing polygon \"{0}\": {1}", path, exc.Message));
            }
        }

        private static void CreatePathSegment(char command, SvgPathSegmentList segments, CoordinateParser parser,
            bool isRelative)
        {
            var coords = new float[6];

            switch (command)
            {
                case 'm': // relative moveto
                case 'M': // moveto
                    if (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgMoveToSegment(ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'a':
                case 'A':
                    bool size;
                    bool sweep;

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetBool(out size) &&
                           parser.TryGetBool(out sweep) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]))
                    {
                        // A|a rx ry x-axis-rotation large-arc-flag sweep-flag x y
                        segments.Add(new SvgArcSegment(segments.Last.End, coords[0], coords[1], coords[2],
                            (size ? SvgArcSize.Large : SvgArcSize.Small),
                            (sweep ? SvgArcSweep.Positive : SvgArcSweep.Negative),
                            ToAbsolute(coords[3], coords[4], segments, isRelative)));
                    }
                    break;
                case 'l': // relative lineto
                case 'L': // lineto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'H': // horizontal lineto
                case 'h': // relative horizontal lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], segments.Last.End.Y, segments, isRelative, false)));
                    }
                    break;
                case 'V': // vertical lineto
                case 'v': // relative vertical lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(segments.Last.End.X, coords[0], segments, false, isRelative)));
                    }
                    break;
                case 'Q': // curveto
                case 'q': // relative curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'T': // shorthand/smooth curveto
                case 't': // relative shorthand/smooth curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        var lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;

                        var controlPoint = lastQuadCurve != null
                            ? Reflect(lastQuadCurve.ControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'C': // curveto
                case 'c': // relative curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]) && parser.TryGetFloat(out coords[5]))
                    {
                        segments.Add(new SvgCubicCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative),
                            ToAbsolute(coords[4], coords[5], segments, isRelative)));
                    }
                    break;
                case 'S': // shorthand/smooth curveto
                case 's': // relative shorthand/smooth curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        var lastCubicCurve = segments.Last as SvgCubicCurveSegment;

                        var controlPoint = lastCubicCurve != null
                            ? Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgCubicCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'Z': // closepath
                case 'z': // relative closepath
                    segments.Add(new SvgClosePathSegment());
                    break;
            }
        }

        private static Point Reflect(Point point, Point mirror)
        {
            double x, y, dx, dy;
            dx = Math.Abs(mirror.X - point.X);
            dy = Math.Abs(mirror.Y - point.Y);

            if (mirror.X >= point.X)
            {
                x = mirror.X + dx;
            }
            else
            {
                x = mirror.X - dx;
            }
            if (mirror.Y >= point.Y)
            {
                y = mirror.Y + dy;
            }
            else
            {
                y = mirror.Y - dy;
            }

            return new Point(x, y);
        }

        /// <summary>
        ///     Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeBoth">
        ///     <b>true</b> if <paramref name="x" /> and <paramref name="y" /> contains relative
        ///     coordinate values, otherwise <b>false</b>.
        /// </param>
        /// <returns><see cref="PointF" /> that contains absolute coordinates.</returns>
        private static Point ToAbsolute(double x, double y, SvgPathSegmentList segments, bool isRelativeBoth)
        {
            return ToAbsolute(x, y, segments, isRelativeBoth, isRelativeBoth);
        }

        /// <summary>
        ///     Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeX">
        ///     <b>true</b> if <paramref name="x" /> contains relative coordinate value, otherwise
        ///     <b>false</b>.
        /// </param>
        /// <param name="isRelativeY">
        ///     <b>true</b> if <paramref name="y" /> contains relative coordinate value, otherwise
        ///     <b>false</b>.
        /// </param>
        /// <returns><see cref="PointF" /> that contains absolute coordinates.</returns>
        private static Point ToAbsolute(double x, double y, SvgPathSegmentList segments, bool isRelativeX,
            bool isRelativeY)
        {
            var point = new Point(x, y);

            if ((isRelativeX || isRelativeY) && segments.Count > 0)
            {
                var lastSegment = segments.Last;

                // if the last element is a SvgClosePathSegment the position of the previous element should be used because the position of SvgClosePathSegment is 0,0
                if (lastSegment is SvgClosePathSegment)
                    lastSegment = segments.Reverse().OfType<SvgMoveToSegment>().First();

                if (isRelativeX)
                {
                    point.X += lastSegment.End.X;
                }

                if (isRelativeY)
                {
                    point.Y += lastSegment.End.Y;
                }
            }

            return point;
        }

        private static IEnumerable<string> SplitCommands(string path)
        {
            var commandStart = 0;

            for (var i = 0; i < path.Length; i++)
            {
                string command;
                if (char.IsLetter(path[i]) && path[i] != 'e') //e is used in scientific notiation. but not svg path
                {
                    command = path.Substring(commandStart, i - commandStart).Trim();
                    commandStart = i;

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }

                    if (path.Length == i + 1)
                    {
                        yield return path[i].ToString();
                    }
                }
                else if (path.Length == i + 1)
                {
                    command = path.Substring(commandStart, i - commandStart + 1).Trim();

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }
                }
            }
        }
    }
}