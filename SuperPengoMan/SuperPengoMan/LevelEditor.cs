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
        KeyboardState prevKeyState;
        KeyboardState keyState;

        public LevelEditor(Game game, LevelReader levelsLevelReader, Game1.HandleMenuOptionDelegate handleMenuOptionDelegate) :
            base(null, handleMenuOptionDelegate)
        {
            this.game = game;
            this.levelsLevelReader = levelsLevelReader;
            LoadContent(game.Content);
            cursorTex = game.Content.Load<Texture2D>(@"cursor");
            hudFont = game.Content.Load<SpriteFont>(@"HUDFont");
            EditorTileSize = Game1.TILE_SIZE / 2;
            camera = new Camera(EditorTileSize*45, EditorTileSize, 1);
            cursor = new Cursor(cursorTex, new Vector2(gameAreaStartPos.X, gameAreaStartPos.Y), EditorTileSize);
            keyState = Keyboard.GetState();
        }

        public Camera EditorCamera
        {
            get { return camera; }
            set { camera = value; }
        }

        public int EditorTileSize { get; private set; }

        private bool WasKeyPressed(Keys key)
        {
            return keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key);
        }

        internal Vector2 ScreenPos(int ixXPos, int ixYPos)
        {
            return new Vector2(gameAreaStartPos.X + EditorTileSize * ixXPos, gameAreaStartPos.Y + EditorTileSize * ixYPos);
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

            CreateLevel(levelsLevelReader[currentlevel]);
        }

        public void CreateLevel(Level level)
        {
            CreateLevel(level, EditorTileSize, gameAreaStartPos);
            camera.GameAreaTilesWidth = level.Cols;
            cursor.TilesWidth = level.Cols;
            cursor.TilesHeight = level.Rows;
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
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                keyState.IsKeyDown(Keys.Back))
            {
                handleMenuOptionDelegate('0');
            }
            cursor.Update();
            camera.Update(cursor.pos);
            Keys[] keys = keyState.GetPressedKeys();
            if(keys.Length > 0)
            { 
                CheckKeys(cursor.CursorTilePosX(),cursor.CursorTilePosY());
            }
        }

        private void CheckKeys(int cursorXPos, int cursorYPos)
        {
            foreach (KeyItem keyItem in KeyList.GameObjetItems)
            {
                if (keyItem.Key != KeyList.MenuTileKey.Key && WasKeyPressed(keyItem.Key))
                {
                    LevelItem levelItem = levelsLevelReader[currentlevel].Get(cursorYPos, cursorXPos);
                    ChangeTileObject(cursorXPos, cursorYPos, keyItem, levelItem);
                }

            }
            foreach (KeyItem keyItem in KeyList.OptionItems)
            {
                if (WasKeyPressed(keyItem.Key))
                {
                    LevelItem levelItem = levelsLevelReader[currentlevel].Get(cursorYPos, cursorXPos);
                    ChangeOptionObject(cursorXPos, cursorYPos, keyItem, levelItem);
                }

            }
        }

        private void ChangeTileObject(int cursorXPos, int cursorYPos, KeyItem keyItem, LevelItem levelItem)
        {
            RemoveTile(cursorXPos, cursorYPos);
            char option = KeyList.Option0Key.Char;

            if (keyItem.Key == KeyList.FloorTileKey.Key)
            {
                AddFloortile(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.PengoKey.Key)
            {
                NewPengo(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.WaterTileKey.Key)
            {
                AddWaterTile(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.TrapKey.Key)
            {
                AddTrap(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.EnemyKey.Key)
            {
                AddEnemy(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.LadderKey.Key)
            {
                AddLadder(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.CoinKey.Key)
            {
                if (levelsLevelReader[currentlevel].Get(cursorXPos, cursorYPos).GameObject == KeyList.CoinKey.Char)
                {
                    option = levelsLevelReader[currentlevel].Get(cursorYPos, cursorXPos).Option;
                }
                AddCoin(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            if (keyItem.Key == KeyList.MenuTileKey.Key)
            {
                if (levelsLevelReader[currentlevel].Get(cursorYPos, cursorXPos).GameObject == KeyList.MenuTileKey.Char)
                {
                    option = levelsLevelReader[currentlevel].Get(cursorYPos, cursorXPos).Option;
                }
                AddMenuTile(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            levelItem.Option = option;
            levelItem.GameObject = keyItem.Char;
        }

        private void RemoveTile(int cursorXPos, int cursorYPos)
        {
            FloorTile ft = FindFlorTile(cursorXPos, cursorYPos);
            floorTiles.Remove(ft);

            Pengo pengo = FindPengo(cursorXPos, cursorYPos);
            if (pengo != null)
            {
                this.pengo = null;
            }

            WaterTile wt = FindWaterTile(cursorXPos, cursorYPos);
            waterTiles.Remove(wt);

            Trap trap = FindTrap(cursorXPos, cursorYPos);
            traps.Remove(trap);

            Enemy enemy = FindEnemy(cursorXPos, cursorYPos);
            enemies.Remove(enemy);

            Ladder ladder = FindLadder(cursorXPos, cursorYPos);
            ladders.Remove(ladder);

            Coin coin = FindCoin(cursorXPos, cursorYPos);
            coins.Remove(coin);

            MenuTile mt = FindMenuTile(cursorXPos, cursorYPos);
            menuTiles.Remove(mt);
        }

        private FloorTile FindFlorTile(int cursorXPos, int cursorYPos)
        {
            FloorTile result = null;
            foreach (FloorTile floorTile in floorTiles)
            {
                if (floorTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = floorTile;
                }
            }
            return result;
        }

        private Pengo FindPengo(int cursorXPos, int cursorYPos)
        {
            Pengo result = null;
            if (pengo.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
            {
                result = pengo;
            }
            return result;
        }

        private WaterTile FindWaterTile(int cursorXPos, int cursorYPos)
        {
            WaterTile result = null;
            foreach (WaterTile waterTile in waterTiles)
            {
                if (waterTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = waterTile;
                }
            }
            return result;
        }

        private Trap FindTrap(int cursorXPos, int cursorYPos)
        {
            Trap result = null;
            foreach (Trap trap in traps)
            {
                if (trap.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = trap;
                }
            }
            return result;
        }

        private Enemy FindEnemy(int cursorXPos, int cursorYPos)
        {
            Enemy result = null;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = enemy;
                }
            }
            return result;
        }

        private Ladder FindLadder(int cursorXPos, int cursorYPos)
        {
            Ladder result = null;
            foreach (Ladder ladder in ladders)
            {
                if (ladder.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = ladder;
                }
            }
            return result;
        }

        private Coin FindCoin(int cursorXPos, int cursorYPos)
        {
            Coin result = null;
            foreach (Coin coin in coins)
            {
                if (coin.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = coin;
                }
            }
            return result;
        }

        private MenuTile FindMenuTile(int cursorXPos, int cursorYPos)
        {
            MenuTile result = null;
            foreach (MenuTile menuTile in menuTiles)
            {
                if (menuTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = menuTile;
                }
            }
            return result;
        }

        private void ChangeOptionObject(int cursorXPos, int cursorYPos, KeyItem keyItem, LevelItem levelItem)
        {
            //FloorTile ft = FindFlorTile(cursorXPos, cursorYPos);

            //Pengo pengo = FindPengo(cursorXPos, cursorYPos);

            //WaterTile wt = FindWaterTile(cursorXPos, cursorYPos);

            //Trap trap = FindTrap(cursorXPos, cursorYPos);

            //Enemy enemy = FindEnemy(cursorXPos, cursorYPos);

            //Ladder ladder = FindLadder(cursorXPos, cursorYPos);

            Coin coin = FindCoin(cursorXPos, cursorYPos);
            if (coin != null)
            {
                coin.SetOption(keyItem.Char);
            }

            MenuTile mt = FindMenuTile(cursorXPos, cursorYPos);
            if (mt != null)
            {
                mt.SetOption(keyItem.Char);
            }

            levelItem.Option = keyItem.Char;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatchObject.Scale = 1.0f * EditorTileSize / Game1.TILE_SIZE;
            foreach (FloorTile iceTile in floorTiles)
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
            foreach (WaterTile waterTile in waterTiles)
            {
                waterTile.Draw(spriteBatch);
            }
            foreach (MenuTile menuTile in menuTiles)
            {
                menuTile.Draw(spriteBatch);
            }
            DrawGrid(spriteBatch);
            cursor.Draw(spriteBatch);
            SpriteBatchObject.Scale = 1f;
            DrawTileData(spriteBatch, cursor.CursorTilePosX(), cursor.CursorTilePosY());
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            Texture2D tex = new Texture2D(game.GraphicsDevice, 1, 1);
            tex.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            for (int rows = 0; rows <= levelsLevelReader[currentlevel].Rows; rows++)
            {
                DrawLine(spriteBatch, tex,//draw line
                    new Vector2(gameAreaStartPos.X, 
                                    gameAreaStartPos.Y + rows * EditorTileSize), //start of line
                    new Vector2(gameAreaStartPos.X + levelsLevelReader[currentlevel].Cols * EditorTileSize,
                                    gameAreaStartPos.Y + rows * EditorTileSize), //end of line
                                    Color.Black);
            }
            for (int cols = 0; cols <= levelsLevelReader[currentlevel].Cols; cols++)
            {
                DrawLine(spriteBatch, tex,//draw line
                    new Vector2(gameAreaStartPos.X + cols * EditorTileSize,
                                    gameAreaStartPos.Y), //start of line
                    new Vector2(gameAreaStartPos.X + cols * EditorTileSize,
                                    gameAreaStartPos.Y + levelsLevelReader[currentlevel].Rows * EditorTileSize), //end of line
                                    Color.Black);
            }
        }

        void DrawLine(SpriteBatch spriteBatch, Texture2D tex, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            spriteBatch.Draw(tex,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        readonly float TËXTOFFSET_X = 5f;
        readonly float TILE_TËXTOFFSET_Y = 5f;
        readonly float OPTION_TËXTOFFSET_Y = 20f;

        private void DrawTileData(SpriteBatch spriteBatch, int tilePosX, int tilePosY)
        {
            LevelItem levelItem = levelsLevelReader[currentlevel].Get(tilePosY, tilePosX);
            string tile = "Tile: " + levelItem.GameObject;
            spriteBatch.DrawString(hudFont, tile,
                new Vector2(TËXTOFFSET_X - camera.ViewMatrix.Translation.X, 
                gameAreaStartPos.Y + EditorTileSize * levelsLevelReader[currentlevel].Rows + TILE_TËXTOFFSET_Y), Color.White);
            string option = "Option: " + levelItem.Option;
            spriteBatch.DrawString(hudFont, option,
                new Vector2(TËXTOFFSET_X - camera.ViewMatrix.Translation.X, 
                gameAreaStartPos.Y + EditorTileSize * levelsLevelReader[currentlevel].Rows + OPTION_TËXTOFFSET_Y), Color.White);
        }

    }
}
