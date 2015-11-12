using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Util
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private int columns;
        private int rows;

        public SpriteSheet(int columns, int rows, Texture2D texture)
        {
            if (columns == 0)
            {
                throw new ArgumentException("Must not be zero value", nameof(columns));
            }
            if (rows == 0)
            {
                throw new ArgumentException("Must not be zero value", nameof(rows));
            }
            this.columns = columns;
            this.rows = rows;
            this.Texture = texture;
        }

        public Rectangle SrcRect(int animation)
        {
            int row = animation / Columns;
            int col = animation - row * Columns;
            return new Rectangle(Texture.Width / Columns * col, Texture.Height / Rows * row,
                Texture.Width / Columns * (col + 1), Texture.Height / Rows * (row + 1));
        }
        public int Columns
        {
            get
            {
                return columns;
            }
        }
        public int Rows
        {
            get
            {
                return rows;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return texture;
            }

            set
            {
                texture = value;
            }
        }
    }

}
