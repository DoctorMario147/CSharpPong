using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Texture2D paddleTexture;
        Vector2 ballPosition;
        Vector2 onePaddlePosition;
        Vector2 twoPaddlePosition;
        Vector2 ballSpeed = new(150, 150);
        private readonly int paddleSpeed = 5;
        int plyOneScore = 0;
        int plyTwoScore = 0;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            ballPosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - ballTexture.Width,
                _graphics.GraphicsDevice.Viewport.Height / 2 - ballTexture.Height);
            onePaddlePosition = new Vector2(15,
                _graphics.GraphicsDevice.Viewport.Height / 2 - paddleTexture.Height / 2);
            twoPaddlePosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width - paddleTexture.Width - 15,
                _graphics.GraphicsDevice.Viewport.Height / 2 - paddleTexture.Height / 2);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            paddleTexture = Content.Load<Texture2D>("paddle");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int paddleMaxY = GraphicsDevice.Viewport.Height - paddleTexture.Height;

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W) && onePaddlePosition.Y > 0)
                onePaddlePosition.Y -= paddleSpeed;
            if (keyState.IsKeyDown(Keys.S) && onePaddlePosition.Y < paddleMaxY)
                onePaddlePosition.Y += paddleSpeed;

            if (keyState.IsKeyDown(Keys.Up) && twoPaddlePosition.Y > 0)
                twoPaddlePosition.Y -= paddleSpeed;
            if (keyState.IsKeyDown(Keys.Down) && twoPaddlePosition.Y < paddleMaxY)
                twoPaddlePosition.Y += paddleSpeed;

            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            int ballMaxX = GraphicsDevice.Viewport.Width - ballTexture.Width;
            int ballMaxY = GraphicsDevice.Viewport.Height - ballTexture.Height;

            if (ballPosition.X < 0)
            {
                plyTwoScore++;
                ballPosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - ballTexture.Width,
                _graphics.GraphicsDevice.Viewport.Height / 2 - ballTexture.Height);
            }
            else if (ballPosition.X > ballMaxX)
            {
                plyOneScore++;
                ballPosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - ballTexture.Width,
                _graphics.GraphicsDevice.Viewport.Height / 2 - ballTexture.Height);
            }

            if (ballPosition.Y > ballMaxY || ballPosition.Y < 0)
                ballSpeed.Y *= -1;

            Rectangle onePaddleRect = new((int)onePaddlePosition.X + paddleTexture.Width - 1, (int)onePaddlePosition.Y, 1, paddleTexture.Height);
            Rectangle twoPaddleRect = new((int)twoPaddlePosition.X, (int)twoPaddlePosition.Y, 1, paddleTexture.Height);
            Rectangle ballRect = new((int)ballPosition.X, (int)ballPosition.Y, ballTexture.Width, ballTexture.Height);

            if (ballRect.Intersects(onePaddleRect) && ballSpeed.X < 0)
            {
                if (ballSpeed.Y < 0)
                    ballSpeed.Y -= 50;
                else
                    ballSpeed.Y += 50;

                ballSpeed.X *= -1;
            }

            if (ballRect.Intersects(twoPaddleRect) && ballSpeed.X > 0)
            {
                if (ballSpeed.Y < 0)
                    ballSpeed.Y -= 50;
                else
                    ballSpeed.Y += 50;

                ballSpeed.X *= -1;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            _spriteBatch.Draw(paddleTexture, onePaddlePosition, Color.White);
            _spriteBatch.Draw(paddleTexture, twoPaddlePosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}