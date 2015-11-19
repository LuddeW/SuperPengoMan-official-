using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPengoMan.GameObject;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SuperPengoMan
{
    
    public class Game1 : Game
    {
        public const int TILE_SIZE = 40;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private HandleGame handlegame;
        private enum GameState { StartMenu, HighScore, GameScreen, LevelEditor, EndGame }
        private GameState CurrentState = GameState.StartMenu;

        private LevelReader levelsLevelReader;
        private LevelReader menuLevelReader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            handlegame = new HandleGame(this);
            levelsLevelReader = new LevelReader(@"Level1.txt");
            menuLevelReader = new LevelReader(@"MenuLevel.txt");
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
            handlegame.CreateLevel(menuLevelReader[0]);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case GameState.StartMenu:
                    handlegame.Update();
                    break;

                case GameState.HighScore:

                    break;

                case GameState.GameScreen:
                    handlegame.Update();
                    break;
                case GameState.LevelEditor:

                    break;
                case GameState.EndGame:
                    CurrentState = GameState.StartMenu;
                    break;
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            switch (CurrentState)
            {
                case GameState.StartMenu:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.PengoCamera.ViewMatrix);
                    handlegame.Draw(spriteBatch);
                    break;

                case GameState.HighScore:
                    Debug.WriteLine("LevelEditor");
                    break;

                case GameState.GameScreen:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.PengoCamera.ViewMatrix);
                    handlegame.Draw(spriteBatch);
                    break;
                case GameState.LevelEditor:
                    Debug.WriteLine("LevelEditor");
                    break;
                case GameState.EndGame:
                    CurrentState = GameState.StartMenu;
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
