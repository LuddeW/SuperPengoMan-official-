using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Util
{
    class Animator
    {
        private readonly SpriteSheet spriteSheet;
        private int row;
        private int updateTime;
        private int nextUpdateTime = 0;
        private int animation = 1;

        public Animator(SpriteSheet spriteSheet, int updateTime, int row = 0)
        {
            if (spriteSheet == null)
            {
                throw new ArgumentException("SpriteSheet must be an instance", nameof(spriteSheet));
            }
            if (row >= spriteSheet.Rows)
            {
                throw new ArgumentException("Row value out of index", nameof(row));
            }

            this.spriteSheet = spriteSheet;
            this.row = row;
            this.updateTime = updateTime;
        }

        public void Update(GameTime gameTime)
        {
            if (nextUpdateTime == 0)
            {
                nextUpdateTime = Convert.ToInt32((gameTime.TotalGameTime.TotalMilliseconds));
            }
            if (gameTime.TotalGameTime.TotalMilliseconds.CompareTo(nextUpdateTime) == 1)
            {
                NextAnimation();
                while (gameTime.TotalGameTime.TotalMilliseconds.CompareTo(nextUpdateTime) == 1)
                {
                    nextUpdateTime += updateTime;
                }
            }
        }

        private void NextAnimation()
        {
            animation++;
            if (animation >= spriteSheet.Columns)
            {
                animation = 0;
            }
        }
 

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(spriteSheet.Texture, pos, spriteSheet.SrcRect(animation),Color.White);  
        }
    }
}
