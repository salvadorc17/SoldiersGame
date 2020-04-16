using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierTactics.GameFormats
{
    public enum MapPolygonType
    {
        Default,
        Ramp,
        Zoom
    }

    public class MapPolygon
    {
        public string Name { get; set; }
        public Point3D Center { get; set; }
        public MapPolygonType Type = MapPolygonType.Default;
        public int Altitude { get; set; }
        public int Radio { get; set; }
        public byte[] ExtraInfo = new byte[8];
        public List<MapTile> Tiles = new List<MapTile>();
        public List<Point> Vertices = new List<Point>();

        /// <summary>
        /// Only used when type is Ramp
        /// </summary>
        public int AltitudeOffset { get; set; }

        /// <summary>
        /// Only used when type is Zoom
        /// </summary>
        public int Zoom { get; set; }

        public MapPolygon(string name, int centerX, int centerY, int centerZ, int altitude)
        {
            Name = name;
            Center = new Point3D(centerX, centerY, centerZ);
            Altitude = altitude;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
