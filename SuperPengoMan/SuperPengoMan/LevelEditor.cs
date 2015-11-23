using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;

namespace SuperPengoMan
{
    class LevelEditor: ObjectHandler
    {
        private Game game;

        private Camera camera;
        private LevelReader levelsLevelReader;
        int currentlevel = -1;

        public LevelEditor(Game game, LevelReader levelsLevelReader)
        {
            this.levelsLevelReader = levelsLevelReader;
            camera = new Camera();
            LoadContent(game.Content);
        }

        public Camera EditorCamera
        {
            get { return camera; }
            set { camera = value; }
        }

        public void InitEditor()
        {
            currentlevel = -1;
            NextLevel();
        }

        public void NextLevel()
        {
            ResetLevel();
            if (currentlevel < (levelsLevelReader.Count - 1))
            {
                currentlevel++;
            }

            CreateLevel(levelsLevelReader[currentlevel], Game1.TILE_SIZE, new Point(0, 0));
        }

        private void ResetLevel()
        {

        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (FloorTile iceTile in floortile)
            {
                iceTile.Draw(spriteBatch);
            }
            foreach (Ladder ladderTile in ladders)
            {
                ladderTile.Draw(spriteBatch);
            }
            foreach (Trap spike in traps)
            {
                spike.Draw(spriteBatch);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (Coin coin in coins)
            {
                coin.Draw(spriteBatch);
            }
            if (pengo != null)
            {
                pengo.Draw(spriteBatch);
            }
            foreach (WaterTile waterTile in watertile)
            {
                waterTile.Draw(spriteBatch);
            }
            foreach (MenuTile menuTile in menuTiles)
            {
                menuTile.Draw(spriteBatch);
            }
        }

    }
}
