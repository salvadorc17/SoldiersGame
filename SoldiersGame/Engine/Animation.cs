using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SoldiersGame;


namespace SoldierTactics.Engine
{

    public class Animation
    {
        public Sprite CurrentSprite
        {
            get { return m_currentsprite; }
        }

        private Sprite m_currentsprite;


        public List<Sprite> Frames;
        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return m_frameTime; }
        }

        private float m_frameTime;
        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return m_isLooping; }
        }

        private bool m_isLooping;

        public bool IsFliped;
       

        public bool isEnabled;
        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return m_framecount; }
        }

        private int m_framecount;

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { return CurrentSprite.Width / FrameCount; }
        }

        public int FrameWidthFixed
        {
            // Assume square frames.
            get { return CurrentSprite.Width; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return CurrentSprite.Height; }
        }

        public Vector Origin
        {
            get {

                return new Vector(FrameWidth / 2, FrameHeight / 2);
            
            }
        }


        public int FrameIndex
        {
            get { return m_frameIndex; }
        }
        private int m_frameIndex;

        private Single time;

        /// <summary>
        /// Constructors a new animation.
        /// </summary>        


        public Animation(List<Sprite> textures, float frameTime, int framecount, bool isLooping)
        {

            this.m_currentsprite = textures[0];
            this.m_frameTime = frameTime;
            this.m_framecount = framecount;
            this.m_isLooping = isLooping;
            isEnabled = true;

            Frames = textures;
        }

        public void Update(GameTime gameTime)
        {

            
            DateTime startTime = new DateTime();
            TimeSpan elapsedTime = new TimeSpan();
            

            elapsedTime = DateTime.Now + gameTime.ElapsedGameTime - startTime;

            // Process passing time.
            time += Convert.ToSingle(elapsedTime.Seconds);
            while (time > FrameTime)
            {
                time -= FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (IsLooping)
                {
                    m_frameIndex = (m_frameIndex + 1) % FrameCount;

                    if (m_frameIndex == FrameCount - 1)
                        m_frameIndex = 0;
                }
                else
                {
                    m_frameIndex = Math.Min(m_frameIndex + 1, FrameCount - 1);

                    if (m_frameIndex == FrameCount - 1)
                    isEnabled = false;
                }
            }


        }

        public void Draw( int x, int y, int w, int h)
        {

            if (Frames[FrameIndex].Image != null)
                Global.SpriteBatch.Draw(Frames[FrameIndex].Image, new Vector2(x,y), new Rectangle(0,0, w,h), Color.White, 
                    0, Vector2.Zero, 1.0f, IsFliped == true ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1.0f);


        }

    }
}
