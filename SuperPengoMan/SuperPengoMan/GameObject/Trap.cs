using Microsoft.Xna.Framework;
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
        public Trap(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
        }

        public bool PixelCollition(Pengo pengo)
        {
            Color[] dataA = new Color[texture.Width * texture.Height];
            texture.GetData(dataA);
            Color[] dataB = new Color[pengo.texture.Width * pengo.texture.Height];
            pengo.texture.GetData(dataB);

            int top = Math.Max(hitbox.Top, pengo.hitbox.Top);
            int bottom = Math.Min(hitbox.Bottom, pengo.hitbox.Bottom);
            int left = Math.Max(hitbox.Left, pengo.hitbox.Left);
            int right = Math.Min(hitbox.Right, pengo.hitbox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[texture.Width * (y - hitbox.Y) + x - hitbox.X];
                    //Color colorA = dataA[(x - hitbox.Left) + (y - hitbox.Top) * hitbox.Width];
                    int index = pengo.texture.Width * (y - pengo.hitbox.Y + pengo.srcRect.Y) + x - pengo.hitbox.X + pengo.srcRect.X;
                    Color colorB = dataB[index];
                    //Color colorB = dataB[(x - pengo.hitbox.Left) + (y - pengo.hitbox.Top) * pengo.hitbox.Width];

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
