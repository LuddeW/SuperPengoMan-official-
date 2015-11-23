﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPengoMan.GameObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class HandleGame
    {
        Game game;

        Texture2D coin;
        Texture2D penguin;
        Texture2D penguin_jump;
        Texture2D penguin_glide;
        Texture2D iceTile;
        Texture2D caveBackground;
        Texture2D waterTile;
        Texture2D spike;
        Texture2D snowball;
        Texture2D ladderTile;
        Texture2D penguin_climb;

        Background backgrounds;

        Vector2 pengoRespawnPos;
        Point gameTiles = new Point(0,0);

        private List<FloorTile> floortile = new List<FloorTile>();
        private List<WaterTile> watertile = new List<WaterTile>();
        private List<Trap> traps = new List<Trap>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<Ladder> ladders = new List<Ladder>();
        private List<Coin> coins = new List<Coin>();
        private List<MenuTile> menuTiles = new List<MenuTile>();

        Pengo pengo;
        Camera camera;
        private Game1.AddPointsDelegate addPointsDelegate;
        private Game1.HandleMenuOptionDelegate handleMenuOptionDelegate;

        public HandleGame(Game game, Game1.AddPointsDelegate addPointsDelegate, Game1.HandleMenuOptionDelegate handleMenuOptionDelegate)
        {
            this.game = game;
            this.addPointsDelegate = addPointsDelegate;
            this.handleMenuOptionDelegate = handleMenuOptionDelegate;
        }

        public Camera PengoCamera
        {
            get { return camera; }
            set { camera = value; }
        }

        public void LoadContent()
        {


            coin = game.Content.Load<Texture2D>(@"coin");
            penguin = game.Content.Load<Texture2D>(@"penguin_spritesheet");
            penguin_jump = game.Content.Load<Texture2D>(@"penguin_jump");
            penguin_glide = game.Content.Load<Texture2D>(@"penguin_glide");
            penguin_climb = game.Content.Load<Texture2D>(@"penguin_climb");
            iceTile = game.Content.Load<Texture2D>(@"ice_tile");
            waterTile = game.Content.Load<Texture2D>(@"water_tile");
            spike = game.Content.Load<Texture2D>(@"spike");
            snowball = game.Content.Load<Texture2D>(@"snowball");
            ladderTile = game.Content.Load<Texture2D>(@"Ladder");
           
            camera = new Camera();
        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();
            pengo.Update();
            foreach (MenuTile menuTile in menuTiles)
            {
                menuTile.IsColliding(pengo.hitbox);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
            }
            foreach (FloorTile iceTile in floortile)
            {
                if (pengo.IsColliding(iceTile.TopHitbox, iceTile.LeftHitbox,iceTile.RightHitbox))
                {
                    pengo.HandleCollision(iceTile.TopHitbox);
                }
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.IsColliding(iceTile.LeftHitbox, iceTile.RightHitbox))
                    {
                        enemy.HandleCollision();
                    }
                }
            }
            foreach (Trap spike in traps)
            {
                if (spike.PixelCollition(pengo.texture, pengo.srcRect, pengo.hitbox))
                {
                    pengo.KillPengo(pengoRespawnPos);
                }
            }
            foreach (Ladder ladderTile in ladders)
            {
                if (ladderTile.hitbox.Intersects(pengo.hitbox))
                {
                    pengo.isOnLadder = true;
                }
            }
            backgrounds.Update();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.hitbox.Intersects(pengo.hitbox) && pengo.speed.Y >= 5)
                {
                    pengo.KillPengo(pengoRespawnPos);
                }
                else if (enemy.topHitbox.Intersects(pengo.hitbox))
                {
                    Console.WriteLine("You killed it");
                }
            }
            if (pengo.pos.Y >= Game1.TILE_SIZE * (gameTiles.Y - 1))
            {
                pengo.KillPengo(pengoRespawnPos);
            }
            foreach (Coin coin in coins)
            {
                coin.IsColliding(pengo.hitbox);
            }
            camera.Update(pengo.pos);
            
        }

        public void CreateLevel(Level level)
        {
            gameTiles.Y = level.Rows;
            gameTiles.X = level.Cols;
            backgrounds = new Background(game, gameTiles);
            for (int row = 0; row < level.Rows; row++)
            {
                for (int col = 0; col < level.Cols; col++)
                {
                    ObjectFactory(level.Get(row,col).GameObject, level.Get(row, col).Option, row, col);
                }
            }
        }

        private void ObjectFactory(char gameObject, char option, int row, int col)
        {
            Vector2 pos = new Vector2(Game1.TILE_SIZE * col, Game1.TILE_SIZE * row);
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
                    enemies.Add( new Enemy(snowball, pos));
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

        public void Draw(SpriteBatch spriteBatch)
        {

            backgrounds.Draw(spriteBatch);
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
            pengo.Draw(spriteBatch);
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
  
