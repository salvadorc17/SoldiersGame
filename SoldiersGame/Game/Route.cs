using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoldierTactics.Game
{
   public class Route
    {
       public int X, Y;
       public int XF, YF;
       public RouteType Type;
       public bool Enabled;

      public Route(int x, int y, int xf, int yf, RouteType type)
        {
            X = x;
            Y = y;
            XF = xf;
            YF = yf;
            Type = type;

        }

       public void Enable(bool enable)
        {

            Enabled = enable;

       
        }

    }

   public enum RouteType
   {
       None = 0,
       Horizontal = 1,
       Vertical = 2

   }

    public enum Direction
    {
        None,
        Right,
        Left,
        Up,
        Down

    }

}
