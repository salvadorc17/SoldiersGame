using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SoldiersGame.Engine;

namespace SoldierTactics.Game
{
   public class FieldView
    {

       public bool Enabled;
       public Triangle Triangle;

       public FieldView(float angle, bool enabled)
       {
            Triangle = new Triangle(angle);
           
           Enabled = enabled;



       }

       public void SetPoints(int x, int y)
       {

           int valx = 0;

           int valy = 0;

           Triangle.Points[0] = new Point(x, y);

           if (x > 0)
               valx = x * (int)Math.Cos(Triangle.Angle);

           if (y > 0)
               valy = y * (int)Math.Sin(Triangle.Angle);


            Triangle.Points[1] = new Point(x + valx, y - valy);

            Triangle.Points[2] = new Point(x + valx, y + valy);
       
       
       }


       public void Draw()
       { 
       
       
     

       }



    }
}
