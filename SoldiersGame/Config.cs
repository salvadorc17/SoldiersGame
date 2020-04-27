using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SoldierTactics
{
   public static class Config
    {
        public static string GAMEDIR = "Content";
        public static string DATOSDIR = "DATOS" + Path.DirectorySeparatorChar + "RECURSOS";
        public static string SYSDIR = "DATOS" + Path.DirectorySeparatorChar + "RECURSOS" + Path.DirectorySeparatorChar +
            "BMPS" + Path.DirectorySeparatorChar + "SYSTEM";
        public static string MAPDIR = "DATOS" + Path.DirectorySeparatorChar + "RECURSOS" + Path.DirectorySeparatorChar +
            "BMPS" + Path.DirectorySeparatorChar + "MAP";

        public static string MUSICDIR = "DATOS" + Path.DirectorySeparatorChar + "MUSICA";
        public static string SOUNDDIR = DATOSDIR + Path.DirectorySeparatorChar + "SONIDO" + Path.DirectorySeparatorChar + "WAVE";
        public static string BGDIR = GAMEDIR + Path.DirectorySeparatorChar + "Textures";
        public static string SPRITEDIR = GAMEDIR + Path.DirectorySeparatorChar + "Sprites";
        public const int FPS = 1000 / 30;
        public const short SCREEN_WIDTH = 640;
        public const short SCREEN_HEIGHT = 480;
        public const short ANIM_SPEED = 24;
        public static short SPRITE_SIZE = 32;
        public const short COLOR_FORMAT = 24;
        public const bool FULL_SCREEN = false;

       


    }
}
