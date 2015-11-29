using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPengoMan.GameObject;


namespace SuperPengoMan
{
    internal class KeyItem
    {
        public char Char { get; set; }
        public Keys Key { get; set; }

        public KeyItem(char ch, Keys key)
        {
            Char = ch;
            Key = key;
        }
    }

    internal static class KeyList
    {
        public static List<KeyItem> GameObjetItems = new List<KeyItem>();
        public static List<KeyItem> OptionItems = new List<KeyItem>();

        public static void Init()
        {
            GameObjetItems.Add(EmptyTileKey);
            GameObjetItems.Add(FloorTileKey);
            GameObjetItems.Add(PengoKey);
            GameObjetItems.Add(WaterTileKey);
            GameObjetItems.Add(TrapKey);
            GameObjetItems.Add(EnemyKey);
            GameObjetItems.Add(LadderKey);
            GameObjetItems.Add(CoinKey);
            GameObjetItems.Add(MenuTileKey);
            GameObjetItems.Add(RubyTileKey);
            GameObjetItems.Add(GoalTileKey);
            GameObjetItems.Add(BackgroundKey);

            OptionItems.Add(Option0Key);
            OptionItems.Add(Option1Key);
            OptionItems.Add(Option2Key);
            OptionItems.Add(Option3Key);
            OptionItems.Add(Option4Key);
            OptionItems.Add(Option5Key);
            OptionItems.Add(Option6Key);
            OptionItems.Add(Option7Key);
            OptionItems.Add(Option8Key);
            OptionItems.Add(Option9Key);
        }

        public static KeyItem EmptyTileKey  = new KeyItem('A', Keys.A);
        public static KeyItem FloorTileKey  = new KeyItem('F', Keys.F);
        public static KeyItem PengoKey      = new KeyItem('S', Keys.S);
        public static KeyItem WaterTileKey  = new KeyItem('W', Keys.W);
        public static KeyItem TrapKey       = new KeyItem('T', Keys.T);
        public static KeyItem EnemyKey      = new KeyItem('E', Keys.E);
        public static KeyItem LadderKey     = new KeyItem('L', Keys.L);
        public static KeyItem CoinKey       = new KeyItem('C', Keys.C);
        public static KeyItem MenuTileKey   = new KeyItem('M', Keys.M);
        public static KeyItem GoalTileKey   = new KeyItem('G', Keys.G);
        public static KeyItem RubyTileKey   = new KeyItem('R', Keys.R);
        public static KeyItem BackgroundKey = new KeyItem('B', Keys.B);

        public static KeyItem Option0Key    = new KeyItem('0', Keys.D0);
        public static KeyItem Option1Key    = new KeyItem('1', Keys.D1);
        public static KeyItem Option2Key    = new KeyItem('2', Keys.D2);
        public static KeyItem Option3Key    = new KeyItem('3', Keys.D3);
        public static KeyItem Option4Key    = new KeyItem('4', Keys.D4);
        public static KeyItem Option5Key    = new KeyItem('5', Keys.D5);
        public static KeyItem Option6Key    = new KeyItem('6', Keys.D6);
        public static KeyItem Option7Key    = new KeyItem('7', Keys.D7);
        public static KeyItem Option8Key    = new KeyItem('8', Keys.D8);
        public static KeyItem Option9Key    = new KeyItem('9', Keys.D9);
    }

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
        internal Texture2D goalTile;
        internal Texture2D rubyTile;

        internal List<FloorTile> floorTiles = new List<FloorTile>();
        internal List<WaterTile> waterTiles = new List<WaterTile>();
        internal List<Trap> traps = new List<Trap>();
        internal List<Enemy> enemies = new List<Enemy>();
        internal List<Ladder> ladders = new List<Ladder>();
        internal List<Coin> coins = new List<Coin>();
        internal List<OptionCollisionTile> menuTiles = new List<OptionCollisionTile>();
        internal List<OptionCollisionTile> rubyTiles = new List<OptionCollisionTile>();
        internal List<OptionCollisionTile> goalTiles = new List<OptionCollisionTile>();
        internal Pengo pengo = null;

        internal Vector2 pengoRespawnPos;
        internal Game1.AddPointsDelegate addPointsDelegate = null;
        internal Game1.HandleOptionDelegate handleMenuOptionDelegate = null;
        internal Game1.HandleOptionDelegate handleGoalOptionDelegate = null;
        internal Game1.HandleOptionDelegate handleRubyOptionDelegate = null;

