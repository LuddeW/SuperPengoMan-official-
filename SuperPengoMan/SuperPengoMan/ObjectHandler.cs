using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class ObjectHandler
    {
        protected Game game;

        protected Texture2D penguin;
        protected Texture2D iceTile;
        protected Texture2D caveBackground;
        protected Texture2D waterTile;
        protected Texture2D spike;
        protected Texture2D snowball;
        protected Texture2D ladderTile;

        protected Background backgrounds;

        protected List<FloorTile> floortile = new List<FloorTile>();
        protected List<WaterTile> watertile = new List<WaterTile>();
        protected List<Trap> trap = new List<Trap>();
        protected List<Ladder> ladder = new List<Ladder>();
        protected List<OptionCollisionTile> menuTiles = new List<OptionCollisionTile>();
        protected List<SpriteBatchObject> spriteBatchObjects = new List<SpriteBatchObject>();
        protected Pengo pengo;
        protected Enemy enemy;

        protected Game1.HandleOptionDelegate handleOptionDelegate;

        protected Camera camera;

        protected Vector2 pengoRespawnPos;

        public ObjectHandler(Game game, Game1.HandleOptionDelegate handleOptionDelegate)
        {
            this.game = game;
            this.handleOptionDelegate = handleOptionDelegate;
        }

        public void LoadContent()
        {
            penguin = game.Content.Load<Texture2D>(@"penguin_spritesheet");
            iceTile = game.Content.Load<Texture2D>(@"ice_tile");
            caveBackground = game.Content.Load<Texture2D>(@"snowcave");
            waterTile = game.Content.Load<Texture2D>(@"water_tile");
            spike = game.Content.Load<Texture2D>(@"spike");
            snowball = game.Content.Load<Texture2D>(@"snowball");
            ladderTile = game.Content.Load<Texture2D>(@"Ladder");
            backgrounds = new Background(game.Content, game.Window);
            spriteBatchObjects.Add(backgrounds);
            camera = new Camera(Game1.TILE_SIZE * 15, Game1.TILE_SIZE, 1);
        }

        public void CreateLevel(Level level, int tileSize, Point startPos)
        {
            camera.GameAreaTilesWidth = level.Cols;
            for (int row = 0; row < level.Rows; row++)
            {
                for (int col = 0; col < level.Cols; col++)
                {
                    ObjectFactory(level.Get(row, col).GameObject, level.Get(row, col).Option, row, col, tileSize, startPos);
                }
            }
        }

        private void ObjectFactory(char objectChar, char option, int row, int col, int tileSize, Point startPos)
        {
            Vector2 pos = new Vector2(tileSize * col + startPos.X, tileSize * row + startPos.Y);
            switch (objectChar)
            {
                case 'F':
                    FloorTile ft = new FloorTile(iceTile, pos);
                    floortile.Add(ft);
                    spriteBatchObjects.Add(ft);
                    break;
                case 'S':
                    pengoRespawnPos = pos;
                    pengo = new Pengo(penguin, pos);
                    spriteBatchObjects.Add(pengo);
                    break;
                case 'W':
                    WaterTile wt = new WaterTile(waterTile, pos);
                    watertile.Add(wt);
                    spriteBatchObjects.Add(wt);
                    break;
                case 'T':
                    Trap trp = new Trap(spike, pos);
                    trap.Add(trp);
                    spriteBatchObjects.Add(trp);
                    break;
                case 'E':
                    enemy = new Enemy(snowball, pos);
                    spriteBatchObjects.Add(enemy);
                    break;
                case 'L':
                    Ladder ldr = new Ladder(ladderTile, pos);
                    ladder.Add(ldr);
                    spriteBatchObjects.Add(ldr);
                    break;
                case 'M':
                    OptionCollisionTile mt = new OptionCollisionTile(waterTile, pos, option, handleOptionDelegate);
                    menuTiles.Add(mt);
                    spriteBatchObjects.Add(mt);
                    break;
                case 'B':
                    int optionValue = option == '0' ? 0 : 1;
                    backgrounds.EnableBackground(option * Convert.ToInt32(Math.Pow(2, col)));
                    break;
            }
        }

        public Matrix ViewMatrix()
        {
            return camera.ViewMatrix;
        }


    }
}
