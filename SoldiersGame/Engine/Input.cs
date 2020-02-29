using System;
using Microsoft.Xna.Framework.Input;


namespace SoldierTactics.Engine
{

    public enum KeyStates
    {
        None = 0,
        Down = 1,
        Up = 2
    }


    public static class Input
    {

        public static KeyStates  KeyState = KeyStates.None;

        public static int CurrentKey = 0;

        public static KeyboardState KeyboardState;

        

       
        public static void GetInput()
        {

            KeyboardState = Keyboard.GetState();



        }

        public static bool IsKeyDown(Keys key)
        {

            if (KeyboardState != null)
                if (KeyboardState.IsKeyDown(key))
                     return true;

            CurrentKey = (int)key;

            return false;

        }

        public static bool IsKeyUp(Keys key)
        {

            if (KeyboardState != null)
                if (KeyboardState.IsKeyUp(key))
                    return true;

            CurrentKey = (int)key;

            return false;

        }


    }


}
