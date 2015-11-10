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
        Rectangle srcRect;
        public Pengo(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            srcRect = new Rectangle(0, 0, 40, 40); 
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, srcRect, Color.White);
        }
    }
}
