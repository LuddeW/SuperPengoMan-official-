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

        public LevelEditor(Game game, LevelReader levelReader, Game1.HandleOptionDelegate handleOptionDelegate) : base(game, handleOptionDelegate)
        {
            this.levelReader = levelReader;
            EditorTileSize = Game1.TILE_SIZE / 2;
            camera = new Camera(EditorTileSize * 45, EditorTileSize, 1);
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
            SpriteBatchObject.Scale = 1f;
        }
    }
}
