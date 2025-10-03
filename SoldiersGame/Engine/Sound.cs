using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;


namespace SoldierTactics.Engine
{
   

    public class Sound
    {

        public SoundEffect SoundFile;
        public int ID;
        public string Filepath;
        public bool Finished, Loop;

        public Sound(int id, string path, bool loop)
        {
            Stream buffer = File.Open(path, FileMode.Open);

            ID = id;
            Filepath = path;
            Loop = loop;
            Finished = false;


        }

        public void Update()
        {

            if (Loop)
            {

                SoundFile.CreateInstance().Play();

            }

            else
            {


                Loop = false;

                if (!Finished)
                {

                    SoundFile.Dispose();
                    Finished = true;


                }

            }


        }


    }


         

}
