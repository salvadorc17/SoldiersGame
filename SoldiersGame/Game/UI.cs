using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoldiersGame
{
   public class UI
    {
        public Texture2D Cursor, Backpack, Hud, Hud2, Bar, Bar2, 
            Camera, Interrog, Eye, Marco;
        public List<Texture2D> Cursors;


        public UI()
        {
            Cursors = new List<Texture2D>();

           

        }

        public void Load()
        {
            Backpack = ImageManager.ImageFromWADArchive(0, "MOCHILA");

            Camera = ImageManager.ImageFromWADArchive(0, "CAMARA");

            Interrog = ImageManager.ImageFromWADArchive(0, "INTERROG");

            Eye = ImageManager.ImageFromWADArchive(0, "P_OJOAL0");

            Marco = ImageManager.ImageFromWADArchive(0, "MARC0000");

            Hud =  ImageManager.ImageFromWADArchive(2, "BARRA01");

            Hud2 = ImageManager.ImageFromWADArchive(2, "BARRA02");

            Bar = ImageManager.ImageFromWADArchive(2, "BARS0000");

            Bar2 = ImageManager.ImageFromWADArchive(2, "BARS0001");

        }

    }
}
