
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SoldierTactics;
using SoldierTactics.GameFormats;

namespace LevelEditor
{
   public static class Utils
    {


        public static Bitmap GetBitmap(WADImage image)
        {
            Bitmap b = new Bitmap((int)image.Width, (int)image.Height);
            for (int h = 0; h < image.Height; ++h)
                for (int w = 0; w < image.Width; ++w)
                {
                    var pix = image.Pixels[h, w];
                    b.SetPixel(w, h, System.Drawing.Color.FromArgb(pix.Opacity, pix.R, pix.G, pix.B));
                }
            return b;
        }

      public static Rectangle GetBounds(Point point)
        {
            Rectangle rect = new Rectangle();

            rect.X = point.X;
            rect.Y = point.Y;

            return rect;
        }


    }
}
