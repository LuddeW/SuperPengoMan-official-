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

        private enum SpriteShow { hor, slide, jump}
        private SpriteShow currentSprite = SpriteShow.hor;
        private int pengoAnimation = 0;
        

        public Pengo(Texture2D texture, Texture2D glide, Texture2D jump, Vector2 pos) : base(texture, pos)
        {
            clock = new Clock();
            this.glide = glide;
            this.jump = jump;
        }

        public void Update()
        {
            if (pos.X < 0)
            {
                pos.X = 0;
            }
            MovePengo();
            clock.AddTime(0.03f);
            //if (currentSprite == SpriteShow.hor)
            //{
            //    srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            //}
            //else
            //{
            //    srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            //}
        }

        private void MovePengo()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                pos.X++;
                currentSprite = SpriteShow.hor;
                srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                pos.X--;
                currentSprite = SpriteShow.hor;
                srcRect = new Rectangle(Game1.TILE_SIZE * PengoAnimation(), 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                pos.Y--;
                currentSprite = SpriteShow.jump;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                pos.Y++;
                currentSprite = SpriteShow.slide;
                srcRect = new Rectangle(Game1.TILE_SIZE * 0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
            }
        }

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
