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

        private enum HitState { left, right, none}
        HitState currentHitState = HitState.none;

        public Rectangle hitbox;
        public Rectangle topHitbox;
        Rectangle srcRect;
        int speed = -1;

        int animate = 1;
        public Enemy(Texture2D texture, Vector2 pos) : base(texture, pos)
        {           
            clock = new Clock(); 
        }
        
        public void Update()
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, texture.Height);
            topHitbox = new Rectangle((int)pos.X + 5, (int)pos.Y-5, Game1.TILE_SIZE - 10, 5);
            clock.AddTime(0.03f);
            Animate();
            pos.X += speed;
        }

        public bool IsColliding(FloorTile floorTile)
        {
            if (hitbox.Intersects(floorTile.leftHitbox))
            {
                currentHitState = HitState.left;
                return true;
            }
            else if (hitbox.Intersects(floorTile.rightHitbox))
            {
                currentHitState = HitState.right;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void HandleCollision()
        {
            if (currentHitState == HitState.left)
            {
                speed = 0;
                hitbox.X = hitbox.X - 2;
                speed = -1;
            }
            else if (currentHitState == HitState.right)
            {
                speed = 0;
                hitbox.X = hitbox.X + 2;
                speed = 1;
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            srcRect = new Rectangle(Game1.TILE_SIZE * Animate(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            Draw(spriteBatch, texture, hitbox, srcRect, Color.White);
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
