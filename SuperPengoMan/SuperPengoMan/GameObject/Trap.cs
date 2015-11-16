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
        private readonly bool rotated;
        Rectangle hitbox;
        public Trap(Texture2D texture, Vector2 pos, bool rotated = false) : base(texture, pos)
        {
            this.rotated = rotated;
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
                    Color colorA;
                    Color colorB = dataB[(x - pengo.hitbox.Left) + (y - pengo.hitbox.Top) * pengo.hitbox.Width];
                    if (!rotated)
                    {
                        colorA = dataA[(x - hitbox.Left) + (y - hitbox.Top) * hitbox.Width];
                    }
                    else
                    {
                        colorA = dataA[((Game1.TILE_SIZE - 1) - (x - hitbox.Left)) + 
                                                    ((Game1.TILE_SIZE - 1) - (y - hitbox.Top)) * hitbox.Width];
                    }

                    if (colorA.A !=0 && colorB.A !=0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (!rotated)
            {
                spriteBatch.Draw(texture, pos, Color.White);
            }
            else
            {
                float rotation = MathHelper.ToRadians(180);
                spriteBatch.Draw(texture, pos, new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE),
                    Color.White, rotation, new Vector2(Game1.TILE_SIZE, Game1.TILE_SIZE), 1f, SpriteEffects.None, 1f);

            }
        }

    }
}
