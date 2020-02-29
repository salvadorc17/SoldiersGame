using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SoldiersGame;

namespace SoldierTactics.Engine
{

    public class Animation
    {
        public Sprite Texture
        {
            get { return m_texture; }
        }

        private Sprite m_texture;


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
            get { return Texture.Width / FrameCount; }
        }

        public int FrameWidthFixed
        {
            // Assume square frames.
            get { return Texture.Width; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
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

        public Animation(Sprite texture, float frameTime, int framecount, bool isLooping)
        {

            this.m_texture = texture;
            this.m_frameTime = frameTime;
            this.m_framecount = framecount;
            this.m_isLooping = isLooping;
            isEnabled = true;

            Frames = Texture.Split(Texture.Width, Texture.Height);
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

            if (m_texture != null)
                Global.SpriteBatch.Draw(Texture.Image, new Rectangle(x, y, w * (int)Global.ScaleWidth, h * (int)Global.ScaleHeight), new Rectangle(m_frameIndex * w, 0, w, h), Color.White);


        }

    }
}
