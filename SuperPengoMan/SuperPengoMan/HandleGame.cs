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
    class HandleGame
    {
        Game game;

        Texture2D penguin;
        Texture2D penguin_jump;
        Texture2D penguin_glide;
        Texture2D iceTile;
        Texture2D background;
        Texture2D caveBackground;
        Texture2D waterTile;
        Texture2D spike;
        Texture2D snowball;
        Texture2D ladderTile;
        Texture2D penguin_climb;

        Background backgrounds;

        Vector2 pengoRespawnPos;

        List<FloorTile> floortile = new List<FloorTile>();
        List<WaterTile> watertile = new List<WaterTile>();
        List<Trap> trap = new List<Trap>();
        List<Ladder> ladder = new List<Ladder>();
        List<OptionCollisionTile> menuTiles = new List<OptionCollisionTile>();

        Game1.HandleOptionDelegate handleOptionDelegate;

        Pengo pengo;
        Enemy enemy;
        Camera camera;

        public HandleGame(Game game, Game1.HandleOptionDelegate handleOptionDelegate)
        {
            this.game = game;
            this.handleOptionDelegate = handleOptionDelegate;
            
        }
       
        public void LoadContent()
        {
            penguin = game.Content.Load<Texture2D>(@"penguin_spritesheet");
            penguin_jump = game.Content.Load<Texture2D>(@"penguin_jump");
            penguin_glide = game.Content.Load<Texture2D>(@"penguin_glide");
            penguin_climb = game.Content.Load<Texture2D>(@"penguin_climb");
            iceTile = game.Content.Load<Texture2D>(@"ice_tile");
            background = game.Content.Load<Texture2D>(@"background");
            caveBackground = game.Content.Load<Texture2D>(@"snowcave");
            waterTile = game.Content.Load<Texture2D>(@"water_tile");
            spike = game.Content.Load<Texture2D>(@"spike");
            snowball = game.Content.Load<Texture2D>(@"snowball");
            ladderTile = game.Content.Load<Texture2D>(@"Ladder");
            backgrounds = new Background(game.Content, game.Window);          
            camera = new Camera();
        }

        public Matrix ViewMatrix()
        {
            return camera.ViewMatrix;
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
                    pengo = new Pengo(penguin, penguin_glide, penguin_jump, penguin_climb, pos);
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
            }
        }

        public Vector2 GetPengoPos()
        {
            return pengo.pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, game.Window.ClientBounds.Height - background.Height - (1 * Game1.TILE_SIZE), background.Width, background.Height), Color.White);
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
  
