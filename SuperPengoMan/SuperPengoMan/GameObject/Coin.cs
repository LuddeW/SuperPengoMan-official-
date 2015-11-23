using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Coin : GameObject
    {
        Game1.AddPointsDelegate addPoints;

        public bool visible = true;

        private int coins;
        private Rectangle srcRect;
        private Rectangle dstRect;

        public Coin(Texture2D texture,  Vector2 pos, char option, Game1.AddPointsDelegate addPoints) : base(texture, pos)
        {
            if (option == '0')
            {
                this.coins = 10;
            }
            else
            {
                this.coins = 50;
            }
            this.addPoints = addPoints;
            srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            dstRect = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), Game1.TILE_SIZE,Game1.TILE_SIZE);
            if (coins <= 10)
            {
                dstRect.Inflate(-15, -15);
            }
            else
            {
                dstRect.Inflate(-10, -10);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, dstRect, srcRect, Color.White);
            }

        }

        public bool IsColliding(Rectangle hitRect)
        {
            Rectangle tempHitRect = new Rectangle(hitRect.Location, hitRect.Size);
            Rectangle temp = new Rectangle(dstRect.Location,dstRect.Size);
            //tempHitRect.Inflate(-19, -19);
            //temp.Inflate(-19, -19);
            if (temp.Intersects(tempHitRect) && visible)
            {
                visible = false;
                addPoints(coins);
                return true;
            }
            else
            {
                return false;
            }  
        }
    }
}
