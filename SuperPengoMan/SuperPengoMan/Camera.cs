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

        public Matrix ViewMatrix { get; private set; }
        public int ScreenWidth { get; set; }
        public int ViewTileSize { get; set; }
        public int GameAreaTilesWidth { get; set; }

        public void Update(Vector2 objectPos)
        {
            position.X = objectPos.X - (ScreenWidth / 2);

            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > Game1.TILE_SIZE * 49 - ScreenWidth)
            {
                position.X = Game1.TILE_SIZE * 49 - ScreenWidth;
            }

            ViewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
