using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPengoMan.GameObject;
using System.Collections.Generic;
using System.IO;

namespace SuperPengoMan
{
    
    public class Game1 : Game
    {
        public const int TILE_SIZE = 40;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D penguin;
        Texture2D iceTile;
        Texture2D background;
        Texture2D waterTile;

        List<FloorTile> floortile = new List<FloorTile>();
        List<WaterTile> watertile = new List<WaterTile>();

        Pengo pengo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

       
        protected override void Initialize()
        {

            graphics.PreferredBackBufferWidth = TILE_SIZE * 15;
            graphics.PreferredBackBufferHeight = TILE_SIZE * 15;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            penguin = Content.Load<Texture2D>(@"penguin_spritesheet");
            iceTile = Content.Load<Texture2D>(@"ice_tile");
            background = Content.Load<Texture2D>(@"background");
            waterTile = Content.Load<Texture2D>(@"water_tile");
            CreateObjectFactory();
        }

        
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        private void CreateObjectFactory()
        {
            StreamReader sr = new StreamReader(@"Level1.txt");
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
            Vector2 pos = new Vector2(TILE_SIZE * col, TILE_SIZE * row);
            switch (objectChar)
            {
                case 'F':
                    floortile.Add(new FloorTile(iceTile, pos));
                    break;
                case 'S':
                    pengo = new Pengo(penguin, pos);
                    break;
                case 'W':
                    watertile.Add(new WaterTile(waterTile, pos));
                    break;

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, Window.ClientBounds.Height - background.Height - (2 * TILE_SIZE), background.Width, background.Height), Color.White);
            pengo.Draw(spriteBatch);
            foreach (FloorTile iceTile in floortile)
            {
                iceTile.Draw(spriteBatch);
            }
            foreach (WaterTile waterTile in watertile)
            {
                waterTile.Draw(spriteBatch);
            }    
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
