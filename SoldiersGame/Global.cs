using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SoldiersGame
{
    public static class Global
    {

        public static GraphicsDevice GraphicsDevice;
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static SoldiersGame Game;
        public static double ScreenWidth, ScaleWidth;
        public static double ScreenHeight, ScaleHeight;
        public static bool DebugMode;

    }
}
