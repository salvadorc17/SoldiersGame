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
        public bool Action, LeftDown, RightDown;
        public SpriteTable SpriteTable;
        public Sprite Face, Weapon, Cursor;
        public List<Animation> Animations;
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

                Animations = new List<Animation>();

                XmlSerializer ax = new XmlSerializer(typeof(SpriteTable));

                Anim = 0;
                Action = false;
                FieldView = new FieldView(fov, true);


                using (Stream file = TitleContainer.OpenStream("Content/Sprites/" + name + ".xml"))
                {
                    SpriteTable = (SpriteTable)ax.Deserialize(file);

                }


                Load(content);

            }



        public void Load(ContentManager content)
            {

                    List<Sprite> Sprites;
                    //Face = Image.FromFile(Config.SPRITEDIR + Class + "/face.png");
                    //Weapon = Image.FromFile(Config.SPRITEDIR + Class + "/rifle.png");
                    //Cursor = Image.FromFile(Config.SPRITEDIR + Class + "/aim.png");


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
                            CurrentAnimation.IsFliped = false;
                            X += 3;

                        }

                        else if (LeftDown == true)
                        {

                            //P1 Move left
                            CurrentAnimation = Animations[1];
                            CurrentAnimation.IsFliped = true;
                            X -= 3;

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




    }

}
