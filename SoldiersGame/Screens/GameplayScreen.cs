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
       
        private float pauseAlpha;
        // Meta-Maze game state.

        private bool wasContinuePressed;

        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        //private AccelerometerState accelerometerState;
        // When the time remaining is less than the warning time, it blinks on the hud

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);




		public GameplayScreen(ScreenManager screenManager)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            screenManager.Game.ResetElapsedTime();


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



        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {


            base.Update(gameTime, otherScreenHasFocus, false);

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
                 
                


            wasContinuePressed = continuePressed;
        }


        public override void Draw(GameTime gameTime)
        {

            //Matrix cameraTransform = Matrix.CreateTranslation(-Maze.cameraPosition, 0.0f, 0.0f);

            ScreenManager.SpriteBatch.Begin();

            base.Draw(gameTime);

            DrawHud(ScreenManager.SpriteBatch);

            if (Level != null)
                Level.Draw(ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();

        }

        private void DrawHud(SpriteBatch spriteb)
        {
            
            Rectangle titleSafeArea = new Rectangle(10,10, 640, 200);
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2f, titleSafeArea.Y + titleSafeArea.Height / 2f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // er is running out of time.
            string timeString = "TIME: "; //+ Maze.TimeRemaining.Minutes.ToString("00") + ":" + Maze.TimeRemaining.Seconds.ToString("00");
            Color timeColor = Color.DarkRed;
            //if (Maze.TimeRemaining > WarningTime || Maze.CurrentRoom.ReachedExit || Convert.ToInt32(Maze.TimeRemaining.TotalSeconds) % 2 == 0)
           // {
               // timeColor = Color.White;
            //}
           // else
            //{
               // timeColor = Color.Red;
            //}
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            // Draw score
           // float timeHeight = hudFont.MeasureString(timeString).Y;
           // DrawShadowedString(hudFont, "SCORE: " + Maze.Score.ToString(), hudLocation + new Vector2(0f, timeHeight * 1.2f), Color.White);

            // Determine the status overlay message to show.
            Texture2D status = null;


            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteb.Draw(status, center - statusSize / 2, Color.White);
            }
            

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color__1)
        {
            ScreenManager.SpriteBatch.DrawString(font, value, position + new Vector2(1f, 1f), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position, color__1);
        }
    }
}