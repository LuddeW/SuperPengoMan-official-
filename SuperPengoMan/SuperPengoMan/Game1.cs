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

        HandleGame handlegame;

        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            handlegame = new HandleGame(this);
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
            handlegame.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            camera = new Camera();
        }

        
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            handlegame.Update();
            camera.Update(handlegame.GetPengoPos());
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix);
            handlegame.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
