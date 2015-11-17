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

        Pengo pengo;
        Enemy enemy;
        Camera camera;

        public HandleGame(Game game)
        {
            this.game = game;
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
            CreateObjectFactory();
            camera = new Camera();
        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();
            pengo.Update();
            enemy.Update();
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
            foreach (Trap spike in trap)
            {
                if (spike.PixelCollition(pengo))
                {
                    pengo.KillPengo(pengoRespawnPos);
                }
            }
            foreach (Ladder ladderTile in ladder)
            {
                if (ladderTile.hitbox.Intersects(pengo.hitbox))
                {
                    pengo.isOnLadder = true;
                }
            }
            backgrounds.Update();
            if (enemy.PixelCollition(pengo))
            {
                pengo.KillPengo(pengoRespawnPos);
            }
            if (pengo.pos.Y >= Game1.TILE_SIZE * 14)
            {
                pengo.KillPengo(pengoRespawnPos);
            }
            camera.Update(pengo.pos);
            
        }



        private void CreateObjectFactory()
        {
            StreamReader sr = new StreamReader(@"MenuLevel.txt");
            int row = 0;
            while (!sr.EndOfStream)
            {
                string objectStr = sr.ReadLine();
                for (int col = 0; col < objectStr.Length; col++)
                {
                    ObjectFactory(objectStr[col], row, col);
                }
                row++;
            }
        }

        private void ObjectFactory(char objectChar, int row, int col)
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

            }
        }

        public Vector2 GetPengoPos()
        {
            return pengo.pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {   
            spriteBatch.Draw(background, new Rectangle(0, game.Window.ClientBounds.Height - background.Height - (1 * Game1.TILE_SIZE), background.Width, background.Height), Color.White);
            spriteBatch.Draw(caveBackground, new Vector2(Game1.TILE_SIZE * 37, Game1.TILE_SIZE * 9), Color.White);
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
        }
    }
}
  
