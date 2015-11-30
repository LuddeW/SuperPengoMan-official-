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
            camera = new Camera();
        }

        public void CreateLevel(Level level)
        {
            for (int row = 0; row < level.Rows; row++)
            {
                for (int col = 0; col < level.Cols; col++)
                {
                    ObjectFactory(level.Get(row, col).GameObject, level.Get(row, col).Option, row, col);
                }
            }
        }

        private void ObjectFactory(char objectChar, char option, int row, int col)
        {
            Vector2 pos = new Vector2(Game1.TILE_SIZE * col, Game1.TILE_SIZE * row);
            switch (objectChar)
            {
                case 'F':
                    floortile.Add(new FloorTile(iceTile, pos));
                    break;
                case 'S':
                    pengoRespawnPos = pos;
                    pengo = new Pengo(penguin, pos);
                    break;
                case 'W':
                    watertile.Add(new WaterTile(waterTile, pos));
                    break;
                case 'T':
                    trap.Add(new Trap(spike, pos));
                    break;
                case 'E':
                    enemy = new Enemy(snowball, pos);
                    break;
                case 'L':
                    ladder.Add(new Ladder(ladderTile, pos));
                    break;
                case 'M':
                    menuTiles.Add(new OptionCollisionTile(waterTile, pos, option, handleOptionDelegate));
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
