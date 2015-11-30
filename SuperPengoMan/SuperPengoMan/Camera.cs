using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    public class Camera
    {
        Vector2 position;
        Matrix viewMatrix;

        public Camera(int screenWidth, int viewTileSize, int gameAreaTilesWidth)
        {
            ScreenWidth = screenWidth;
            ViewTileSize = viewTileSize;
            GameAreaTilesWidth = gameAreaTilesWidth;
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix;}
        }

        public int ScreenWidth { get; set; }
        public int ViewTileSize { get; set; }
        public int GameAreaTilesWidth { get; set; }

        public void Update(Vector2 pengoPos)
        {
            position.X = pengoPos.X - (ScreenWidth / 2);            
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > Game1.TILE_SIZE * GameAreaTilesWidth - ScreenWidth)
            {
                position.X = Game1.TILE_SIZE * GameAreaTilesWidth - ScreenWidth;
            }
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
