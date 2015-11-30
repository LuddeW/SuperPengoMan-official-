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

        public delegate void HandleOptionDelegate(char menuOption);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        HandleGame handlegame;
        LevelReader levelreader;
        LevelReader menuLevelreader;
        LevelEditor levelEditor;

        private enum GameState { Setup, StartMenu, HighScore, GameScreen, LevelEditor, EndGame }
        private GameState currentGameState = GameState.Setup;
        private GameState nextGameStateState = GameState.StartMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            handlegame = new HandleGame(this, new HandleOptionDelegate(HandleMenuOption));
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
            levelreader = new LevelReader("Level1.txt");
            menuLevelreader = new LevelReader("MenuLevel.txt");
            levelEditor = new LevelEditor(this, levelreader, new HandleOptionDelegate(HandleMenuOption));
            handlegame.CreateLevel(menuLevelreader[0], TILE_SIZE, new Point(0, 0));
        }
        
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentGameState)
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
                    nextGameStateState = GameState.StartMenu;
                    break;
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            switch (currentGameState)
            {
                case GameState.StartMenu:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.ViewMatrix());
                    handlegame.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.HighScore:
                    break;
                case GameState.GameScreen:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.ViewMatrix());
                    handlegame.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.LevelEditor:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, levelEditor.ViewMatrix());
                    levelEditor.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.EndGame:
                    nextGameStateState = GameState.StartMenu;
                    break;
            }

            HandleNextState();
            base.Draw(gameTime);
        }

        private void HandleNextState()
        {
            if (nextGameStateState != currentGameState)
            {
                switch (nextGameStateState)
                {
                    case GameState.StartMenu:
                        StartHandleGame(menuLevelreader[0]);
                        break;
                    case GameState.HighScore:
                        break;
                    case GameState.GameScreen:
                        StartHandleGame(levelreader[0]);
                        break;
                    case GameState.LevelEditor:
                        levelEditor.Init();
                        break;
                    case GameState.EndGame:
                        break;
                }
            }
            currentGameState = nextGameStateState;
        }

        private void StartHandleGame(Level level)
        {
            handlegame = new HandleGame(this, new HandleOptionDelegate(HandleMenuOption));
            handlegame.LoadContent();
            handlegame.CreateLevel(level, TILE_SIZE, new Point(0, 0));
        }

        private void HandleMenuOption(char menuOption)
        {
            switch (menuOption)
            {
                case '0':
                    nextGameStateState = GameState.GameScreen;
                    break;
                case '1':
                    nextGameStateState = GameState.LevelEditor;
                    break;
                case '2':
                    nextGameStateState = GameState.StartMenu;
                    break;
            }
        }
    }
}
