using System;
using System.Collections.Generic;
using SoldierTactics.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SoldierTactics.Game
{
    public class Player : Entity
    {
        
        public string Class;
        public int State, Anim;
        public Direction Direction;
        public bool Action, LeftDown, RightDown;
        public Sprite Face, Weapon, Cursor;
        public Animation[] Animations;
        public Animation CurrentAnimation, Effect;
        public Rectangle Bounds;
        public Vector Position;
        public FieldView FieldView;
        public Sound ActionSound; 


        public Player(int id, string name, string clss, int x, int y, int fov, ContentManager content)
            {


                ID = id;
                Name = name;
                Class = clss;
                X = x;
                Y = y;
                Animations = new Animation[10];
                Anim = 0;
                Action = false;
                FieldView = new FieldView(fov, true);

                Load(content);

            }



        public Player(int id, string name, string clss, int x, int y, ContentManager content)
             {
                 ID = id;
                 Name = name;
                 Class = clss;
                 X = x;
                 Y = y;
                 Anim = 0;
                 Animations = new Animation[10];
                 Action = false;
                 //ActionSound = new Sound(1, SoundEngine.shootSound, false);
                 FieldView = new FieldView(45, true);

                 Load(content);

             }

        public void Load(ContentManager content)
            {

          

                    //Face = Image.FromFile(Config.SPRITEDIR + Class + "/face.png");
                    //Weapon = Image.FromFile(Config.SPRITEDIR + Class + "/rifle.png");
                    //Cursor = Image.FromFile(Config.SPRITEDIR + Class + "/aim.png");
                    Animations[0] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Idle", content), 2.0F, 1, true);
                    Animations[1] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Right", content), 1.0F, 8, true);
                    Animations[2] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Left", content), 1.0F, 8, true);
                    Animations[3] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Down", content), 1.0F, 8, true);
                    Animations[4] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Up", content), 1.0F, 8, true);
                    Animations[5] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Shoot", content), 1.0F, 1, true);
                    Animations[6] = new Animation(new Sprite(Config.SPRITEDIR + Name + "/Snipe", content), 1.0F, 4, true);
                    //Effect = new Animation(new Sprite(Config.SPRITEDIR + "/shooteff.png", 100, 17), 1.0F, 5, false);
                  

                    CurrentAnimation = Animations[0];
                    Direction = Direction.Right;
                    Position = new Vector(X, Y);
                    Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);

            }

        public void Update(GameTime gameTime)
            {

                if (CurrentAnimation != null)
                     {

                switch (State)
                {
                    case 0: //Idle
                        //Cursor = MoveCursor;
                        CurrentAnimation = Animations[0];
                        break;

                    case 1: //Move
                        //Cursor = MoveCursor;
                        if (RightDown == true)
                        {

                            //P1 Move right
                            CurrentAnimation = Animations[1];
                            X += 3;

                        }

                        else if (LeftDown == true)
                        {

                            //P1 Move left
                            CurrentAnimation = Animations[2];
                            X-= 3;

                        }

                        break;

                    case 2: //Shoot
                        //Cursor = Player.Cursor;
                        CurrentAnimation = Animations[5];


                        break;

                }



                    Position.Update(X, Y);
                    Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
                    CurrentAnimation.Update(gameTime);

                    FieldView.SetPoints((int)Position.X + CurrentAnimation.FrameWidth, (int)Position.Y);


                     }


                if (Action)
                    {

                    Effect.Update(gameTime);

                    }

            }

        public void Draw()
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Draw( X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);

            if (Action == true && Effect.isEnabled)
                Effect.Draw( X, Y, Effect.FrameWidth, Effect.FrameHeight);

        }




    }

}
