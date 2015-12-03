
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Trap : GameObject
    {
        Rectangle hitbox;
        Color[] texData;
 
        public Trap(Texture2D texture, Vector2 pos, bool rotated = false) : base(texture, pos)
        {
            Rotated = rotated;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            texData = new Color[texture.Width * texture.Height];
            texture.GetData(texData);
        }

        public bool Rotated { get; set; }

        public bool PixelCollition(Color[] collideData, int collideWidth, Rectangle collideSrcRect, Rectangle collideHitbox)
        {

            int top = Math.Max(hitbox.Top, collideHitbox.Top);
            int bottom = Math.Min(hitbox.Bottom, collideHitbox.Bottom);
            int left = Math.Max(hitbox.Left, collideHitbox.Left);
            int right = Math.Min(hitbox.Right, collideHitbox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA; 
                    if (!Rotated)
                    {
                        colorA = texData[texture.Width * (y - hitbox.Y) + x - hitbox.X];
                    }
                    else
                    {
                        colorA = texData[((Game1.TILE_SIZE - 1) - (x - hitbox.Left)) +
                                                    ((Game1.TILE_SIZE - 1) - (y - hitbox.Top)) * texture.Width];
                    }


                    int index = collideWidth * (y - collideHitbox.Y + collideSrcRect.Y) + x - collideHitbox.X + collideSrcRect.X;
                    Color colorB = collideData[index];
                    if (colorA.A + colorB.A > 256)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (!Rotated)
            {
                Draw(spriteBatch, texture, pos, Color.White);
            }
            else
            {
                float rotation = MathHelper.ToRadians(180);
                Draw(spriteBatch, texture, pos, new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE),
                    Color.White, rotation, new Vector2(Game1.TILE_SIZE, Game1.TILE_SIZE), 1f, SpriteEffects.None, 1f);

            }
        }

    }
}
