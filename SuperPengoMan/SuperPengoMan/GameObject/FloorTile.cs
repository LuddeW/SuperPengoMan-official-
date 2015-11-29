using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class FloorTile:GameObject
    {
        public Rectangle topHitbox;
        public Rectangle leftHitbox;
        public Rectangle rightHitbox;
        public FloorTile(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            this.texture = texture;
            this.pos = pos;
            topHitbox = new Rectangle((int)pos.X + 4, (int)pos.Y - 2, texture.Width - 8, 2);
            leftHitbox = new Rectangle((int)pos.X, (int)pos.Y + 3, texture.Width / 2, texture.Height - 1);
            rightHitbox = new Rectangle((int)pos.X + texture.Width / 2, (int)pos.Y + 3, texture.Width / 2, texture.Height - 1);
        }
    }
}
