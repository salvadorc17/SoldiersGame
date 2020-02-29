using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierTactics
{
   public static class Config
    {
        public static string GAMEDIR = "";
        public static string BGDIR = GAMEDIR + "Interface/Background/";
        public static string SPRITEDIR = GAMEDIR + "Sprites/";
        public const int FPS = 1000 / 30;
        public const short SCREEN_WIDTH = 640;
        public const short SCREEN_HEIGHT = 480;
        public const short ANIM_SPEED = 24;
        public static short SPRITE_SIZE = 32;
        public const short COLOR_FORMAT = 24;
        public const bool FULL_SCREEN = false;

       


    }
}
