using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;


namespace SuperPengoMan
{
    internal class ObjectHandler
    {
        internal Texture2D coin;
        internal Texture2D penguin;
        internal Texture2D penguin_jump;
        internal Texture2D penguin_glide;
        internal Texture2D iceTile;
        internal Texture2D waterTile;
        internal Texture2D spike;
        internal Texture2D snowball;
        internal Texture2D ladderTile;
        internal Texture2D penguin_climb;

        internal List<FloorTile> floortile = new List<FloorTile>();
        internal List<WaterTile> watertile = new List<WaterTile>();
        internal List<Trap> traps = new List<Trap>();
        internal List<Enemy> enemies = new List<Enemy>();
        internal List<Ladder> ladders = new List<Ladder>();
        internal List<Coin> coins = new List<Coin>();
        internal List<MenuTile> menuTiles = new List<MenuTile>();

        internal Pengo pengo = null;
        internal Vector2 pengoRespawnPos;
        internal Game1.AddPointsDelegate addPointsDelegate = null;
        internal Game1.HandleMenuOptionDelegate handleMenuOptionDelegate = null;

        public ObjectHandler(Game1.AddPointsDelegate addPointsDelegate, Game1.HandleMenuOptionDelegate handleMenuOptionDelegate)
        {
            this.addPointsDelegate = addPointsDelegate;
            this.handleMenuOptionDelegate = handleMenuOptionDelegate;
        }

        internal void LoadContent(ContentManager content)
        {
            String s = Directory.GetCurrentDirectory();

            coin = content.Load<Texture2D>(@"coin");
            penguin = content.Load<Texture2D>(@"penguin_spritesheet");
            penguin_jump = content.Load<Texture2D>(@"penguin_jump");
            penguin_glide = content.Load<Texture2D>(@"penguin_glide");
            penguin_climb = content.Load<Texture2D>(@"penguin_climb");
            iceTile = content.Load<Texture2D>(@"ice_tile");
            waterTile = content.Load<Texture2D>(@"water_tile");
            spike = content.Load<Texture2D>(@"spike");
            snowball = content.Load<Texture2D>(@"snowball");
            ladderTile = content.Load<Texture2D>(@"Ladder");
        }

        public void CreateLevel(Level level, int tileSize, Point StartPos)
        {
            for (int row = 0; row < level.Rows; row++)
            {
                for (int col = 0; col < level.Cols; col++)
                {
                    ObjectFactory(level.Get(row, col).GameObject, level.Get(row, col).Option, row, col, tileSize, StartPos);
                }
            }
        }

        private void ObjectFactory(char gameObject, char option, int row, int col, int tileSize, Point StartPos)
        {
            Vector2 pos = new Vector2(StartPos.X + tileSize * col, StartPos.Y + tileSize * row);
            switch (gameObject)
            {
                case 'F':
                    floortile.Add(new FloorTile(iceTile, pos));
                    break;
                case 'S':
                    pengoRespawnPos = pos;
                    pengo = new Pengo(penguin, penguin_glide, penguin_jump, penguin_climb, pos);
                    break;
                case 'W':
                    watertile.Add(new WaterTile(waterTile, pos));
                    break;
                case 'T':
                    traps.Add(new Trap(spike, pos));
                    break;
                case 'E':
                    enemies.Add(new Enemy(snowball, pos));
                    break;
                case 'L':
                    ladders.Add(new Ladder(ladderTile, pos));
                    break;
                case 'C':
                    coins.Add(new Coin(coin, pos, option, addPointsDelegate));
                    break;
                case 'M':
                    menuTiles.Add(new MenuTile(waterTile, pos, option, handleMenuOptionDelegate));
                    break;

            }
        }


    }
}