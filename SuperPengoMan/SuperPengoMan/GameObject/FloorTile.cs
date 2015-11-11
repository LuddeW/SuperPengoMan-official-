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
        Rectangle hitbox;
        public FloorTile(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            
        }
        public Rectangle GetRect()
        {
            return hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
        }
        
    }
}
