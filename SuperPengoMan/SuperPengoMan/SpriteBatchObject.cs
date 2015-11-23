using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class SpriteBatchObject
    {
        private static float scale = 1f;

        public static float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex, Rectangle dstRect, Color color)
        {
            Rectangle srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            Draw(spriteBatch, tex, dstRect, srcRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex, Vector2 pos, Color color)
        {
            Rectangle dstRect = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), tex.Width, tex.Height);
            Rectangle srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            Draw(spriteBatch, tex, dstRect, srcRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color)
        {
            dstRect.Height = Convert.ToInt32(dstRect.Height * scale);
            dstRect.Width = Convert.ToInt32(dstRect.Width* scale);
            spriteBatch.Draw(tex, dstRect, srcRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex, Vector2 pos, Rectangle srcRect, Color color, float rotation, 
                                Vector2 origin, float texScale, SpriteEffects spriteFx, float layerDepth)
        {
            spriteBatch.Draw(tex, pos, srcRect, color, rotation, origin, texScale * scale, spriteFx, layerDepth);
        }


    }
}
