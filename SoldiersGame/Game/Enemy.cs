﻿using System;
using System.Collections.Generic;
using SoldierTactics.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Xml.Serialization;
using SoldiersGame;

namespace SoldierTactics.Game
{
    public class Enemy : Entity
    {

        public int State;
        public Direction Direction;
        public SpriteTable SpriteTable;

        public List<Animation> Animations;
        public Animation CurrentAnimation;
        public Animation Effect;
        public Rectangle Bounds;
        public Sound DieSound;       
        public Route Path;
        public Vector Position;
        public Point[] ViewField;
        public bool MoveRoute, Alive;
        public EntityType Type { get; set; }
        public bool Route { get; set; }
        public RouteType RType { get; set; }

        public Enemy(int id, string name, int x, int y, bool route, RouteType type, ContentManager content)
             {
                 ID = id;
                 Name = name;
                 X = x;
                 Y = y;
                 Position = new Vector(x, y);
                 MoveRoute = route;
                 Alive = true;
            Type = EntityType.None;
            RType = RouteType.None;


            Animations = new List<Animation>();
            ViewField = new Point[4];

            if (route == true)
                      {
                    Path = new Route(X, Y, X + 200, Y + 200, type);
                    Path.Enable(true);

                      }
                 else if (route == false)
                     {
                    Path = new Route(X, Y, X, Y, RouteType.None);
                    Path.Enable(false);
                 
                     }

                 State = 1;

                XmlSerializer ax = new XmlSerializer(typeof(SpriteTable));

            using (Stream file = TitleContainer.OpenStream("Content/Sprites/" + name + ".xml"))
            {
                SpriteTable = (SpriteTable)ax.Deserialize(file);

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

            if (Animations != null)
                     {

                    
                         CurrentAnimation = Animations[0];
                         //Effect = new Animation(new Sprite(Config.TILEDIR + "/blood.png", 31, 21), 1.0F, 1, false);
                         Direction = Direction.None; 
                         Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
                         //DieSound = new Sound(2, SoundEngine.diedSound, false);
                     
                      }
            }

        public void Die()
         {
             CurrentAnimation = Animations[6];
             Alive = false;
         }


        public void SetPoisition(int x, int y)
        {

            X = x;
            Y = y;
            Position = new Vector(X, Y);
        
        
        }

         public void Update(GameTime gameTime)
         {


             if (CurrentAnimation != null)
                  {

                      Position.Update(X, Y);
                 
                 if (MoveRoute && Alive)
                      {

                     switch (Path.Type)
                      {

                         case RouteType.Horizontal:

                          if (X >= Path.XF)
                              State = 2;

                          if (X < Path.X)
                              State = 1;
                     
                          if (State == 1)
                              {
                                 CurrentAnimation = Animations[1];
                                 
                                 X += 5;
                              }
                          else if (State == 2)
                              {
                                  CurrentAnimation = Animations[2];
                                  //CurrentAnimation.Update();
                                  X -= 5;
                              }

                             break;


                         case RouteType.Vertical:

                             if (Y >= Path.YF)
                              State = 2;

                          if (Y < Path.Y)
                              State = 1;
                     
                          if (State == 1)
                              {
                                 CurrentAnimation = Animations[4];
                                 //CurrentAnimation.Update();
                                 Y += 5;
                              }
                          else if (State == 2)
                              {
                                  CurrentAnimation = Animations[3];
                                  //CurrentAnimation.Update();
                                  Y -= 5;
                              }

                             break;
                      }



                     }

                 Bounds = new Rectangle(X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
                 CurrentAnimation.Update(gameTime);

                  }

             if (Alive == false)
                 {

                     Effect.Update(gameTime);
                        

                 }
                  
         }

         public void Draw()
         {
             if (CurrentAnimation != null)
                 CurrentAnimation.Draw( X, Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);

             if (Alive == false && Effect.isEnabled)
             {

                 Effect.Draw( X, Y, Effect.FrameWidth, Effect.FrameHeight);


             }


         }

    }
}
