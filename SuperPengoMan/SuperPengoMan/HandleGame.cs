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
    class HandleGame : ObjectHandler
    {

        public HandleGame(Game game, Game1.HandleOptionDelegate handleOptionDelegate) : base(game, handleOptionDelegate)
        {
             
        }
       
        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Back))
                handleOptionDelegate('2');

            pengo.Update();
            enemy.Update();
            HandleFloorTile();
            HandleTraps();
            HandleLadderTiles();
            HandleMenuTiles();
            HandlePengo();          
            backgrounds.Update();
            camera.Update(pengo.pos);           
        }

        private void HandleFloorTile()
        {
            foreach (FloorTile iceTile in floortile)
            {
                if (pengo.IsColliding(iceTile))
                {
                    pengo.HandleCollision(iceTile);
                }
                if (enemy.IsColliding(iceTile))
                {
                    enemy.HandleCollision();
                }
            }
        }

        private void HandleTraps()
        {
            foreach (Trap spike in trap)
            {
                if (spike.PixelCollition(pengo))
                {
                    pengo.KillPengo(pengoRespawnPos);
                }
            }
        }

        private void HandleLadderTiles()
        {
            foreach (Ladder ladderTile in ladder)
            {
                if (ladderTile.hitbox.Intersects(pengo.hitbox))
                {
                    pengo.isOnLadder = true;
                }
            }
        }

        private void HandleMenuTiles()
        {
            foreach (OptionCollisionTile menuTile in menuTiles)
            {
                menuTile.IsColliding(pengo.hitbox);
            }
        }

        private void HandlePengo()
        {
            if (enemy.hitbox.Intersects(pengo.hitbox) && pengo.speed.Y >= 5)
            {
                pengo.KillPengo(pengoRespawnPos);
            }
            else if (enemy.topHitbox.Intersects(pengo.hitbox))
            {
                Console.WriteLine("You killed it");
            }
            if (pengo.pos.Y >= Game1.TILE_SIZE * 14)
            {
                pengo.KillPengo(pengoRespawnPos);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            backgrounds.Draw(spriteBatch);
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
        }
    }
}
  
