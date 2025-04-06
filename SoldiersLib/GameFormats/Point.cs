using System;


namespace SoldierTactics.GameFormats
{
    public class Point
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int id, int x, int y)
        {
            ID = id;
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

        public Point3D(int id, int x, int y, int z = 0) : base(id, x, y)
        {
            ID = id;
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
