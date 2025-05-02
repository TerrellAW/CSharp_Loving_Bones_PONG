using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// An example of why source control is necessary, in the future create a private github repo even if based on another IP

/// TODO:
/// 1. Refactor game loop code into a function - complete
/// 2. Add functions for start screen and end screen - complete
/// 3. Functions will be called in update with conditions instead of hard code - complete
/// 4. Move game loop, start screen and end screen into their own classes/files - critical failure
/// 5. Add a background image - complete, buffer is black instead of needing a texture
/// 6. Add option to turn off music
/// 7. Add option to change music volume
/// 8. Add option to play singleplayer against AI
/// 9. Refactor switch statement into function to avoid duplicate code, Stringify class
/// 10. Refactor collision detection into functions to avoid duplicate code, put in a Collision class
/// 11. Refactor Draw method so it draws according to game state: Start, Game or End - complete
/// 12. Controller support
/// 13. Figure out why "Press Enter to Start" isn't blinking
/// 14. Have ball initial speed be higher depending on amount of rounds
/// 15. Figure out why music is loud despite volume setting
/// 16. Make objects that contain Bone attributes and Skull attributes and pass those as params for WeGaming

namespace PONG
{
    internal class Pong : Game
    {
        // Skull
        private Texture2D _skullTexture;
        private Vector2 _skullPosition;
        private float _skullSpeed;

        // Bones
        private Texture2D _boneTexture;
        private Vector2 _bonePosition1;
        private Vector2 _bonePosition2;
        private float _boneSpeed1;
        private float _boneSpeed2;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Random
        private Random _random = new Random();
        private int _randomIntX;
        private int _randomIntY;

        // Points
        private int _p1_points = 0;
        private int _p2_points = 0;
        private string _p1_points_string = "O";
        private string _p2_points_string = "O";

        // Fonts
        private SpriteFont _font;
        private SpriteFont _font2;

        // Song
        internal Song song;

        // Game state
        GameState gamestate = GameState.Start;

        // Player
        private string player = "";

        internal Pong()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize entity positions
            _skullPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2
            );

            _bonePosition1 = new Vector2(
                10,
                _graphics.PreferredBackBufferHeight / 2
            );

            _bonePosition2 = new Vector2(
                _graphics.PreferredBackBufferWidth - 10,
                _graphics.PreferredBackBufferHeight / 2
            );

            _skullSpeed = 100f;
            _boneSpeed1 = 250f;
            _boneSpeed2 = 250f;

            // Initialize random int
            _randomIntX = _random.Next(0, 2);
            _randomIntY = _random.Next(0, 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load game assets
            _skullTexture = Content.Load<Texture2D>("skull");
            _boneTexture = Content.Load<Texture2D>("bone");

            // Load font
            _font = Content.Load<SpriteFont>("font");
            _font2 = Content.Load<SpriteFont>("font2");

            // Load song
            song = Content.Load<Song>("PongSong");

            // Play song
            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop(); // stop current audio playback if playing or paused.
            }
            MediaPlayer.Volume = 20;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get Keyboard state
            var kstate = Keyboard.GetState();

            // Toggle fullscreen with F11
            if (kstate.IsKeyDown(Keys.F11))
            {
                ToggleFullscreen();
            }

            // Game state
            if (gamestate == GameState.Start)
            {
                // Start screen
                GameStateManager.StartScreen(gamestate, kstate);
                if (kstate.IsKeyDown(Keys.Enter))
                {
                    gamestate = GameState.Game;
                }
            }
            else if (gamestate == GameState.Game)
            {
                // Start game loop
                GameStateManager.WeGaming(
                    ref gamestate,
                    gameTime,
                    kstate,
                    ref _boneSpeed1,
                    ref _bonePosition1,
                    _graphics,
                    _boneTexture,
                    ref _boneSpeed2,
                    ref _bonePosition2,
                    ref _skullSpeed,
                    ref _skullPosition,
                    _skullTexture,
                    ref _randomIntX,
                    ref _randomIntY,
                    _random,
                    ref _p1_points,
                    ref _p2_points,
                    ref _p1_points_string,
                    ref _p2_points_string
                    );
            }
            else
            {
                // End screen
                GameStateManager.EndScreen(_p1_points, player);
            }

            base.Update(gameTime);
        }

        // Functions for Draw states
        protected void StartScreenDraw()
        {
            // Draw Title
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font,
                "PONG",
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 65, _graphics.PreferredBackBufferHeight / 2 - 100),
                Color.White
            );
            _spriteBatch.End();

            // Draw Subtitle
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font2,
                "By CSharp Loving Bones",
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, _graphics.PreferredBackBufferHeight / 2),
                Color.White
            );
            _spriteBatch.End();

            // Draw Start Instructions
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font2,
                "Press Enter to Start",
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, _graphics.PreferredBackBufferHeight / 2 + 100),
                Color.White
            );
            _spriteBatch.End();
        }

        protected void GamingDraw()
        {
            // Draw Skull
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                _skullTexture,
                _skullPosition,
                null,
                Color.White,
                0f,
                new Vector2(_skullTexture.Width / 2, _skullTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            // Draw Bone1
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                _boneTexture,
                _bonePosition1,
                null,
                Color.White,
                0f,
                new Vector2(1 / _boneTexture.Width, _boneTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            // Draw Bone2
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                _boneTexture,
                _bonePosition2,
                null,
                Color.White,
                0f,
                new Vector2(_boneTexture.Width, _boneTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            // Draw P1 Points
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font,
                _p1_points_string,
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 200, 10),
                Color.White * 0.8f
            );
            _spriteBatch.End();

            // Draw P2 Points
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font,
                _p2_points_string,
                new Vector2((_graphics.PreferredBackBufferWidth / 2) + 200, 10),
                Color.White * 0.8f
            );
            _spriteBatch.End();
        }

        protected void EndScreenDraw()
        {
            // Draw Game Over
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font,
                "GAME OVER",
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 180, _graphics.PreferredBackBufferHeight / 2 - 100),
                Color.White
            );
            _spriteBatch.End();

            // Draw Winner
            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                _font2,
                player + " Wins!",
                new Vector2((_graphics.PreferredBackBufferWidth / 2) - 100, _graphics.PreferredBackBufferHeight / 2),
                Color.White
            );
            _spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Background colour
            GraphicsDevice.Clear(Color.Black);

            if (gamestate == GameState.Start)
            {
                StartScreenDraw();
            }
            else if (gamestate == GameState.Game)
            {
                GamingDraw();
            }
            else
            {
                EndScreenDraw();
            }

            base.Draw(gameTime);
        }

        private void ToggleFullscreen()
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;

            if (_graphics.IsFullScreen)
            {
                // When fullscreen, use desktop resolution
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                // Return to window size
                _graphics.PreferredBackBufferWidth = 800; // Set your desired width
                _graphics.PreferredBackBufferHeight = 600; // Set your desired height
            }

            _graphics.ApplyChanges();
        }
    }
}
