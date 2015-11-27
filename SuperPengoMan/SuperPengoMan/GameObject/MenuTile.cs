using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class MenuTile : GameObject
    {
        char menuOption;
        Game1.HandleMenuOptionDelegate handleMenuOptionDelegate;

        public MenuTile(Texture2D texture, Vector2 pos, char option, Game1.HandleMenuOptionDelegate handleMenuOptionDelegate) : base(texture, pos)
        {
            this.menuOption = option;
            this.handleMenuOptionDelegate = handleMenuOptionDelegate;
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
        }

        public Rectangle Hitbox { get; }

        public void SetOption(char option)
        {
            menuOption = option;
        }

        void HandleCollision()
        {
            handleMenuOptionDelegate(menuOption);
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
