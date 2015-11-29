using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace SuperPengoMan
{
    
    public class Game1 : Game
    {
        public delegate void AddPointsDelegate(int points);
        public delegate void HandleOptionDelegate(char menuOption);

        public const int TILE_SIZE = 40;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private HandleGame handlegame;
        private LevelEditor levelEditor;
        private enum GameState { Setup, StartMenu, HighScore, GameScreen, LevelEditor, EndGame }
        private GameState currentGameState = GameState.Setup;
        private GameState nextGameStateState = GameState.StartMenu;

        private LevelReader levelsLevelReader;
        private LevelReader menuLevelReader;
        private int currentLevel = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            levelsLevelReader = new LevelReader("Level1.txt");
            menuLevelReader = new LevelReader("MenuLevel.txt");
        }


        protected override void Initialize()
        {
            KeyList.Init();
            graphics.PreferredBackBufferWidth = TILE_SIZE * 15;
            graphics.PreferredBackBufferHeight = TILE_SIZE * 15;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            levelEditor = new LevelEditor(this, levelsLevelReader, new HandleOptionDelegate(HandleMenuOption));
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
                    levelEditor.Update();
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
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.ViewMatrix);
                    handlegame.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.HighScore:
                    Debug.WriteLine("HighScore");
                    break;

                case GameState.GameScreen:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, handlegame.ViewMatrix);
                    handlegame.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.LevelEditor:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, levelEditor.ViewMatrix);
                    levelEditor.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.EndGame:
                    Debug.WriteLine("EndGame");
                    break;
            }
            base.Draw(gameTime);

            HandleNextState();
        }

        private void HandleNextState()
        {
            if (nextGameStateState != currentGameState)
            {
                switch (nextGameStateState)
                {
                    case GameState.StartMenu:
                        StartHandleGame(menuLevelReader[0]);
                        break;
                    case GameState.HighScore:
                        break;
                    case GameState.GameScreen:
                        StartHandleGame(levelsLevelReader[currentLevel]);
                        break;
                    case GameState.LevelEditor:
                        levelEditor.InitEditor();
                        break;
                    case GameState.EndGame:
                        break;
                }
            }
            currentGameState = nextGameStateState;
        }

        private void StartHandleGame(Level level)
        {
            handlegame = new HandleGame(this, new AddPointsDelegate(AddPoints),
                            new HandleOptionDelegate(HandleMenuOption),
                            new HandleOptionDelegate(HandleGoalOption),
                            new HandleOptionDelegate(HandleRubyOption));
            handlegame.LoadContent();
            handlegame.CreateLevel(level);
        }

        private void AddPoints(int points)
        {
            
        }

        private void HandleMenuOption(char menuOption)
        {
            switch (menuOption)
            {
                case '0':
                    nextGameStateState = GameState.StartMenu;
                    break;
                case '1':
                    nextGameStateState = GameState.GameScreen;
                    break;
                case '2':
                    nextGameStateState = GameState.LevelEditor;
                    break;
            }
        }

        private void HandleGoalOption(char goalOption)
        {
        }

        private void HandleRubyOption(char rubyOption)
        {
        }
    }
}
