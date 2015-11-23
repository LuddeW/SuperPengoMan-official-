using Microsoft.Xna.Framework;
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
    class HandleGame: ObjectHandler
    {
        Game game;

        Background backgrounds;

        Point gameTiles = new Point(0,0);

        Camera camera;

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

            LoadContent(game.Content);
            camera = new Camera();
        }

        public void CreateLevel(Level level)
        {
            gameTiles.Y = level.Rows;
            gameTiles.X = level.Cols;
            backgrounds = new Background(game, gameTiles);
            CreateLevel(level, Game1.TILE_SIZE, new Point(0, 0));
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
  
