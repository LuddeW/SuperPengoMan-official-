using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class WaterTile:GameObject
    {
        public WaterTile(Texture2D texture, Vector2 pos) : base(texture, pos)
        {

        }

        public void Update(float speedX)
        {
            pos.X += speedX;
        }
    }
}
