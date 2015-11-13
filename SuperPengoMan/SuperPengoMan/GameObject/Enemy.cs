using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Enemy:GameObject
    {
        Clock clock;

        Rectangle hitbox;
        Rectangle srcRect;

        int animate = 1;
        public Enemy(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, texture.Height);
            clock = new Clock(); 
        }
        
        public void Update()
        {
            clock.AddTime(0.03f);
            Animate();          
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            srcRect = new Rectangle(Game1.TILE_SIZE * Animate(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            spriteBatch.Draw(texture, hitbox, srcRect, Color.White);

        }

        private int Animate()
        {
            if (clock.Timer() > 0.2f)
            {
                animate++;
                clock.ResetTime();
                if (animate > 5)
                {
                    animate = 0;
                }
            }
            return animate;
        }
    }
}
