using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SoldierTactics.Game
{
   public class FieldView
    {

       public bool Enabled;
       public int Angle;
       public Point[] Points;

       public FieldView(int angle, bool enabled)
       {
           Points = new Point[3];
           Angle = angle;
           Enabled = enabled;



       }

       public void SetPoints(int x, int y)
       {

           int valx = 0;

           int valy = 0;

           Points[0] = new Point(x, y);

           if (x > 0)
               valx = x * (int)Math.Sin(Angle);

           if (y > 0)
               valy = y * (int)Math.Sin(Angle);


           Points[1] = new Point(x + valx, y - valy);

           Points[2] = new Point(x + valx, y + valy);
       
       
       }


       public void Draw()
       { 
       
       
     

       }



    }
}
