using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class OptionCollisionTile : GameObject
    {
        char option;
        Game1.HandleOptionDelegate handleOptionDelegate;

        public OptionCollisionTile(Texture2D texture, Vector2 pos, char option, Game1.HandleOptionDelegate handleOptionDelegate) : base(texture, pos)
        {
            this.option = option;
            this.handleOptionDelegate = handleOptionDelegate;
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
        }

        public Rectangle Hitbox { get; }

        public void SetOption(char option)
        {
            this.option = option;
        }

        void HandleCollision()
        {
            handleOptionDelegate(option);
        }

        public bool IsColliding(Rectangle hitbox)
        {
            bool result = false;
            if (Hitbox.Intersects(hitbox))
            {
                HandleCollision();
                result = true;
            }
            return result;
        }
    }
}
