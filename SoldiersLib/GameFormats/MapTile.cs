using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierTactics.GameFormats
{
    [Flags]
    public enum MapTileTransformation
    {
        None = 0,
        MirrorX = 1,
        FlipY = 2,
        LightOrExplosion = 4,
    }

    public class MapTile
    {
        public Point2D Position { get; set; }
        public Point2D Offset { get; set; }
        public int Brightness { get; set; }
        public string SpriteName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public MapTileTransformation Transformation { get; set; }

        public override string ToString()
        {
            return SpriteName;
        }
    }
}
