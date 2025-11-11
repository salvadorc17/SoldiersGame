using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SoldiersGame;
using System.IO;


namespace SoldierTactics
{

    class BackgroundScreen : GameScreen
    {
        
        private ContentManager content;
        public int backgroundScale = 0;
        public string backgroundFile;
        public Texture2D backgroundTexture;
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            backgroundFile = "MENU800.BMP";

            backgroundTexture = ImageManager.LoadTexture(ScreenManager.GraphicsDevice, Config.SYSDIR + Path.DirectorySeparatorChar + "MISC" +
                Path.DirectorySeparatorChar + backgroundFile);


        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (backgroundScale == 1)
                backgroundFile = "MENU800.BMP";
            else if (backgroundScale == 2)
                backgroundFile = "MENU1024.BMP";
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }


    }
}