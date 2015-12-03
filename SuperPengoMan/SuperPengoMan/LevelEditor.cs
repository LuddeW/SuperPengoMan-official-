using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace SuperPengoMan
{
    class LevelEditor: ObjectHandler
    {
        static float TËXTOFFSET_X = 5f;
        static float FIRST_TËXTOFFSET_Y = 5f;
        static float ADD_TËXTOFFSET_Y = 15f;
        static int DEFAULT_ROWS_IN_LEVEL = 15;

        private Camera camera;
        private LevelReader levelsLevelReader;
        int currentlevel = -1;
        Texture2D cursorTex;
        Cursor cursor;
        readonly Point gameAreaStartPos = new Point(0, 50);
        SpriteFont hudFont;
        KeyboardState prevKeyState;
        KeyboardState keyState;

        public LevelEditor(Game game, LevelReader levelsLevelReader,
                            Game1.HandleOptionDelegate handleMenuOptionDelegate) :
                            base( game, null, handleMenuOptionDelegate, null, null)
        {
            this.levelsLevelReader = levelsLevelReader;
            LoadContent(game.Content);
            cursorTex = game.Content.Load<Texture2D>(@"cursor");
            hudFont = game.Content.Load<SpriteFont>(@"HUDFont");
            EditorTileSize = Game1.TILE_SIZE / 2;
            camera = new Camera(EditorTileSize*30, EditorTileSize, 1);
            cursor = new Cursor(cursorTex, new Vector2(gameAreaStartPos.X, gameAreaStartPos.Y), EditorTileSize);
            keyState = Keyboard.GetState();
        }

        public Matrix ViewMatrix
        {
            get { return camera.ViewMatrix; }
        }

        public int EditorTileSize { get; private set; }

        public Level CurrentLevel
        {
            get
            {
                Level result = null;
                if (currentlevel < levelsLevelReader.Count)
                {
                    result = levelsLevelReader[currentlevel];
                }
                return result;
            }
        }

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
            currentlevel = 0;
            NextLevel();
        }

        private void NextLevel()
        {
            ResetLevel();
            currentlevel++;
            if (currentlevel >= levelsLevelReader.Count)
            {
                currentlevel = 0;
            }

            CreateLevel(CurrentLevel);
        }

        public void PreviousLevel()
        {
            ResetLevel();
            currentlevel--;
            if (currentlevel < 0)
            {
                currentlevel = levelsLevelReader.Count - 1;
            }
            CreateLevel(CurrentLevel);
        }

        public void CreateLevel(Level level)
        {
            CreateLevel(level, EditorTileSize, gameAreaStartPos);
            camera.GameAreaTilesWidth = level.Cols;
            cursor.TilesWidth = level.Cols;
            cursor.TilesHeight = level.Rows;
            int tilePosY = (CurrentLevel != null) ? CurrentLevel.Rows - 1 : 0;
            cursor.SetPos(0, tilePosY);
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            if (pengo != null)
            {
                pengo.Update(true);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(true);
            }
        }

        private void IncreaseCols()
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.AddColumn(KeyList.EmptyTileKey.Char, KeyList.Option0Key.Char);
                cursor.TilesWidth = CurrentLevel.Cols;
            }
        }

        private void DecreaseCols()
        {
            if (CurrentLevel != null)
            {
                if (cursor.Col == (CurrentLevel.Cols - 1))
                {
                    int col = CurrentLevel.Cols - 2;
                    if (col < 0)
                    {
                        col = 0;
                    }
                    cursor.SetPos(col, cursor.Col);
                }
                for (int row = 0; row < CurrentLevel.Rows; row++)
                {
                    RemoveTile(CurrentLevel.Cols - 1, row);
                    cursor.TilesWidth = CurrentLevel.Cols;
                }
                CurrentLevel.DeleteColumn(CurrentLevel.Cols - 1);
            }
        }

        private void DeleteLevel()
        {
            if (CurrentLevel != null)
            {
                levelsLevelReader.DeleteLevel(CurrentLevel);
                NextLevel();
            }
        }

        private void AddLevel()
        {
            int tiles = CurrentLevel != null ? CurrentLevel.Rows : DEFAULT_ROWS_IN_LEVEL;
            levelsLevelReader.AddLevel(KeyList.EmptyTileKey.Char, KeyList.Option0Key.Char, tiles, tiles);
        }

        private void SaveLevels()
        {
            levelsLevelReader.WriteFile();
        }

        private void ResetLevel()
        {
            if (CurrentLevel != null)
            {
                for (int row = 0; row < CurrentLevel.Rows; row++)
                {
                    for (int col = 0; col < CurrentLevel.Cols; col++)
                    {
                        RemoveTile(col, row);
                    }
                }
            }
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
                CheckKeys(cursor.Col,cursor.Row, keys);
            }
        }

        private void CheckKeys(int col, int row, Keys[] keys)
        {
            if (keys.Length == 1 && CurrentLevel != null)
            {
                foreach (KeyItem keyItem in KeyList.GameObjetItems)
                {
                    if (keyItem.Key != KeyList.MenuTileKey.Key && WasKeyPressed(keyItem.Key))
                    {
                        LevelItem levelItem = CurrentLevel.Get(row, col);
                        ChangeTileObject(col, row, keyItem, levelItem);
                    }

                }
                foreach (KeyItem keyItem in KeyList.OptionItems)
                {
                    if (WasKeyPressed(keyItem.Key))
                    {
                        LevelItem levelItem = CurrentLevel.Get(row, col);
                        ChangeOptionObject(col, row, keyItem, levelItem);
                    }

                }
            }
            else
            {
                if(keys.Contains(Keys.LeftControl))
                {
                    if (WasKeyPressed(Keys.S))
                    {
                        SaveLevels();
                    }
                    if (WasKeyPressed(Keys.N))
                    {
                        AddLevel();
                    }
                    if (WasKeyPressed(Keys.Left) && keys.Contains(Keys.LeftAlt) && CurrentLevel != null)
                    {
                        DecreaseCols();
                    }
                    if (WasKeyPressed(Keys.Right) && keys.Contains(Keys.LeftAlt) && CurrentLevel != null)
                    {
                        IncreaseCols();
                    }
                    if (WasKeyPressed(Keys.Up) && keys.Contains(Keys.LeftAlt) && CurrentLevel != null)
                    {
                        NextLevel();
                    }
                    if (WasKeyPressed(Keys.Down) && keys.Contains(Keys.LeftAlt) && CurrentLevel != null)
                    {
                        PreviousLevel();
                    }
                    if (WasKeyPressed(Keys.D) && keys.Contains(Keys.LeftShift) && CurrentLevel != null)
                    {
                        DeleteLevel();
                    }
                }
            }
        }

        private void ChangeTileObject(int cursorXPos, int cursorYPos, KeyItem keyItem, LevelItem levelItem)
        {
            RemoveTile(cursorXPos, cursorYPos);

            char option = KeyList.Option0Key.Char;

            if (levelItem.GameObject == KeyList.BackgroundKey.Char)
            {
                backgrounds.DisableBackground(Convert.ToInt32(Math.Pow(2, cursorXPos)));
            }
            if (keyItem.Key == KeyList.FloorTileKey.Key)
            {
                AddFloortile(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.PengoKey.Key)
            {
                NewPengo(ScreenPos(cursorXPos, cursorYPos));
                if (pengo != null)
                {
                    pengo.Update(true);
                }
            }
            if (keyItem.Key == KeyList.WaterTileKey.Key)
            {
                AddWaterTile(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.TrapKey.Key)
            {
                AddTrap(ScreenPos(cursorXPos, cursorYPos), option == '1');
            }
            if (keyItem.Key == KeyList.EnemyKey.Key)
            {
                Enemy enemy = AddEnemy(ScreenPos(cursorXPos, cursorYPos));
                if (enemy != null)
                {
                    enemy.Update(true);
                }
            }
            if (keyItem.Key == KeyList.LadderKey.Key)
            {
                AddLadder(ScreenPos(cursorXPos, cursorYPos));
            }
            if (keyItem.Key == KeyList.CoinKey.Key)
            {
                if (CurrentLevel.Get(cursorXPos, cursorYPos).GameObject == KeyList.CoinKey.Char)
                {
                    option = CurrentLevel.Get(cursorYPos, cursorXPos).Option;
                }
                AddCoin(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            if (keyItem.Key == KeyList.MenuTileKey.Key)
            {
                if (CurrentLevel.Get(cursorYPos, cursorXPos).GameObject == KeyList.MenuTileKey.Char)
                {
                    option = CurrentLevel.Get(cursorYPos, cursorXPos).Option;
                }
                AddMenuTile(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            if (keyItem.Key == KeyList.RubyTileKey.Key)
            {
                if (CurrentLevel.Get(cursorYPos, cursorXPos).GameObject == KeyList.RubyTileKey.Char)
                {
                    option = CurrentLevel.Get(cursorYPos, cursorXPos).Option;
                }
                AddRubyTile(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            if (keyItem.Key == KeyList.GoalTileKey.Key)
            {
                if (CurrentLevel.Get(cursorYPos, cursorXPos).GameObject == KeyList.GoalTileKey.Char)
                {
                    option = CurrentLevel.Get(cursorYPos, cursorXPos).Option;
                }
                AddGoalTile(ScreenPos(cursorXPos, cursorYPos), option, null);
            }
            if (keyItem.Key == KeyList.BackgroundKey.Key)
            {
                if (CurrentLevel.Get(cursorYPos, cursorXPos).GameObject == KeyList.BackgroundKey.Char)
                {
                    option = CurrentLevel.Get(cursorYPos, cursorXPos).Option;
                }
                if( option == '0')
                {
                    backgrounds.DisableBackground(Convert.ToInt32(Math.Pow(2, cursorXPos)));
                }
                else
                {
                    backgrounds.EnableBackground(Convert.ToInt32(Math.Pow(2, cursorXPos)));
                }
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

            OptionCollisionTile mt = FindMenuTile(cursorXPos, cursorYPos);
            menuTiles.Remove(mt);

            OptionCollisionTile rt = FindRubyTile(cursorXPos, cursorYPos);
            rubyTiles.Remove(rt);

            OptionCollisionTile gt = FindGoalTile(cursorXPos, cursorYPos);
            goalTiles.Remove(gt);
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
            if (pengo!=null && pengo.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
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

        private OptionCollisionTile FindMenuTile(int cursorXPos, int cursorYPos)
        {
            OptionCollisionTile result = null;
            foreach (OptionCollisionTile menuTile in menuTiles)
            {
                if (menuTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = menuTile;
                }
            }
            return result;
        }

        private OptionCollisionTile FindRubyTile(int cursorXPos, int cursorYPos)
        {
            OptionCollisionTile result = null;
            foreach (OptionCollisionTile rubyTile in rubyTiles)
            {
                if (rubyTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = rubyTile;
                }
            }
            return result;
        }

        private OptionCollisionTile FindGoalTile(int cursorXPos, int cursorYPos)
        {
            OptionCollisionTile result = null;
            foreach (OptionCollisionTile goalTile in goalTiles)
            {
                if (goalTile.pos.Equals(ScreenPos(cursorXPos, cursorYPos)))
                {
                    result = goalTile;
                }
            }
            return result;
        }


        private void ChangeOptionObject(int col, int row, KeyItem keyItem, LevelItem levelItem)
        {
            //FloorTile ft = FindFlorTile(col, row);

            //Pengo pengo = FindPengo(col, row);

            //WaterTile wt = FindWaterTile(col, row);

            Trap trap = FindTrap(col, row);
            trap.Rotated = keyItem.Char == '1';

            //Enemy enemy = FindEnemy(col, row);

            //Ladder ladder = FindLadder(col, row);

            Coin coin = FindCoin(col, row);
            if (coin != null)
            {
                coin.SetOption(keyItem.Char);
            }

            OptionCollisionTile mt = FindMenuTile(col, row);
            if (mt != null)
            {
                mt.SetOption(keyItem.Char);
            }

            OptionCollisionTile rt = FindRubyTile(col, row);
            if (rt != null)
            {
                rt.SetOption(keyItem.Char);
            }

            OptionCollisionTile gt = FindGoalTile(col, row);
            if (gt != null)
            {
                gt.SetOption(keyItem.Char);
            }

            levelItem.Option = keyItem.Char;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatchObject.Scale = 1.0f * EditorTileSize / Game1.TILE_SIZE;
            backgrounds.Draw(spriteBatch);
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
            foreach (OptionCollisionTile menuTile in menuTiles)
            {
                menuTile.Draw(spriteBatch);
            }
            DrawGrid(spriteBatch);
            cursor.Draw(spriteBatch);
            SpriteBatchObject.Scale = 1f;
            DrawTileData(spriteBatch, cursor.Col, cursor.Row);
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            if (CurrentLevel != null)
            {
                Texture2D tex = new Texture2D(game.GraphicsDevice, 1, 1);
                tex.SetData<Color>(
                    new Color[] {Color.White}); // fill the texture with white

                for (int rows = 0; rows <= CurrentLevel.Rows; rows++)
                {
                    DrawLine(spriteBatch, tex, //draw line
                        new Vector2(gameAreaStartPos.X,
                            gameAreaStartPos.Y + rows*EditorTileSize), //start of line
                        new Vector2(gameAreaStartPos.X + CurrentLevel.Cols*EditorTileSize,
                            gameAreaStartPos.Y + rows*EditorTileSize), //end of line
                        Color.Black);
                }
                for (int cols = 0; cols <= CurrentLevel.Cols; cols++)
                {
                    DrawLine(spriteBatch, tex, //draw line
                        new Vector2(gameAreaStartPos.X + cols*EditorTileSize,
                            gameAreaStartPos.Y), //start of line
                        new Vector2(gameAreaStartPos.X + cols*EditorTileSize,
                            gameAreaStartPos.Y + CurrentLevel.Rows*EditorTileSize), //end of line
                        Color.Black);
                }
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

        private void DrawTileData(SpriteBatch spriteBatch, int col, int row)
        {
            if (CurrentLevel != null)
            {
                string text;

                LevelItem levelItem = CurrentLevel.Get(row, col);
                text = "Tile: " + levelItem.GameObject;
                DrawString(spriteBatch, text, 0);

                text = "Option: " + levelItem.Option;
                DrawString(spriteBatch, text, 1);

                text = "CurrentLevel: " + currentlevel;
                DrawString(spriteBatch, text, 2);

                text = "Cols: " + CurrentLevel.Cols + " Rows: " + CurrentLevel.Rows;
                DrawString(spriteBatch, text, 3);

                text = "Col: " + cursor.Col + " Row: " + cursor.Row;
                DrawString(spriteBatch, text, 4);

                text = "Levels: " + levelsLevelReader.Count;
                DrawString(spriteBatch, text, 5);

                text = "Ctrl-S: Save to file";
                DrawString(spriteBatch, text, 6);

                text = "Ctrl-N: New level";
                DrawString(spriteBatch, text, 7);

                text = "Alt-Crtl-LeftArrow: Decrease columns";
                DrawString(spriteBatch, text, 8);

                text = "Alt-Ctrl-RightArrow: Increase columns";
                DrawString(spriteBatch, text, 9);

                text = "Alt-Crtl-UpArrow: Next level";
                DrawString(spriteBatch, text, 10);

                text = "Alt-Ctrl-DownArrow: Previous level";
                DrawString(spriteBatch, text, 11);

                text = "Shift-Ctrl-D: Delete level";
                DrawString(spriteBatch, text, 12);
            }
        }

        private void DrawString(SpriteBatch spriteBatch, string textString, int textRow)
        {
            spriteBatch.DrawString(hudFont, textString,
                new Vector2(TËXTOFFSET_X - camera.ViewMatrix.Translation.X,
                gameAreaStartPos.Y + EditorTileSize * CurrentLevel.Rows + FIRST_TËXTOFFSET_Y + ADD_TËXTOFFSET_Y * textRow), Color.White);
        }
    }
}
