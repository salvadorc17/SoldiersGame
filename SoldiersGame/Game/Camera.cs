﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace SoldiersGame
{
    public class Camera
    {

        public Vector2 Position, panPos;
        public int Width, Height, vpWidth, vpHeight;
        public double panSpeed;
        public bool ToPan;


        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(vpWidth * 0.5f, vpHeight * 0.5f);
            }
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                   -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(0) *
                   Matrix.CreateScale(new Vector3(1, 1, 1)) *
                   Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }

        public Camera(int width, int height, int vpWidth, int vpHeight)
        {


            Width = width;
            Height = height;

            this.vpWidth = vpWidth;
            this.vpHeight = vpHeight;

            this.Position = new Vector2(0, 0);
            this.panPos = new Vector2(0, 0);

            this.panSpeed = 6.1;
            this.ToPan = false;

        }

        public bool setX(int x)
        {
            if (this.vpWidth + x <= this.Width && x >= 0)
            {
                this.Position.X = x;
                return true;
            }
            return false;
        }

        public bool setY(int y)
        {
            if (this.vpHeight + y <= this.Height && y >= 0)
            {
                this.Position.Y = y;
                return true;
            }

            return false;
        }

        public bool IncrementX(int x)
        {
            return this.setX((int)Position.X + x);
        }

        public bool IncrementY(int y)
        {
            return this.setY((int)Position.Y + y);
        }

        public void Pan(Vector2 pos)
        {

            panPos = pos;
            ToPan = true;

        }

        public void Update()
        {

            if (ToPan)
            {
                if (this.panPos.X > this.Position.X)
                {
                    this.IncrementX((int)panSpeed);
                }

                if (this.panPos.X < this.Position.X)
                {
                    this.IncrementX(-((int)panSpeed));
                }

                if (this.panPos.Y > this.Position.Y)
                {
                    this.IncrementY((int)panSpeed);

                }


                if (this.panPos.Y < this.Position.Y)
                {
                    this.IncrementY(-((int)panSpeed));
                }
            }


        }

    }

   
}
