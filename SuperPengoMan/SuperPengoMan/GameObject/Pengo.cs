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
        private Rectangle srcRect;
        private Clock clock;

        private Texture2D glide;
        private Texture2D jump;
        KeyboardState keyState;

        private enum SpriteShow { hor, slide, jump}
        private SpriteShow currentSprite = SpriteShow.hor;
        private int pengoAnimation = 0;
        private bool isOnGround = false;
        private Vector2 speed;
        private int windowX;
        private int windowY;
        private Rectangle hitbox;
        private bool moving = false;
        

        public Pengo(Texture2D texture, Texture2D glide, Texture2D jump, Vector2 pos) : base(texture, pos)
        {
            clock = new Clock();
            this.glide = glide;
            this.jump = jump;
            speed = new Vector2(1, 1);
            windowX = Game1.TILE_SIZE * 15;
            windowY = Game1.TILE_SIZE * 11;
            
        }

        public void Update()
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
            if (pos.X < 0)
            {
                pos.X = 0;
            }
          
            MovePengo();
            
            clock.AddTime(0.03f);
        }

        private void MovePengo()
        {
            keyState = Keyboard.GetState();
            if (!isOnGround)
            {
                speed.Y += 0.2f;
                speed.X = 0;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.Right) && pos.X + (texture.Width / 2) < windowX )
            {
                speed.X = 1;
                moving = true;
            }
            if (keyState.IsKeyDown(Keys.Left) && pos.X + (texture.Width / 2) > 0)
            {
                speed.X = -1;
                moving = true;
            }
            if (keyState.IsKeyDown(Keys.Up) && isOnGround)
            {
                speed.Y = -6;
                isOnGround = false;
                currentSprite = SpriteShow.jump;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.Down) && isOnGround)
            {              
                currentSprite = SpriteShow.slide;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
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
                currentSprite = SpriteShow.hor;

            }
            if (isOnGround && moving)
            {
                srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                moving = false;
            }
        }

        //private void MovePengo()
        //{
        //    srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    if (Keyboard.GetState().IsKeyDown(Keys.Right))
        //    {
        //        pos.X++;
        //        currentSprite = SpriteShow.hor;
        //        srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.Left))
        //    {
        //        pos.X--;
        //        currentSprite = SpriteShow.hor;
        //        srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    }
        //    else
        //    {
        //        currentSprite = SpriteShow.hor;
        //    }
        //    if (Keyboard.GetState().IsKeyDown(Keys.Up))
        //    {
        //        pos.Y--;
        //        currentSprite = SpriteShow.jump;
        //        srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    }
        //    if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left))
        //    {
        //        pos.X --;
        //        currentSprite = SpriteShow.slide;
        //        srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right))
        //    {
        //        pos.X++;
        //        currentSprite = SpriteShow.slide;
        //        srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.Down))
        //    {
        //        srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
        //        pos.Y++;
        //    }
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentSprite)
            {
                case SpriteShow.hor:
                    spriteBatch.Draw(texture, pos, srcRect, Color.White);
                    break;
                case SpriteShow.slide:
                    spriteBatch.Draw(glide, pos, srcRect, Color.White);
                    break;
                case SpriteShow.jump:
                    spriteBatch.Draw(jump, pos, srcRect, Color.White);
                    break;
            }
        }

        private int PengoAnimation()
        {
            if (clock.Timer() > 0.2f)
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
    }
}
