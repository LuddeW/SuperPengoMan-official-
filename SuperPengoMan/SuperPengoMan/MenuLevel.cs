using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using SuperPengoMan.GameObject;

namespace SuperPengoMan
{
    class MenuLevel
    {
        //Game game;
        //public Background backgrounds;

        //Texture2D penguin;
        //Texture2D penguin_jump;
        //Texture2D penguin_glide;
        //Texture2D iceTile;
        //Texture2D background;
        //Texture2D caveBackground;
        //Texture2D waterTile;
        //Texture2D spike;
        //Texture2D snowball;
        //Texture2D ladderTile;
        //Texture2D penguin_climb;

        //List<FloorTile> Floortile = new List<FloorTile>();
        //List<WaterTile> Watertile = new List<WaterTile>();
        //List<Trap> trap = new List<Trap>();
        //List<Ladder> ladder = new List<Ladder>();

        //public Pengo pengo;
        //public Enemy enemy;
        

        //Vector2 pengoRespawnPos;

        //public MenuLevel(Game game)
        //{
        //    this.game = game;
        //}

        //public void Load()
        //{

            //penguin = game.Content.Load<Texture2D>(@"penguin_spritesheet");
            //penguin_jump = game.Content.Load<Texture2D>(@"penguin_jump");
            //penguin_glide = game.Content.Load<Texture2D>(@"penguin_glide");
            //penguin_climb = game.Content.Load<Texture2D>(@"penguin_climb");
            //iceTile = game.Content.Load<Texture2D>(@"ice_tile");
            //background = game.Content.Load<Texture2D>(@"background");
            //caveBackground = game.Content.Load<Texture2D>(@"snowcave");
            //waterTile = game.Content.Load<Texture2D>(@"water_tile");
            //spike = game.Content.Load<Texture2D>(@"spike");
            //snowball = game.Content.Load<Texture2D>(@"snowball");
            //ladderTile = game.Content.Load<Texture2D>(@"Ladder");
            //backgrounds = new Background(game.Content, game.Window);
            //CreateLevel();
        //}

        //private void CreateLevel()
        //{
        //    StreamReader sr = new StreamReader(@"MenuLevel.txt");
        //    int row = 0;
        //    while (!sr.EndOfStream)
        //    {
        //        string objectStr = sr.ReadLine();
        //        for (int col = 0; col < objectStr.Length; col++)
        //        {
        //            ObjectFactory(objectStr[col], row, col);
        //        }
        //        row++;
        //    }
        //}

        //private void ObjectFactory(char objectChar, int row, int col)
        //{
        //    Vector2 pos = new Vector2(Game1.TILE_SIZE * col, Game1.TILE_SIZE * row);
        //    switch (objectChar)
        //    {
        //        case 'F':
        //            Floortile.Add(new FloorTile(iceTile, pos));
        //            break;
        //        case 'S':
        //            pengoRespawnPos = pos;
        //            pengo = new Pengo(penguin, penguin_glide, penguin_jump, penguin_climb, pos);
        //            break;
        //        case 'W':
        //            Watertile.Add(new WaterTile(waterTile, pos));
        //            break;
        //        case 'T':
        //            trap.Add(new Trap(spike, pos));
        //            break;
        //        case 'E':
        //            enemy = new Enemy(snowball, pos);
        //            break;
        //        case 'L':
        //            ladder.Add(new Ladder(ladderTile, pos));
        //            break;

        //    }
        //}

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(background, new Rectangle(0, game.Window.ClientBounds.Height - background.Height - (1 * Game1.TILE_SIZE), background.Width, background.Height), Color.White);
        //    backgrounds.Draw(spriteBatch);
        //    foreach (Ladder ladderTile in ladder)
        //    {
        //        ladderTile.Draw(spriteBatch);
        //    }
        //    enemy.Draw(spriteBatch);
        //    pengo.Draw(spriteBatch);
        //    foreach (FloorTile iceTile in Floortile)
        //    {
        //        iceTile.Draw(spriteBatch);
        //    }
        //    foreach (WaterTile waterTile in Watertile)
        //    {
        //        waterTile.Draw(spriteBatch);
        //    }
        //    foreach (Trap spike in trap)
        //    {
        //        spike.Draw(spriteBatch);
        //    }
        //}
    }
}
