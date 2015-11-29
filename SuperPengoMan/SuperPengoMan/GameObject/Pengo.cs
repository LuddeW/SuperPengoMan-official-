using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Pengo:GameObject
    {
        public Rectangle srcRect;
        private Clock clock;

        private Texture2D glide;
        private Texture2D jump;
        private Texture2D climb;
        KeyboardState keyState;

        private enum HitState { top, left, right, none }
        HitState currentHitState = HitState.none;
        private enum SpriteShow { hor, slide, jump, climb}
        private SpriteShow currentSprite = SpriteShow.hor;
        private int pengoAnimation = 0;
        private bool isOnGround = false;
        public Vector2 speed;
        private int windowY;
        public Rectangle hitbox;
        private bool moving = false;
        private SpriteEffects spriteFx = SpriteEffects.None;
        public bool isOnLadder = false;

        Color[] texData;

        public Pengo(Texture2D texture, Texture2D glide, Texture2D jump, Texture2D climb, Vector2 pos) : base(texture, pos)
        {
            clock = new Clock();
            this.glide = glide;
            this.jump = jump;
            this.climb = climb;
            speed = new Vector2(0, 0);
            windowY = Game1.TILE_SIZE * 15;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
            srcRect = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            texData = new Color[texture.Width * texture.Height];
            texture.GetData(texData);
        }

        public Color[] TexData { get { return texData; }}
        public int TexWidth { get { return texture.Width; } }

        public void Update(bool editMode = false)
        {
            
            if (pos.X < 0)
            {
                pos.X = 0;
            }

            if (!editMode)
            {
                MovePengo();
                clock.AddTime(0.03f);
            }
        }

        public bool IsColliding(Rectangle topHitbox, Rectangle leftHitbox, Rectangle rightHitbox)
        {
            if (hitbox.Intersects(topHitbox))
            {
                
                currentHitState = HitState.top;
                return true;
            }
            else if (hitbox.Intersects(leftHitbox))
            {
                currentHitState = HitState.left;
                return true;
            }
            else if (hitbox.Intersects(rightHitbox))
            {
                currentHitState = HitState.right;
                return true;
            }
            else
            {
                currentHitState = HitState.none;
                return false;
            }
        }

        public void HandleCollision(Rectangle collideHitbox)
        {
            switch (currentHitState)
            {
                case HitState.top:
                    isOnGround = true;
                    speed.Y = 5.0f;
                    speed.X = 0;
                    hitbox.Y = collideHitbox.Y - hitbox.Height;
                    pos.Y = hitbox.Y;
                    break;
                case HitState.left:
                    speed.X = 0;
                    if (currentSprite == SpriteShow.slide)
                    {
                        hitbox.X = (int)pos.X - 4;
                    }
                    else
                    {
                        hitbox.X = (int)pos.X - 2;
                    }
                    pos.X = hitbox.X;
                    break;
                case HitState.right:
                    speed.X = 0;
                    if (currentSprite == SpriteShow.slide)
                    {
                        speed.X = 0;
                        hitbox.X = hitbox.X + 4;
                    }
                    else
                    {
                        speed.X = 0;
                        hitbox.X = hitbox.X + 2;
                    }                   
                    pos.X = hitbox.X;
                    break;
                case HitState.none:
                    break;
                
            }
            
        }

        private void MovePengo()
        {
            keyState = Keyboard.GetState();
            currentSprite = SpriteShow.hor;
            if (!isOnGround)
            {
                speed.Y += 0.3f;
                speed.X = 0;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                currentSprite = SpriteShow.jump;
            }
            if (keyState.IsKeyDown(Keys.Right) && !keyState.IsKeyDown(Keys.D))
            {
                speed.X = +2;
                moving = true;
                spriteFx = SpriteEffects.None;
            }
            if (keyState.IsKeyDown(Keys.Left) && !keyState.IsKeyDown(Keys.D))
            {
                speed.X = -2;
                moving = true;
                spriteFx = SpriteEffects.FlipHorizontally;
            }
            if (keyState.IsKeyDown(Keys.Up) && isOnGround && !isOnLadder)
            {
                speed.Y = -6;
                isOnGround = false;
                currentSprite = SpriteShow.jump;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.D) && isOnGround)
            {              
                currentSprite = SpriteShow.slide;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.Right) && isOnGround)
            {
                speed.X = +4;
                currentSprite = SpriteShow.slide;
                spriteFx = SpriteEffects.None;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.Left) && isOnGround)
            {
                speed.X = - 4;
                currentSprite = SpriteShow.slide;
                spriteFx = SpriteEffects.FlipHorizontally;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.Up) && isOnLadder)
            {
                speed.Y = -1;
                isOnGround = false;
                currentSprite = SpriteShow.climb;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                isOnLadder = false;
            }

            pos += speed;
            hitbox.X = (int)(pos.X > 0 ? pos.X + 0.5f : pos.X - 0.5f);
            hitbox.Y = (int)(pos.Y > 0 ? pos.Y + 0.5f : pos.Y - 0.5f);

            if (hitbox.Y + hitbox.Height >= windowY)
            {
                pos.Y = windowY - hitbox.Height;
                isOnGround = true;
                speed.Y = 0;
                speed.X = 0;
            }
            if (isOnGround && moving)
            {
                srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                moving = false;   
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentSprite)
            {
                case SpriteShow.hor:
                    Draw(spriteBatch, texture, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
                case SpriteShow.slide:
                    Draw(spriteBatch, glide, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
                case SpriteShow.jump:
                    Draw(spriteBatch, jump, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
                case SpriteShow.climb:
                    Draw(spriteBatch, climb, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
            }
        }

        private int PengoAnimation()
        {
            if (clock.Timer() > 0.1f)
            {
                pengoAnimation++;
                clock.ResetTime();
                if (pengoAnimation > 3)
                {
                    pengoAnimation = 0;
                }
            }
            return pengoAnimation;
        }

        public void KillPengo(Vector2 pengoRespawnPos)
        {         
            pos = pengoRespawnPos;
        }

        public void IsONLadder()
        {
            isOnLadder = true;
        }
    }
}
