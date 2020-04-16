using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoldierTactics.GameFormats
{
    /// <summary>
    /// Map file
    ///
    /// Format:
    /// {
    /// MAPDIMXY 1453,2450  // Map Dimension (width, height)
    /// MAPTABPOLYS
    /// {
    ///   ... // Polygons
    /// }
    ///
    ///
    /// </summary>
    public class VOL
    {
        public string File { get; set; }

        /// <summary>Determined by content.</summary>
        public int XMin { get; private set; }
        /// <summary>Determined by content.</summary>
        public int XMax { get; private set; }
        /// <summary>Determined by content.</summary>
        public int YMin { get; private set; }
        /// <summary>Determined by content.</summary>
        public int YMax { get; private set; }

        /// <summary>Width that was used in the VOL file (MAPDIMXY).</summary>
        public int Width { get; private set; }
        /// <summary>Height that was used in the VOL file (MAPDIMXY).</summary>
        public int Height { get; private set; }

        public List<MapPolygon> Polys = new List<MapPolygon>();

        private VOL() { }

        public VOL(string file)
        {
            if (!System.IO.File.Exists(file))
                throw new FileNotFoundException("File '" + file + "' not found");

            File = file;

            using (var reader = new StreamReader(file))
            {
                Init(reader);
            }
        }

        /// <summary>
        /// Might be useful for local memory stream.
        /// </summary>
        /// <param name="reader"></param>
        public VOL(StreamReader reader)
        {
            Init(reader);
        }

        void Init(System.IO.StreamReader reader)
        {
            int linecnt = 0;
            // Search for start tag
            while (!reader.EndOfStream)
            {
                var line = GetLine(reader, ref linecnt);
                if (line.StartsWith("{")) break;
            }
            if (reader.EndOfStream) throw new Exception("Incomplete file!");

            // Search for MAPDIMXY
            while (!reader.EndOfStream)
            {
                var line = GetLine(reader, ref linecnt);
                if (line.StartsWith("MAPDIMXY"))
                {
                    var split = GetParameters(line, linecnt);
                    if (split.Length < 2) throw new Exception("Invalid MAPDIMXY definition (line " + linecnt + ")");
                    int width, height;
                    if (!int.TryParse(split[0], out width)) throw new Exception("Invalid MAPDIMXY definition (line " + linecnt + ")");
                    if (!int.TryParse(split[1], out height)) throw new Exception("Invalid MAPDIMXY definition (line " + linecnt + ")");
                    Width = width;
                    Height = height;
                    break;
                }
            }
            if (reader.EndOfStream) throw new Exception("Incomplete file!");

            // Search for MAPTABPOLYS
            while (!reader.EndOfStream)
            {
                var line = GetLine(reader, ref linecnt);
                if (line.StartsWith("MAPTABPOLYS"))
                {
                    // Check if line contains the opening '{'
                    if (!line.Contains('{'))
                    {
                        while (!reader.EndOfStream)
                        {
                            line = GetLine(reader, ref linecnt);
                            if (line.StartsWith("{"))
                                break;
                        }
                    }
                    break;
                }
            }
            if (reader.EndOfStream) throw new Exception("Incomplete file!");

            MapPolygon poly = null;
            int tilesToGo = 0;
            int verticesToGo = 0; // Vertice = Point

            // Read all the polygons
            while (!reader.EndOfStream)
            {
                var line = GetLine(reader, ref linecnt);

                if (line.StartsWith("}"))
                    break; // Thats it

                // New poly starts
                if (line.StartsWith("POLY"))
                {
                    if (tilesToGo > 0) throw new Exception("Missing tile definition for previous polygon (line " + linecnt + ")");
                    if (verticesToGo > 0) throw new Exception("Missing POINT definition for previous polygon (line " + linecnt + ")");

                    // TODO: Handle commas within string parameters
                    var spl = GetParameters(line, linecnt);
                    if (spl.Length < 7) throw new Exception("Invalid POLY definition (not enough parameters) (line " + linecnt + ")");

                    int cx, cy, cz, alt;
                    int altOff = 0;
                    int zoom = 0;
                    MapPolygonType type = MapPolygonType.Default;
                    if (!int.TryParse(spl[1], out cx)) throw new Exception("Invalid POLY center X (line " + linecnt + ")");
                    if (!int.TryParse(spl[2], out cy)) throw new Exception("Invalid POLY center Y (line " + linecnt + ")");
                    if (!int.TryParse(spl[3], out cz)) throw new Exception("Invalid POLY center Z (line " + linecnt + ")");
                    if (line.StartsWith("POLYRAMPA"))
                    {
                        if (spl.Length < 8) throw new Exception("Invalid POLYRAMPA definition (not enough parameters) (line " + linecnt + ")");
                        if (!int.TryParse(spl[4], out alt)) throw new Exception("Invalid POLY altitude (line " + linecnt + ")");
                        if (!int.TryParse(spl[5], out altOff)) throw new Exception("Invalid POLY altitude offset (line " + linecnt + ")");
                        if (!int.TryParse(spl[6], out verticesToGo)) throw new Exception("Invalid POLY vertice count (line " + linecnt + ")");
                        if (!int.TryParse(spl[7], out tilesToGo)) throw new Exception("Invalid POLY tiles count (line " + linecnt + ")");
                        type = MapPolygonType.Ramp;
                    }
                    else if (line.StartsWith("POLYZOOM"))
                    {
                        if (spl.Length < 8) throw new Exception("Invalid POLYZOOM definition (not enough parameters) (line " + linecnt + ")");
                        if (!int.TryParse(spl[4], out zoom)) throw new Exception("Invalid POLY zoom (line " + linecnt + ")");
                        if (!int.TryParse(spl[5], out alt)) throw new Exception("Invalid POLY altitude (line " + linecnt + ")");
                        if (!int.TryParse(spl[6], out verticesToGo)) throw new Exception("Invalid POLY vertice count (line " + linecnt + ")");
                        if (!int.TryParse(spl[7], out tilesToGo)) throw new Exception("Invalid POLY tiles count (line " + linecnt + ")");
                        type = MapPolygonType.Zoom;
                    }
                    else
                    {
                        if (!int.TryParse(spl[4], out alt)) throw new Exception("Invalid POLY altitude (line " + linecnt + ")");
                        if (!int.TryParse(spl[5], out verticesToGo)) throw new Exception("Invalid POLY vertice count (line " + linecnt + ")");
                        if (!int.TryParse(spl[6], out tilesToGo)) throw new Exception("Invalid POLY tiles count (line " + linecnt + ")");
                    }
                    poly = new MapPolygon(spl[0].Trim().Trim('"'), cx, cy, cz, alt) { AltitudeOffset = altOff, Zoom = zoom, Type = type };
                    Polys.Add(poly);
                }
                else if (line.StartsWith("RADIO"))
                {
                    if (poly == null) throw new Exception("RADIO definition has no poly definition (line " + linecnt + ")");

                    var spl = GetParameters(line, linecnt);
                    if (spl.Length < 1) throw new Exception("RADIO value missing (line " + linecnt + ")");

                    int radio;
                    if (!int.TryParse(spl[0], out radio)) throw new Exception("Invalid RADIO value (line " + linecnt + ")");
                    poly.Radio = radio;
                }
                else if (line.StartsWith("EXTRAINFO"))
                {
                    if (poly == null) throw new Exception("EXTRAINFO definition has no poly definition (line " + linecnt + ")");

                    byte b0, b1, b2, b3, b4, b5, b6, b7;

                    var spl = GetParameters(line, linecnt);
                    if (spl.Length < 8) throw new Exception("Invalid EXTRAINFO definition (missing bytes) (line " + linecnt + ")");

                    if (!byte.TryParse(spl[0], out b0)) throw new Exception("Invalid byte0 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[1], out b1)) throw new Exception("Invalid byte1 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[2], out b2)) throw new Exception("Invalid byte2 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[3], out b3)) throw new Exception("Invalid byte3 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[4], out b4)) throw new Exception("Invalid byte4 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[5], out b5)) throw new Exception("Invalid byte5 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[6], out b6)) throw new Exception("Invalid byte6 value (line " + linecnt + ")");
                    if (!byte.TryParse(spl[7], out b7)) throw new Exception("Invalid byte7 value (line " + linecnt + ")");

                    poly.ExtraInfo[0] = b0;
                    poly.ExtraInfo[1] = b1;
                    poly.ExtraInfo[2] = b2;
                    poly.ExtraInfo[3] = b3;
                    poly.ExtraInfo[4] = b4;
                    poly.ExtraInfo[5] = b5;
                    poly.ExtraInfo[6] = b6;
                    poly.ExtraInfo[7] = b7;
                }
                else if (line.StartsWith("TILE"))
                {
                    if (--tilesToGo < 0) throw new Exception("TILE definition was not expected (line " + linecnt + ")");
                    if (poly == null) throw new Exception("TILE definition has no poly definition (line " + linecnt + ")");

                    var spl = GetParameters(line, linecnt);
                    if (spl.Length < 9) throw new Exception("Invalid TILE definition (missing parameters) (line " + linecnt + ")");

                    int x, y, w, h, ho, vo, b;
                    if (!int.TryParse(spl[0], out x)) throw new Exception("Invalid x position value (line " + linecnt + ")");
                    if (!int.TryParse(spl[1], out y)) throw new Exception("Invalid y position value (line " + linecnt + ")");
                    if (!int.TryParse(spl[2], out w)) throw new Exception("Invalid width value (line " + linecnt + ")");
                    if (!int.TryParse(spl[3], out h)) throw new Exception("Invalid height value (line " + linecnt + ")");
                    if (!int.TryParse(spl[4], out ho)) throw new Exception("Invalid brightness value (line " + linecnt + ")");
                    if (!int.TryParse(spl[5], out vo)) throw new Exception("Invalid sprite value (line " + linecnt + ")");
                    if (!int.TryParse(spl[6], out b)) throw new Exception("Invalid x position value (line " + linecnt + ")");

                    var tile = new MapTile
                    {
                        Position = new Point(x, y),
                        Brightness = b,
                        Height = h,
                        Width = w,
                        Offset = new Point(ho, vo),
                        SpriteName = spl[7].Trim('"'),
                    };

                    // Transformation:
                    // "XYL"
                    var transStr = spl[8].Trim('\"');
                    if (transStr.Length != 3) throw new Exception("Invalid transformation (line " + linecnt + ")");

                    // TODO: Are there other combinations?
                    if (transStr[0] == 'X')
                        tile.Transformation |= MapTileTransformation.MirrorX;
                    if (transStr[1] == 'Y')
                        tile.Transformation |= MapTileTransformation.FlipY;
                    if (transStr[2] == 'L')
                        tile.Transformation |= MapTileTransformation.LightOrExplosion;

                    poly.Tiles.Add(tile);

                }
                else if (line.StartsWith("POINT"))
                {
                    if (--verticesToGo < 0) throw new Exception("POINT definition was not expected (line " + linecnt + ")");
                    if (poly == null) throw new Exception("POINT definition has no poly definition (line " + linecnt + ")");

                    var spl = GetParameters(line, linecnt);
                    if (spl.Length < 2) throw new Exception("Invalid POINT definition (line " + linecnt + ")");

                    int x, y;
                    if (!int.TryParse(spl[0], out x)) throw new Exception("Invalid x value (line " + linecnt + ")");
                    if (!int.TryParse(spl[1], out y)) throw new Exception("Invalid y value (line " + linecnt + ")");

                    poly.Vertices.Add(new Point(x, y));
                }
            }

            if (tilesToGo > 0) throw new Exception("Missing tile definition for previous polygon (line " + linecnt + ")");
            if (verticesToGo > 0) throw new Exception("Missing POINT definition for previous polygon (line " + linecnt + ")");

            // Determine real borders of map:
            XMin = 0;
            XMax = Width;
            YMin = 0;
            YMax = Height;
            //foreach (var p in Polys)
            //{
            //    var x0 = p.Center.X;
            //    var y0 = p.Center.Y;
            //    AdjustLimits(x0, y0);
            //    foreach (var t in p.Tiles)
            //    {
            //        // TODO: Not sure if correct:
            //        //AdjustLimits(x0 + t.Position.X, y0 + t.Position.Y);
            //        //AdjustLimits(x0 + t.Position.X + t.Width, y0 + t.Position.Y + t.Height);
            //    }
            //    // For some reason, vertices y must be divided by 1.6
            //    int f1 = 10;
            //    int f2 = 16;
            //    foreach (var v in p.Vertices)
            //    {
            //        AdjustLimits(x0 + v.X, f1*(y0 + v.Y)/f2);
            //    }
            //}


            // There should be a final '}' to close the definition
            while (!reader.EndOfStream)
            {
                var line = GetLine(reader, ref linecnt);

                if (line.StartsWith("}"))
                {
                    if (tilesToGo > 0) throw new Exception("Missing TILE definition for previous polygon (line " + linecnt + ")");
                    if (verticesToGo > 0) throw new Exception("Missing POINT definition for previous polygon (line " + linecnt + ")");
                    return;
                }
            }
            throw new Exception("Closing '}' not found at the end.");

        }

        void AdjustLimits(int x, int y)
        {
            if (x < XMin) XMin = x;
            if (x > XMax) XMax = x;
            if (y < YMin) YMin = y;
            if (y > YMax) YMax = y;
        }

        string[] GetParameters(string line, int linecnt)
        {
            List<string> list = new List<string>();

            // Find first space to split from
            int index = line.IndexOf(' ');

            if (index == -1)
            {
                throw new Exception(line + " definition has no parameters (line " + linecnt + ")");
            }

            line = line.Substring(index).Trim();

            // Now we just have to split the parameters by ','. Ignore whitespaces outside of strings.

            StringBuilder sb = new StringBuilder();
            bool withinString = false;
            foreach (char c in line)
            {
                if (c == '\"')
                {
                    withinString = !withinString;
                }
                else if (char.IsWhiteSpace(c) && !withinString)
                {
                    continue;
                }
                else if (c == ',' && !withinString)
                {
                    list.Add(sb.ToString().Trim());
                    sb.Clear();
                    continue;
                }
                sb.Append(c);
            }
            list.Add(sb.ToString().Trim());

            return list.ToArray();
        }


        // Trims and removes comments
        string GetLine(System.IO.StreamReader reader, ref int linecnt)
        {
            ++linecnt;
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) return "";

            line = line.Trim();

            // Remove comments
            if (line.StartsWith(";")) return "";
            var index = line.IndexOf(';');
            if (index > 0) line = line.Substring(0, index);

            // Remove duplicate whitespaces between parameters
            StringBuilder sb = new StringBuilder();
            bool prevWasWhiteSpace = false;
            bool withinString = false;
            foreach (char c in line)
            {
                bool isWhiteSpace = char.IsWhiteSpace(c);
                if (c == '\"')
                {
                    withinString = !withinString;
                    prevWasWhiteSpace = false;
                }
                else if (isWhiteSpace && !withinString)
                {
                    if (prevWasWhiteSpace) continue;
                    else prevWasWhiteSpace = true;
                }
                else
                    prevWasWhiteSpace = false;
                sb.Append(isWhiteSpace ? ' ' : c);
            }
            return sb.ToString();
        }

    }

}
