﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Coin : GameObject
    {
        Game1.AddPointsDelegate addPoints;

        public bool visible = true;

        private char option = '0';
        private Rectangle srcRect;
        private Rectangle dstRect;

        public Coin(Texture2D texture,  Vector2 pos, char option, Game1.AddPointsDelegate addPoints) : base(texture, pos)
        {
            SetOption(option);
            this.addPoints = addPoints;
            srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        private int Points
        {
            get
            {
                if (option == '0')
                {
                    return 10;
                }
                else
                {
                    return 50;
                }

            }
        }

        public void SetOption(char option)
        {
            this.option = option;
            dstRect = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), Game1.TILE_SIZE, Game1.TILE_SIZE);
            if (Points <= 10)
            {
                dstRect.Inflate(-15, -15);
            }
            else
            {
                dstRect.Inflate(-10, -10);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Draw(spriteBatch, texture, dstRect, srcRect, Color.White);
            }

        }

        public bool IsColliding(Rectangle hitRect)
        {
            Rectangle tempHitRect = new Rectangle(hitRect.Location, hitRect.Size);
            Rectangle temp = new Rectangle(dstRect.Location,dstRect.Size);
            //tempHitRect.Inflate(-19, -19);
            //temp.Inflate(-19, -19);
            if (temp.Intersects(tempHitRect) && visible)
            {
                visible = false;
                addPoints(Points);
                return true;
            }
            else
            {
                return false;
            }  
        }

    }
}