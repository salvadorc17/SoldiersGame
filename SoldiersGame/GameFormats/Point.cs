using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierTactics.GameFormats
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "{" + X + ";" + Y + "}";
        }

    }

    public class Point3D : Point
    {
        public int Z { get; set; }

        public Point3D(int x, int y, int z = 0) : base(x, y)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return "{" + X + ";" + Y + ";" + Z + "}";
        }
    }

}
