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
        Rectangle srcRect;
        int speed = -1;

        int animate = 1;
        public Enemy(Texture2D texture, Vector2 pos) : base(texture, pos)
        {           
            clock = new Clock(); 
        }
        
        public void Update()
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y -1, Game1.TILE_SIZE, texture.Height);
            clock.AddTime(0.03f);
            Animate();
            pos.X += speed;
            Console.WriteLine(pos);
            Console.WriteLine(speed);    
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

        public bool PixelCollition(Pengo pengo)
        {
            Color[] dataA = new Color[texture.Width * texture.Height];
            texture.GetData(dataA);
            Color[] dataB = new Color[pengo.texture.Width * pengo.texture.Height];
            pengo.texture.GetData(dataB);

            int top = Math.Max(hitbox.Top, pengo.hitbox.Top);
            int bottom = Math.Min(hitbox.Bottom, pengo.hitbox.Bottom);
            int left = Math.Max(hitbox.Left, pengo.hitbox.Left);
            int right = Math.Min(hitbox.Right, pengo.hitbox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - hitbox.Left) + (y - hitbox.Top) * hitbox.Width];
                    Color colorB = dataB[(x - pengo.hitbox.Left) + (y - pengo.hitbox.Top) * pengo.hitbox.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
