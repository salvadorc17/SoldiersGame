
using System;
using System.Collections.Generic;
using SoldierTactics.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SoldierTactics.Game
{
   public class Vehicle : Entity
    {

        public string Driver;
        public int State;
        public Direction Direction;
        public bool Action;
        public Animation[] Animations;
        public Animation CurrentAnimation, Effect;
        public Rectangle Bounds;
        public Vector Position;
        public Point[] ViewField;
        public Sound ActionSound, MoveSound; 

        public Vehicle(int id, string name, int x, int y, ContentManager content)
             {
                 ID = id;
                 Name = name;
                 X = x;
                 Y = y;
                 State = 0;
                 Animations = new Animation[10];
                 Action = false;
                 //ActionSound = new Sound(1, SoundEngine.tankSound, false);
                 ViewField = new Point[3];
                 Load(content);

             }

       public void Load(ContentManager content)
            {

                if (Animations != null)
                     {

                         CurrentAnimation = Animations[1];
                         Effect = Animations[10];
                         Direction = Direction.Right;
                         Position = new Vector(X, Y);
                         Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
                     }


            }

       

       public void Update(GameTime gameTime)
            {

                if (CurrentAnimation != null)
                     {

                    
                    Position.Update(X, Y);
                    Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
                    CurrentAnimation.Update(gameTime);

                    }

            }

       public void Draw()
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Draw(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);

        }
    }
}
