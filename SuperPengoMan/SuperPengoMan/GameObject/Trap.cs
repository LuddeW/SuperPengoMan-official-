﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Trap:GameObject
    {
        Rectangle hitbox;
        Color[] texData;
        public Trap(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            texData = new Color[texture.Width * texture.Height];
            texture.GetData(texData);
        }

        public bool PixelCollition(Pengo pengo)
        {

            int top = Math.Max(hitbox.Top, pengo.hitbox.Top);
            int bottom = Math.Min(hitbox.Bottom, pengo.hitbox.Bottom);
            int left = Math.Max(hitbox.Left, pengo.hitbox.Left);
            int right = Math.Min(hitbox.Right, pengo.hitbox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = texData[texture.Width * (y - hitbox.Y) + x - hitbox.X];
                    int index = pengo.texture.Width * (y - pengo.hitbox.Y + pengo.srcRect.Y) + x - pengo.hitbox.X + pengo.srcRect.X;
                    Color colorB = pengo.texData[index];
                    if (colorA.A + colorB.A > 256)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
