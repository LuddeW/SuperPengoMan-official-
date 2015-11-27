using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class Cursor : GameObject
    {

        KeyboardState keyState;
        KeyboardState prevKeyState;


        public int Step { get; set; }
        public int TilesWidth { get; set; }
        public int TilesHeight { get; set; }
        public Vector2 StartPos { get; set; }

        public Cursor(Texture2D texture, Vector2 pos, int step) : base(texture, pos)
        {
            keyState = Keyboard.GetState();
            Step = step;
            StartPos = pos;
        }

        public void SetPos(int tilePosX, int tilePosY)
        {
            if (tilePosX >= 0 && tilePosX < TilesWidth)
            {
                pos.X = StartPos.X + (tilePosX - 1)*Step;
            }
            if (tilePosY >= 0 && tilePosY < TilesHeight)
            {
                pos.Y = StartPos.Y + (tilePosY - 1) * Step;
            }
        }

        public int CursorTilePosX()
        {
            return Convert.ToInt32((pos.X - StartPos.X)/Step);
        }

        public int CursorTilePosY()
        {
            return Convert.ToInt32((pos.Y - StartPos.Y) / Step);
        }

        private bool WasKeyPressed(Keys key)
        {
            return keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key);
        }

        public void Update()
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            MoveCursor();
        }

        private void MoveCursor()
        {
            if (WasKeyPressed(Keys.Right))
            {
                pos.X += Step;
            }
            if (WasKeyPressed(Keys.Left))
            {
                pos.X -= Step;
            }
            if (WasKeyPressed(Keys.Up))
            {
                pos.Y -= Step;
            }
            if (WasKeyPressed(Keys.Down))
            {
                pos.Y += Step;
            }

            if (pos.X < StartPos.X)
            {
                pos.X = StartPos.X;
            }
            if (pos.X > (StartPos.X + (TilesWidth - 1) * Step))
            {
                pos.X = (StartPos.X + (TilesWidth - 1) * Step);
            }
            if (pos.Y < StartPos.Y)
            {
                pos.Y = StartPos.Y;
            }
            if (pos.Y > (StartPos.Y +(TilesHeight - 1) * Step))
            {
                pos.Y = (StartPos.Y + (TilesHeight - 1) * Step);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, texture, pos, Color.White);
        }

    }
}
