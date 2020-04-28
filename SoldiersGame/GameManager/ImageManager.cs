using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoldierTactics;
using SoldierTactics.GameFormats;
using SoldierTactics.Engine;
using System.Drawing;

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

                WADImages.Add(new WAD(Config.MAPDIR + Path.DirectorySeparatorChar + name));


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

                texture = ImageFromStream(img);


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

        public static Texture2D ImageFromStream(WADImage image)
        {
            Texture2D Image = new Texture2D(GraphicsDevice, (int)image.Width, (int)image.Height);

            Image = Texture2D.FromStream(GraphicsDevice, GetBitmapStream(image));

            return Image;
        }


        public static Stream GetBitmapStream(WADImage img)
        {
            Bitmap Bmp = GetBitmap(img);


            MemoryStream Stream = new MemoryStream();
            //var Stream = File.OpenWrite("image.bmp");
            Bmp.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);


            return Stream;


        }

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


    }

}
