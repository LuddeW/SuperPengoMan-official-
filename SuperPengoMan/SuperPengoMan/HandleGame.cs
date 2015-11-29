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

        public HandleGame(Game game, Game1.AddPointsDelegate addPointsDelegate, 
                                Game1.HandleOptionDelegate handleMenuOptionDelegate,
                                Game1.HandleOptionDelegate handleGoalOptionDelegate,
                                Game1.HandleOptionDelegate handleRubyOptionDelegate) :
                                base(addPointsDelegate, handleMenuOptionDelegate,
                                                            handleGoalOptionDelegate,
                                                            handleRubyOptionDelegate)
        {
            this.game = game;
        }

        public Matrix ViewMatrix
        {
            get { return camera.ViewMatrix; }
        }

        public void LoadContent()
        {

            LoadContent(game.Content);
            camera = new Camera(Game1.TILE_SIZE*15, Game1.TILE_SIZE, 1);
           
        }

        public void CreateLevel(Level level)
        {
            camera.GameAreaTilesWidth = level.Cols;
            gameTiles.Y = level.Rows;
            gameTiles.X = level.Cols;
            backgrounds = new Background(game, gameTiles);
            CreateLevel(level, Game1.TILE_SIZE, new Point(0, 0));
        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                handleMenuOptionDelegate('0');
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }
            else
            {
                pengo.Update();
                foreach (OptionCollisionTile menuTile in menuTiles)
                {
                    menuTile.IsColliding(pengo.hitbox);
                }
                foreach (OptionCollisionTile goalTile in goalTiles)
                {
                    goalTile.IsColliding(pengo.hitbox);
                }
                foreach (OptionCollisionTile rubyTile in rubyTiles)
                {
                    rubyTile.IsColliding(pengo.hitbox);
                }
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update();
                }
                foreach (FloorTile iceTile in floorTiles)
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
                    if (spike.PixelCollition(pengo.TexData, pengo.TexWidth, pengo.srcRect, pengo.hitbox))
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {

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
            pengo.Draw(spriteBatch);
            foreach (WaterTile waterTile in waterTiles)
            {
                waterTile.Draw(spriteBatch);
            }
            foreach (OptionCollisionTile menuTile in menuTiles)
            {
                menuTile.Draw(spriteBatch);
            }
        }
    }
}
  
