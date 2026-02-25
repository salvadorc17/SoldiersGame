using System;
using System.Windows.Forms;
using System.IO;
using SoldierTactics;

namespace SoldiersGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (CheckGameFiles())
                using (var game = new SoldiersGame())
                    game.Run();
            else
                MessageBox.Show("Game files are requiered to run this program");

        }


        public static bool CheckGameFiles()
        {
            bool Exists = false;

            if (Directory.Exists(Config.DATOSDIR)
                && Directory.Exists(Config.SYSDIR)
                && Directory.Exists(Config.MAPDIR )
                && Directory.Exists(Config.SOUNDDIR)
            && Directory.Exists(Config.MUSICDIR))
                Exists = true;

            return Exists;


        }

    }


#endif
}
