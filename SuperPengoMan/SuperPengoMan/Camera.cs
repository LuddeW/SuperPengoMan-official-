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
            else if (position.X > ViewTileSize * GameAreaTilesWidth - ScreenWidth)
            {
                position.X = ViewTileSize * GameAreaTilesWidth - ScreenWidth;
            }

            ViewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
