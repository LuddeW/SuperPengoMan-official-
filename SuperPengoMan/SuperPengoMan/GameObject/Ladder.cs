using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Ladder:GameObject
    {
        public Rectangle hitbox;
        public Ladder(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            hitbox = new Rectangle((int)pos.X + texture.Width -5, (int)pos.Y, 5, texture.Height + 5);
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(texture, hitbox, Color.Red);
        //}
    }
}
