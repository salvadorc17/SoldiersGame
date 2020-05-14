using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;
using SoldierTactics.Game;
using SoldiersGame;


namespace SoldierTactics
{

    class GameplayScreen : GameScreen
    {


        private SpriteFont hudFont;
        private Level Level;
        private int LevelIndex;

        private Camera Camera;
        private VertexBuffer VBuffer;
        private UI UI;
       
        private float pauseAlpha;
        // Meta-Maze game state.

        private bool wasContinuePressed;

        private MouseState MouseState;
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        //private AccelerometerState accelerometerState;
        // When the time remaining is less than the warning time, it blinks on the hud

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);




		public GameplayScreen(ScreenManager screenManager, UI ui)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            screenManager.Game.ResetElapsedTime();
            UI = ui;
            UI.Load();
        }




        public override void LoadContent()
        {


            hudFont = ScreenManager.content.Load<SpriteFont>("Fonts/Hud");


            Level = new Level(LevelIndex, "Test", ScreenManager.content);

            Camera = new Camera(Level.Map.Width, Level.Map.Height,
                 ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            Level.CameraPos = new Vector2(0, 0);

           

            Camera.setX((int)Level.CameraPos.X);
            Camera.setY((int)Level.CameraPos.Y);


            Camera.Pan(Level.CameraPos);


        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            MouseState = Mouse.GetState();


            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Global.ScreenWidth = viewport.Width;
            Global.ScreenHeight = viewport.Height;

            if (viewport.Width >= 1280)
                Global.ScaleWidth = 4;
            else if (viewport.Width >= 1024)
                Global.ScaleWidth = 3;
            else if (viewport.Width >= 800)
                Global.ScaleWidth = 2;
            else if (viewport.Width >= 600)
                Global.ScaleWidth = 1;

            if (viewport.Height >= 960)
                Global.ScaleHeight = 4;
            else if (viewport.Height >= 800)
                Global.ScaleHeight = 3;
            else if (viewport.Height >= 600)
                Global.ScaleHeight = 2;
            else if (viewport.Height >= 480)
                Global.ScaleHeight = 1;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            HandleInput();


            if (Level != null)
                Level.Update(gameTime);


            Camera.Update();

        }

        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            //accelerometerState = Accelerometer.GetState();


            // Exit the game when back is pressed.
            if (keyboardState.IsKeyDown(Keys.Escape) ||
               gamePadState.IsButtonDown(Buttons.Start))
            {
                ExitScreen ();
                ScreenManager.AddScreen(new BackgroundScreen(), PlayerIndex.One);
                ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);


            }

            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A);
                 
            if (keyboardState.IsKeyDown(Keys.F1))
            {

                Global.DebugMode = !Global.DebugMode;

            }
            else if (keyboardState.IsKeyDown(Keys.J))
            {

                Camera.ToPan = false;
                Camera.IncrementX(-15);

            }
            else if (keyboardState.IsKeyDown(Keys.I))
            {
                Camera.ToPan = false;
                Camera.IncrementY(-15);

            }
            else if (keyboardState.IsKeyDown(Keys.K))
            {
                Camera.ToPan = false;
                Camera.IncrementY(15);

            }
            else if (keyboardState.IsKeyDown(Keys.L))
            {
                Camera.ToPan = false;
                Camera.IncrementX(15);

            }


            wasContinuePressed = continuePressed;
        }


        public override void Draw(GameTime gameTime)
        {

            //Matrix cameraTransform = Matrix.CreateTranslation(-Maze.cameraPosition, 0.0f, 0.0f);

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Camera.TranslationMatrix);

            base.Draw(gameTime);


            if (Level != null)
                Level.Draw(ScreenManager.SpriteBatch);

            DrawHud(ScreenManager.SpriteBatch);



            ScreenManager.SpriteBatch.End();

        }

        private void DrawHud(SpriteBatch spriteb)
        {
            
            Rectangle titleSafeArea = new Rectangle(10,60, 640, 200);
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2f, titleSafeArea.Y + titleSafeArea.Height / 2f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // er is running out of time.
            string timeString = "COORDS: " + "X: " + MouseState.X + " Y: " + MouseState.Y;
            Color timeColor = Color.DarkRed;
           
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            if (UI.Hud != null)
                ScreenManager.SpriteBatch.Draw(UI.Hud, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y,
                    UI.Hud.Width, UI.Hud.Height), Color.White);


            if (UI.Hud2 != null)
                ScreenManager.SpriteBatch.Draw(UI.Hud2, new Rectangle((int)Camera.Position.X + UI.Hud.Width, (int)Camera.Position.Y,
                    UI.Hud2.Width, UI.Hud2.Height), Color.White);

            if (UI.Bar != null)
                ScreenManager.SpriteBatch.Draw(UI.Bar, new Rectangle((int)Camera.Position.X + 800 - UI.Bar.Width, (int)Camera.Position.Y + UI.Hud.Height,
                    UI.Bar.Width, UI.Bar.Height), Color.White);

            if (UI.Bar2 != null)
                ScreenManager.SpriteBatch.Draw(UI.Bar2, new Rectangle((int)Camera.Position.X + 800 - UI.Bar2.Width, (int)Camera.Position.Y + 600 - UI.Bar2.Height,
                    UI.Bar2.Width, UI.Bar2.Height), Color.White);

            if (UI.Eye != null)
                ScreenManager.SpriteBatch.Draw(UI.Eye, new Rectangle((int)Camera.Position.X + 800 - UI.Eye.Width, (int)Camera.Position.Y,
                    UI.Eye.Width, UI.Eye.Height), Color.White);

            if (UI.Camera != null)
                ScreenManager.SpriteBatch.Draw(UI.Camera, new Rectangle((int)Camera.Position.X + 800 - 60 - UI.Camera.Width, (int)Camera.Position.Y,
                    UI.Camera.Width, UI.Camera.Height), Color.White);

            if (UI.Backpack != null)
                ScreenManager.SpriteBatch.Draw(UI.Backpack, new Rectangle((int)Camera.Position.X + 800 - UI.Backpack.Width, (int)Camera.Position.Y + 600 - UI.Backpack.Height,
                    UI.Backpack.Width, UI.Backpack.Height), Color.White);

            if (Level.Player != null && UI.Marco != null)
                ScreenManager.SpriteBatch.Draw(UI.Marco, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + 5,
                    UI.Marco.Width, UI.Marco.Height), Color.White);

            // Determine the status overlay message to show.
            Texture2D status = null;

            if (UI.Cursor != null)
                ScreenManager.SpriteBatch.Draw(UI.Cursor, new Rectangle(MouseState.X, MouseState.Y,
                    UI.Cursor.Width, UI.Cursor.Height), Color.White);


            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteb.Draw(status, center - statusSize / 2, Color.White);
            }

            if (Level != null)
                Level.PostDraw((int)Camera.Position.X, (int)Camera.Position.Y);

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color__1)
        {
            ScreenManager.SpriteBatch.DrawString(font, value, position + Camera.Position, Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position + Camera.Position, color__1);
        }
    }
}