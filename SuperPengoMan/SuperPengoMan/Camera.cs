﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    public class Camera
    {
        Vector2 position;
        Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get { return viewMatrix;}
        }

        public int ScreenWidth
        {
            get { return Game1.TILE_SIZE * 15; }
        }

        public void Update(Vector2 pengoPos)
        {
            position.X = pengoPos.X - (ScreenWidth / 2);
            Console.WriteLine(pengoPos);

            if (position.X < 0)
            {
                position.X = 0;
            }

            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}