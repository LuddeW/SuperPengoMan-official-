using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;
using System;
using Microsoft.Xna.Framework.Input;

namespace SuperPengoMan
{
    class LevelEditor: ObjectHandler
    {
        private Game game;

        private Camera camera;
        private LevelReader levelsLevelReader;
        int currentlevel = -1;
        Texture2D cursorTex;
        Cursor cursor;
        readonly Point gameAreaStartPos = new Point(0, 50);
        SpriteFont hudFont;

        public LevelEditor(Game game, LevelReader levelsLevelReader, Game1.HandleMenuOptionDelegate handleMenuOptionDelegate) :
            base(null, handleMenuOptionDelegate)
        {
            this.levelsLevelReader = levelsLevelReader;
            camera = new Camera();
            LoadContent(game.Content);
            cursorTex = game.Content.Load<Texture2D>(@"cursor");
            hudFont = game.Content.Load<SpriteFont>(@"HUDFont");
            cursor = new Cursor(cursorTex, new Vector2(gameAreaStartPos.X, gameAreaStartPos.Y), Game1.TILE_SIZE / 2);

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

            CreateLevel(levelsLevelReader[currentlevel], Game1.TILE_SIZE/2, gameAreaStartPos);
            cursor.TilesWidth = levelsLevelReader[currentlevel].Cols;
            cursor.TilesHeight = levelsLevelReader[currentlevel].Rows;
            cursor.SetPos(0, levelsLevelReader[currentlevel].Cols - 1);
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            pengo.Update();
            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
            }
        }

        private void ResetLevel()
        {

        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                handleMenuOptionDelegate('0');
            }
            cursor.Update();
            camera.Update(cursor.pos);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatchObject.Scale = 0.5f;
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
            cursor.Draw(spriteBatch);
            SpriteBatchObject.Scale = 1f;
            DrawTileData(spriteBatch, cursor.CursorTilePosX(), cursor.CursorTilePosY());
        }

        private void DrawTileData(SpriteBatch spriteBatch, int tilePosX, int tilePosY)
        {
            LevelItem levelItem = levelsLevelReader[currentlevel].Get(tilePosY, tilePosX);
            string tile = "Tile: " + levelItem.GameObject;
            spriteBatch.DrawString(hudFont, tile,
                new Vector2(5 - camera.ViewMatrix.Translation.X, 
                gameAreaStartPos.Y + Game1.TILE_SIZE / 2 * levelsLevelReader[currentlevel].Rows + 5), Color.White);
            string option = "Option: " + levelItem.Option;
            spriteBatch.DrawString(hudFont, option,
                new Vector2(5 - camera.ViewMatrix.Translation.X, 
                gameAreaStartPos.Y + Game1.TILE_SIZE / 2 * levelsLevelReader[currentlevel].Rows + 30), Color.White);
        }
    }
}
