using System;
using System.Collections.Generic;
using SoldierTactics.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;
using SoldiersGame;

namespace SoldierTactics.Game
{
    public class Player : Entity
    {
        
        public string Class;
        public int State, Anim;
        public Direction Direction;
        public bool Action, LeftPress, RightPress, UpPress, DownPress;
        public SpriteTable SpriteTable, CaraTable;
        public List<Animation> Animations, ExtraAnimations;
        public Animation CurrentAnimation, FaceAnimation, Effect;
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

                Animations = new List<Animation>();
                ExtraAnimations = new List<Animation>();

                XmlSerializer ax = new XmlSerializer(typeof(SpriteTable));

                Anim = 0;
                Action = false;
                FieldView = new FieldView(fov, true);


                using (Stream file = TitleContainer.OpenStream("Content/Sprites/" + name + ".xml"))
                {
                    SpriteTable = (SpriteTable)ax.Deserialize(file);

                }

                using (Stream file = TitleContainer.OpenStream("Content/Interface/" + name + ".xml"))
                {
                    CaraTable = (SpriteTable)ax.Deserialize(file);

                }
            

                Load(content);

            }



        public void Load(ContentManager content)
            {

                    List<Sprite> Sprites;
                    


                    for (int i = 0; i < SpriteTable.Sequences.Count; i++)
                    {
                        if (SpriteTable.Sequences[i].Frames.Count > 0)
                        {
                                Sprites = new List<Sprite>();

                                foreach (Frame frame in SpriteTable.Sequences[i].Frames)
                                    Sprites.Add(new Sprite(ImageManager.ImageFromWADArchive(ID, frame.Name)));

                                Animations.Add(new Animation(Sprites, SpriteTable.Sequences[i].Speed,
                                SpriteTable.Sequences[i].Frames.Count, SpriteTable.Sequences[i].Type == 1 ? true : false));
                         }


                       }

            for (int i = 0; i < CaraTable.Sequences.Count; i++)
            {
                if (CaraTable.Sequences[i].Frames.Count > 0)
                {
                    Sprites = new List<Sprite>();

                    foreach (Frame frame in CaraTable.Sequences[i].Frames)
                        Sprites.Add(new Sprite(ImageManager.ImageFromWADArchive(ID + 1, frame.Name)));

                    ExtraAnimations.Add(new Animation(Sprites, CaraTable.Sequences[i].Speed,
                    CaraTable.Sequences[i].Frames.Count, CaraTable.Sequences[i].Type == 1 ? true : false));
                }


            }

            FaceAnimation = ExtraAnimations[0];
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
                        if (RightPress == true)
                        {

                            //P1 Move right
                            CurrentAnimation = Animations[1];
                            CurrentAnimation.IsFliped = false;
                            X += 3;

                        }
                        else if (LeftPress == true)
                        {

                            //P1 Move left
                            CurrentAnimation = Animations[1];
                            CurrentAnimation.IsFliped = true;
                            X -= 3;

                        }
                        else if (UpPress == true)
                        {

                            //P1 Move up
                            CurrentAnimation = Animations[2];
                            CurrentAnimation.IsFliped = false;
                            Y -= 3;

                        }
                        else if (DownPress == true)
                        {

                            //P1 Move down
                            CurrentAnimation = Animations[3];
                            CurrentAnimation.IsFliped = false;
                            Y += 3;

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
                CurrentAnimation.Draw( X, Y, CurrentAnimation.CurrentSprite.Width, CurrentAnimation.CurrentSprite.Height);


            if (Action == true && Effect.isEnabled)
                Effect.Draw( X, Y, Effect.FrameWidth, Effect.FrameHeight);

        }

        public void PostDraw(int x, int y)
        {
            if (FaceAnimation != null)
                FaceAnimation.Draw(x, y, FaceAnimation.CurrentSprite.Width, FaceAnimation.CurrentSprite.Height);

        }

    }

}
