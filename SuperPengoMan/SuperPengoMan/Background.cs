﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class Background
    {
        List<Vector2> foreground, middleground;
        int fgSpacing, mgSpacing;
        float fgSpeed, mgSpeed;
        Texture2D[] texture;
        GameWindow window;
        public Background(ContentManager Content, GameWindow window)
        {
            this.window = window;
            this.texture = new Texture2D[2];

            texture[0] = Content.Load<Texture2D>(@"cloud");
            texture[1] = Content.Load<Texture2D>(@"big_cloud");
            CreatForeGround();
            CreatMiddleGround();
        }

        public void Update()
        {
            MoveForeground();
            MoveMiddleGround();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 v in middleground)
            {
                spriteBatch.Draw(texture[1], new Vector2(v.X, 20), Color.White);
            }
            foreach (Vector2 v in foreground)
            {
                spriteBatch.Draw(texture[0], new Vector2(v.X, 100), Color.White);
            }
        }

        private void CreatForeGround()
        {
            foreground = new List<Vector2>();
            fgSpacing = texture[0].Width + 3 * Game1.TILE_SIZE;
            fgSpeed = 0.75f;
            for (int i = 0; i < (Game1.TILE_SIZE * 49 / fgSpacing) + 2; i++)
            {
                foreground.Add(new Vector2(i * fgSpacing, window.ClientBounds.Height - texture[0].Height));
            }
        }

        private void CreatMiddleGround()
        {
            middleground = new List<Vector2>();
            mgSpacing = texture[1].Width + Game1.TILE_SIZE * 3;
            mgSpeed = 0.3f;
            for (int i = 0; i < (Game1.TILE_SIZE * 49); i++)
            {
                middleground.Add(new Vector2(i * mgSpacing, window.ClientBounds.Height - texture[0].Height - texture[1].Height));
            }
        }

        private void MoveForeground()
        {
            for (int i = 0; i < foreground.Count; i++)
            {
                foreground[i] = new Vector2(foreground[i].X - fgSpeed, foreground[i].Y);
                if (foreground[i].X <= -fgSpacing)
                {
                    int j = i - 1;
                    if (j < 0)
                    {
                        j = foreground.Count - 1;
                    }
                    foreground[i] = new Vector2(foreground[j].X + fgSpacing - 1, foreground[i].Y);
                }
            }
        }

        private void MoveMiddleGround()
        {
            for (int i = 0; i < middleground.Count; i++)
            {
                middleground[i] = new Vector2(middleground[i].X - mgSpeed, middleground[i].Y);
                if (middleground[i].X <= -mgSpacing)
                {
                    int j = i - 1;
                    if (j < 0)
                    {
                        j = middleground.Count - 1;
                    }
                    middleground[i] = new Vector2(middleground[j].X + mgSpacing - 1, middleground[i].Y);
                }
            }
        }
    }
}