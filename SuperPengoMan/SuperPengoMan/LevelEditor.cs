using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class LevelEditor : ObjectHandler
    {

        int currentLevel = 0;
        LevelReader levelReader;
        Point gameAreaStartPos = new Point(0, 50);
        Texture2D cursortex;
        Cursor cursor;


        public LevelEditor(Game game, LevelReader levelReader, Game1.HandleOptionDelegate handleOptionDelegate) : base(game, handleOptionDelegate)
        {
            this.levelReader = levelReader;
            EditorTileSize = Game1.TILE_SIZE / 2;
            camera = new Camera(EditorTileSize * 45, EditorTileSize, 1);
            cursortex = game.Content.Load<Texture2D>(@"cursor");
            cursor = new Cursor(cursortex, new Vector2(gameAreaStartPos.X, gameAreaStartPos.Y), EditorTileSize);
            LoadContent();
        }

        public int EditorTileSize { get; private set; }

        public Level CurrentLevel
        {
            get
            {
                Level result = null;
                if (currentLevel < levelReader.Count)
                {
                    result = levelReader[currentLevel];
                }
                return result;
            }
        }

        public void Init()
        {
            currentLevel = 0;
            NextLevel();         
        }

        private void NextLevel()
        {
            currentLevel++;
            if (currentLevel >= levelReader.Count)
            {
                currentLevel = 0;
            }
            CreateLevel(CurrentLevel, EditorTileSize, gameAreaStartPos);
            cursor.TilesWidth = CurrentLevel.Cols;
            cursor.TilesHeight = CurrentLevel.Rows;
            camera.GameAreaTilesWidth = CurrentLevel.Cols;
            camera.Update(new Vector2(0, 0));
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            if (pengo != null)
            {
                pengo.Update();
            }
            if (enemy != null)
            {
                enemy.Update();
            }
        }

        public void Update()
        {
            cursor.Update();
            camera.Update(cursor.pos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatchObject.Scale = 1.0f * EditorTileSize / Game1.TILE_SIZE;
            foreach (Ladder ladderTile in ladder)
            {
                ladderTile.Draw(spriteBatch);
            }
            enemy.Draw(spriteBatch);
            pengo.Draw(spriteBatch);
            foreach (FloorTile iceTile in floortile)
            {
                iceTile.Draw(spriteBatch);
            }
            foreach (WaterTile waterTile in watertile)
            {
                waterTile.Draw(spriteBatch);
            }
            foreach (Trap spike in trap)
            {
                spike.Draw(spriteBatch);
            }
            foreach (OptionCollisionTile menuTile in menuTiles)
            {
                menuTile.Draw(spriteBatch);
            }
            DrawGrid(spriteBatch);
            cursor.Draw(spriteBatch);
            SpriteBatchObject.Scale = 1f;
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            if (CurrentLevel != null)
            {
                Texture2D tex = new Texture2D(game.GraphicsDevice, 1, 1);
                tex.SetData<Color>(
                    new Color[] { Color.White }); // fill the texture with white

                for (int rows = 0; rows <= CurrentLevel.Rows; rows++)
                {
                    DrawLine(spriteBatch, tex, //draw line
                        new Vector2(gameAreaStartPos.X,
                            gameAreaStartPos.Y + rows * EditorTileSize), //start of line
                        new Vector2(gameAreaStartPos.X + CurrentLevel.Cols * EditorTileSize,
                            gameAreaStartPos.Y + rows * EditorTileSize), //end of line
                        Color.Black);
                }
                for (int cols = 0; cols <= CurrentLevel.Cols; cols++)
                {
                    DrawLine(spriteBatch, tex, //draw line
                        new Vector2(gameAreaStartPos.X + cols * EditorTileSize,
                            gameAreaStartPos.Y), //start of line
                        new Vector2(gameAreaStartPos.X + cols * EditorTileSize,
                            gameAreaStartPos.Y + CurrentLevel.Rows * EditorTileSize), //end of line
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


    }
}
