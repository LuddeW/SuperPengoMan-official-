using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SuperPengoMan
{
    public class SpriteBatchObject
    {

        private static float scale = 1f;

        public SpriteBatchObject()
        {

        }

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

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Rectangle srcRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteFx, float layerDepth)
        {
            spriteBatch.Draw(texture, pos, srcRect, color, rotation, origin, scale * Scale, spriteFx, layerDepth);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color)
        {
            Rectangle srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            Draw(spriteBatch, texture, rectangle, srcRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Color color)
        {
            Rectangle destRect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            Rectangle srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            Draw(spriteBatch,texture, destRect, srcRect, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Rectangle srcRect, Color color)
        {
            rectangle.Width = Convert.ToInt32(rectangle.Width * scale);
            rectangle.Height = Convert.ToInt32(rectangle.Height * scale);
            spriteBatch.Draw(texture, rectangle, srcRect, color);
        }

    }
}