using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimplePlatformerGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Vector2 _playerPosition;
        private Vector2 _playerVelocity;
        private const float Gravity = 0.5f;
        private const float JumpPower = -12f;
        private bool _isJumping;

        private Texture2D _platformTexture;
        private Rectangle[] _platforms;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player texture
            _playerTexture = Content.Load<Texture2D>("player");

            // Set player starting position
            _playerPosition = new Vector2(100, 100);

            // Load platform texture
            _platformTexture = Content.Load<Texture2D>("platform");

            // Initialize platforms
            _platforms = new Rectangle[]
            {
                new Rectangle(0, 500, 800, 20),
                new Rectangle(100, 400, 100, 20),
                new Rectangle(300, 300, 150, 20),
                new Rectangle(600, 200, 100, 20)
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Handle player movement
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _playerVelocity.X = -3f;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _playerVelocity.X = 3f;
            }
            else
            {
                _playerVelocity.X = 0f;
            }

            // Handle player jumping
            if (keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
            {
                _playerVelocity.Y = JumpPower;
                _isJumping = true;
            }

            // Apply gravity to player
            _playerVelocity.Y += Gravity;

            // Update player position
            _playerPosition += _playerVelocity;

            // Check for collisions with platforms
            foreach (var platform in _platforms)
            {
                if (_playerPosition.X + _playerTexture.Width > platform.Left &&
                    _playerPosition.X < platform.Right &&
                    _playerPosition.Y + _playerTexture.Height > platform.Top &&
                    _playerPosition.Y + _playerTexture.Height < platform.Bottom &&
                    _playerVelocity.Y >= 0)
                {
                    // Player landed on platform
                    _playerPosition.Y = platform.Top - _playerTexture.Height;
                    _isJumping = false;
                    _playerVelocity.Y = 0;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw platforms
            foreach (var platform in _platforms)
            {
                _spriteBatch.Draw(_platformTexture, platform, Color.White);
            }

            // Draw player
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}