        public ObjectHandler(Game1.AddPointsDelegate addPointsDelegate, 
            Game1.HandleOptionDelegate handleMenuOptionDelegate, 
            Game1.HandleOptionDelegate handleGoalOptionDelegate,
            Game1.HandleOptionDelegate handleRubyOptionDelegate)
        {
            this.addPointsDelegate = addPointsDelegate;
            this.handleMenuOptionDelegate = handleMenuOptionDelegate;
            this.handleGoalOptionDelegate = handleGoalOptionDelegate;
            this.handleRubyOptionDelegate = handleRubyOptionDelegate;
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
            goalTile = content.Load<Texture2D>(@"goal");
            rubyTile = content.Load<Texture2D>(@"ruby");
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

        internal Vector2 ScreenPos(Point StartPos, int tileSize, int ixXPos, int ixYPos)
        {
            return new Vector2(StartPos.X + tileSize * ixXPos, StartPos.Y + tileSize * ixYPos);
        }

        private void ObjectFactory(char gameObject, char option, int row, int col, int tileSize, Point StartPos)
        {
            Vector2 pos = ScreenPos(StartPos, tileSize, col, row);
            if (gameObject == KeyList.FloorTileKey.Char)
            {
                AddFloortile(pos);
            }
            if (gameObject == KeyList.PengoKey.Char)
            {
                pengoRespawnPos = pos;
                NewPengo(pos);
            }
            if (gameObject == KeyList.WaterTileKey.Char)
            {
                AddWaterTile(pos);
            }
            if (gameObject == KeyList.TrapKey.Char)
            {
                AddTrap(pos);
            }
            if (gameObject == KeyList.EnemyKey.Char)
            {
                AddEnemy(pos);
            }
            if (gameObject == KeyList.LadderKey.Char)
            {
                AddLadder(pos);
            }
            if (gameObject == KeyList.CoinKey.Char)
            {
                AddCoin(pos, option, addPointsDelegate);
            }
            if (gameObject == KeyList.MenuTileKey.Char)
            {
                AddMenuTile(pos, option, handleMenuOptionDelegate);
            }
            if (gameObject == KeyList.GoalTileKey.Char)
            {
                AddGoalTile(pos, option, handleGoalOptionDelegate);
            }
            if (gameObject == KeyList.RubyTileKey.Char)
            {
                AddRubyTile(pos, option, handleRubyOptionDelegate);
            }
        }

        internal void AddFloortile(Vector2 pos)
        {
            floorTiles.Add(new FloorTile(iceTile, pos));
        }

        internal void NewPengo(Vector2 pos)
        {
            pengo = new Pengo(penguin, penguin_glide, penguin_jump, penguin_climb, pos);
        }

        internal void AddWaterTile(Vector2 pos)
        {
            waterTiles.Add(new WaterTile(waterTile, pos));
        }

        internal void AddTrap(Vector2 pos)
        {
            traps.Add(new Trap(spike, pos));
        }

        internal Enemy AddEnemy(Vector2 pos)
        {
            Enemy result = new Enemy(snowball, pos);
            enemies.Add(result);
            return result;
        }

        internal void AddLadder(Vector2 pos)
        {
            ladders.Add(new Ladder(ladderTile, pos));
        }

        internal void AddCoin(Vector2 pos, char option, Game1.AddPointsDelegate addPointsDelegate1)
        {
            coins.Add(new Coin(coin, pos, option, addPointsDelegate));
        }

        internal void AddMenuTile(Vector2 pos, char option, Game1.HandleOptionDelegate handleMenuOptionDelegate1)
        {
            menuTiles.Add(new OptionCollisionTile(waterTile, pos, option, handleMenuOptionDelegate));
        }

        internal void AddGoalTile(Vector2 pos, char option, Game1.HandleOptionDelegate handleGoalyOptionDelegate1)
        {
            goalTiles.Add(new OptionCollisionTile(goalTile, pos, option, handleGoalOptionDelegate));
        }

        internal void AddRubyTile(Vector2 pos, char option, Game1.HandleOptionDelegate handleRubyOptionDelegate1)
        {
            rubyTiles.Add(new OptionCollisionTile(rubyTile, pos, option, handleRubyOptionDelegate));
        }
    }
}