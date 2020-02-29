using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SoldiersGame
{
    public static class ImageManager
    {

        public static Texture2D LoadTexture(GraphicsDevice gd, string path)
        {
            Texture2D texture = new Texture2D(gd, 10, 10);

            if (File.Exists(path))
            {

                using (Stream Stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {


                    texture = Texture2D.FromStream(gd, Stream);

                }


            }


            return texture;



        }

    }

}
