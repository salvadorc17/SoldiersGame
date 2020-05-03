
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoldiersGame.Engine
{
    public class Triangle
    {
        public float Angle;
        public Point[] Points;
        public VertexPositionColor[] Vertices;


        public Triangle(float angle)
        {

            Angle = angle;
            Points = new Point[3];
            Vertices = new VertexPositionColor[3];
        }

    }
}
