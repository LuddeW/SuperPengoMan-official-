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
        private int windowY;
        private Rectangle hitbox;
        private bool moving = false;
        private SpriteEffects spriteFx = SpriteEffects.None;
        private float movingXSpeed;
        private float floorXSpeed;


        public Pengo(Texture2D texture, Texture2D glide, Texture2D jump, Vector2 pos) : base(texture, pos)
        {
            clock = new Clock();
            this.glide = glide;
            this.jump = jump;
            speed = new Vector2(1, 1);
            windowY = Game1.TILE_SIZE * 15;
            
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

        public bool IsColliding(FloorTile floorTile)
        {
            return hitbox.Intersects(floorTile.hitbox);
        }

        public void HandleCollision(FloorTile floorTile)
        {
            isOnGround = true;
            speed.Y += 0.3f;
            speed.X = 0;
            hitbox.Y = floorTile.hitbox.Y - hitbox.Height;
            pos.Y = hitbox.Y;
        }

        private void MovePengo()
        {
            keyState = Keyboard.GetState();
            currentSprite = SpriteShow.hor;
            if (!isOnGround)
            {
                speed.Y += 0.3f;
                SetMovingXSpeed(0);
                speed.X = GetScreenXSpeed();
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                currentSprite = SpriteShow.jump;
            }
            if (keyState.IsKeyDown(Keys.Right) && !keyState.IsKeyDown(Keys.Down))
            {
                SetMovingXSpeed(+2);
                speed.X = GetScreenXSpeed();
                moving = true;
                spriteFx = SpriteEffects.None;
            }
            if (keyState.IsKeyDown(Keys.Left) && !keyState.IsKeyDown(Keys.Down))
            {
                SetMovingXSpeed(-2);
                speed.X = GetScreenXSpeed();
                moving = true;
                spriteFx = SpriteEffects.FlipHorizontally;
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
            if (keyState.IsKeyDown(Keys.Down) && keyState.IsKeyDown(Keys.Right) && isOnGround)
            {
                SetMovingXSpeed(+4);
                speed.X = GetScreenXSpeed();
                currentSprite = SpriteShow.slide;
                spriteFx = SpriteEffects.None;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (keyState.IsKeyDown(Keys.Down) && keyState.IsKeyDown(Keys.Left) && isOnGround)
            {
                SetMovingXSpeed(-4);
                speed.X = GetScreenXSpeed();
                currentSprite = SpriteShow.slide;
                spriteFx = SpriteEffects.FlipHorizontally;
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
            }
            if (isOnGround && moving)
            {
                srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
                moving = false;   
            }
        }

        public float GetScreenXSpeed()
        {
            if (MovingRightAndScrollFloor || MovingLeftAndScrollFloor)
            {
                floorXSpeed = -movingXSpeed;
                return 0;
            }
            else
            {
                floorXSpeed = 0;
                return movingXSpeed;
            }
        }

        private void SetMovingXSpeed(float speedX)
        {
            movingXSpeed = speedX;
        }

        private bool MovingRightAndScrollFloor
        {
            get { return pos.X.CompareTo(Game1.TILE_SIZE * 15 * 0.67f) == 1 &&
                    movingXSpeed.CompareTo(0f) == 1; }
        }

        private bool MovingLeftAndScrollFloor
        {
            get { return pos.X.CompareTo(Game1.TILE_SIZE * 15 * 0.33f) == -1 &&
                    movingXSpeed.CompareTo(0f) == -1; }
        }

        public float FloorXSpeed { get {return floorXSpeed; }}


        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentSprite)
            {
                case SpriteShow.hor:
                    spriteBatch.Draw(texture, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
                case SpriteShow.slide:
                    spriteBatch.Draw(glide, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
                    break;
                case SpriteShow.jump:
                    spriteBatch.Draw(jump, pos, srcRect, Color.White, 0f, new Vector2(), 1f, spriteFx, 0f);
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
    }
}
