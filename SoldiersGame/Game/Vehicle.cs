
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

                         Animations[0] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Dead", content), 1.0F, 1, true);
                         Animations[1] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Right", content), 1.0F, 1, true);
                         Animations[2] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Left", content), 1.0F, 1, true);
                         Animations[3] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Down", content), 1.0F, 1, true);
                         Animations[4] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Up", content), 1.0F, 1, true);
                         CurrentAnimation = Animations[1];
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
