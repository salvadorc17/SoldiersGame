using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SoldiersGame;
using SoldierTactics.GameFormats;

namespace SoldierTactics.Engine
{
    /// <summary>
    /// Image store class
    /// </summary>
    public class Sprite
    {
        public String Name { get; set; }
        public Bitmap Bitmap { get; set; }
        public Texture2D Image {get; set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public int xOrigin { get; set; }
        public int yOrigin { get; set; }

        public Sprite()
        {
        }

        public Sprite(string path) 
        {
            Name = path;

            Stream file = File.Open(path, FileMode.Open);

            if (file == null)
                throw new System.IO.FileNotFoundException("Cannot load image: " + path);

            Image = Texture2D.FromStream(Global.GraphicsDevice, file);
             
            Width = Config.SPRITE_SIZE;
            Height = Config.SPRITE_SIZE;
            xOrigin = 0;
            yOrigin = 0;
        }


        public Sprite(string name, ContentManager content )
        {

            Name = name;
            Image = content.Load<Texture2D>(name);

            Width = Image.Width;
            Height = Image.Height;
            xOrigin = 0;
            yOrigin = 0;



        }

        public Sprite(Texture2D texture)
        {

            Image = texture;
            Width = texture.Width;
            Height = texture.Height;

        }

        public Sprite(string path, int width, int height)
        {
            Name = path;
            Stream file = File.Open(path, FileMode.Open);
            if (file == null)
                throw new System.IO.FileNotFoundException("Cannot load image: " + path);

            Image = Texture2D.FromStream(Global.GraphicsDevice, file);
            
            Width =  width;
            Height = height;
            xOrigin = width / 2;
            yOrigin = height / 2;
        }


        public Sprite(Texture2D image, int width, int height, int xorigin, int yorigin)
        {

            Image = image;
            Width = width;
            Height = height;
            this.xOrigin = xorigin;
            this.yOrigin = yorigin;
        }
  


        public List<Sprite> Split(int widthTile, int heightTile)
        {
            int tileswidth = Width / widthTile;
            int tilesheight = Height / heightTile;
            int cantidadTiles = tileswidth * tilesheight;

            List<Sprite> sprites = new List<Sprite>();

            int tileActual = 0;

            for (int y = 0; y < tilesheight; y++)
            {
                for (int x = 0; x < tileswidth; x++)
                {
                    Texture2D imgtmp = Image;
                    sprites.Add(new Sprite(imgtmp, widthTile, heightTile, x * widthTile, y * heightTile));
                    tileActual++;
                }
            }

            return sprites;

        }
    }
}
