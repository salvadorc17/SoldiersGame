using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoldierTactics;
using SoldierTactics.GameFormats;


namespace SoldiersGame
{
    public static class ImageManager
    {

        public static GraphicsDevice GraphicsDevice;

        public static List<WAD> WADImages;


        public static void InitWADs(GraphicsDevice graphicsDevice)
        {


            GraphicsDevice = graphicsDevice;

            WADImages = new List<WAD>();
            

        }


        public static void LoadWad(int id, string name)
        {
        
            //System wad
            if (id == 0)
            {

                WADImages.Add(new WAD(Config.SYSDIR + Path.DirectorySeparatorChar + "CARAS" +
                Path.DirectorySeparatorChar + name));

            }
            //Map wad
            else if (id == 1)
            {

                WADImages.Add(new WAD(Config.MAPDIR + Path.DirectorySeparatorChar + name + ".wad"));


            }

        }

        public static Texture2D LoadTexture(GraphicsDevice gd, string path)
        {
            Texture2D texture = new Texture2D(gd, 0, 0);

            if (File.Exists(path))
            {

                using (Stream Stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {


                    texture = Texture2D.FromStream(gd, Stream);

                }


            }


            return texture;



        }

        public static Texture2D ImageFromWADArchive(int id, int value)
        {

            Texture2D texture = new Texture2D(GraphicsDevice, 0, 0);

            if (WADImages.Count > 0)
            {


                WAD wad = WADImages[id];
                WADImage img = wad.Images[value];

                if ( img.RawDataSize > 0)
                     texture.SetData<byte>(img.RawData);

            }


            return texture;

        }

        public static Texture2D ImageFromWADArchive(int id, string name)
       {

           Texture2D texture = new Texture2D(GraphicsDevice, 0, 0);

            if (WADImages.Count > 0)
            {


                WAD wad = WADImages[id];
                foreach (WADImage img in wad.Images)
                    if (img.Name == name && img.RawDataSize > 0)
                        texture.SetData<byte>(img.RawData);

            }


           return texture;

       }
    }

}
