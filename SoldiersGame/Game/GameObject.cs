using System;
using System.Collections.Generic;
using System.Linq;
using SoldierTactics.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SoldierTactics.Game
{
   public class GameObject : Entity
    {

       public ObjectType OType;
       public Animation Sprite, Effect;
       public Sound ObjectSound;
       public Rectangle Bounds;
       public Point Origin;
       public bool Trigger;

       public GameObject(int id, string name, int x, int y, ObjectType type, int frames, ContentManager content)
           {

               ID = id;
               Name = name;
               X = x;
               Y = y;
               OType = type;

               Sprite = new Animation(new Sprite(Config.SPRITEDIR + "Objects/" + Name, content ), 1.0F, frames, true);

               //Effect = new Animation(new Sprite(Config.SPRITEDIR + "explosion.png", 1540 , 90), 2.0F, 14, false);
               Bounds = new Rectangle(x, y, Sprite.FrameWidth, Sprite.FrameHeight);
               Origin = new Point(Sprite.FrameWidth / 2, Sprite.FrameHeight / 2);
               //ObjectSound = new Sound(3, SoundEngine.explodeSound, false);
    

           }

       public void Update(GameTime gameTime)
           {

               if (Trigger == true)
                   {
                   Sprite = null;
                   Effect.Update(gameTime);
                   Bounds = new Rectangle(X, Y, Effect.FrameWidth, Effect.FrameHeight);
                   }

           

           }


       public void Draw()
           {
               if (Sprite != null)
                   Sprite.Draw(X, Y, Sprite.FrameWidth, Sprite.FrameHeight);

               if (Trigger == true && Effect.isEnabled)
                   Effect.Draw(X - Effect.Frames[0].xOrigin, Y - Effect.Frames[0].yOrigin, Effect.FrameWidth, Effect.FrameHeight);

           }

    }


    public enum ObjectType
         {

        None = 0,
        Static = 1,
        Dynamic = 2

         }
}
