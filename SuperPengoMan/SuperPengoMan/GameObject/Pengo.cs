using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Pengo:GameObject
    {
        private Rectangle srcRect;
        private Clock clock;

        private int pengoAnimation = 0;

        public Pengo(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            clock = new Clock();
        }

        public void Update()
        {
            clock.AddTime(0.03f);
            srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, srcRect, Color.White);
        }

        private int PengoAnimation()
        {
            if (clock.Timer() > 0.2f)
            {
                pengoAnimation++;
                clock.ResetTime();
                if (pengoAnimation > 3)
                {
                    pengoAnimation = 0;
                }
            }
            return pengoAnimation;
        }
    }
}
