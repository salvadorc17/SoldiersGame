using System;
using Microsoft.Xna.Framework;

namespace SoldierTactics.Engine
{
    public class Vector
    {

        public double X, Y;
        private double Lenght;

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector(double x, double y)
            {

                X = x;
                Y = y;

            }

        public void Update(double x, double y)
         {
            if (X > 0)
             X = x;

            if (Y > 0)
             Y = y;
          

         }

        
        public double AngleBetween(Vector vector1, Vector vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.X * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        public Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X,
                              vector1.Y + vector2.Y);
        }

        public Vector Subtract(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X,
                              vector1.Y - vector2.Y);
        }

        public double Multiply(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y + vector1.X * vector2.Y;
        }

        public Vector Divide(Vector vector, double scalar)
        {
            double val = vector.Y / scalar;

            return new Vector(vector.X, val);
        }

        public Point ToPoint(Vector vector, Point point)
        {
            return new Point(point.X + (int)vector.X, point.Y + (int)vector.Y);
        }
 
    }
}
