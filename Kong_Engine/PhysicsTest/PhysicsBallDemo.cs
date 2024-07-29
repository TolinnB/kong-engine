
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Kong_Engine.States.Base;
using nkast.Aether.Physics2D.Dynamics;


namespace Kong_Engine.PhysicsTest
{
    public class PhysicsBallDemo : Game
    {

        private BaseGameState _currentGameState;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;

        private const int DESIGNED_RESOLUTION_WIDTH = 1280;
        private const int DESIGNED_RESOLUTION_HEIGHT = 720;

        private const float DESIGNED_RESOLUTION_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;

        private List<BallSprite> balls;
        private World ballLand; //Represents phsyical world for objects

        public PhysicsBallDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = DESIGNED_RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = DESIGNED_RESOLUTION_HEIGHT;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // 
            ballLand = new World(); //maintains world, applies effects
            ballLand.Gravity = Vector2.Zero;

            //Syntax for initialising edge collision so things don't float off the edge
            var top = 0;
            var bottom = DESIGNED_RESOLUTION_HEIGHT;
            var left = 0;
            var right  = DESIGNED_RESOLUTION_WIDTH;
            var edges = new Body[] {

            ballLand.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
            ballLand.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
            ballLand.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom)),
            ballLand.CreateEdge(new Vector2(right, top), new Vector2(right, bottom))
            };

            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1.0f);
            }
            System.Random random = new System.Random();
            balls = new List<BallSprite>();
            for (int i = 0; i < 20; i++)
            {
                var radius = random.Next(5, 50);
                var position = new Vector2(
                    random.Next(radius, DESIGNED_RESOLUTION_WIDTH - radius),
                    random.Next(radius, DESIGNED_RESOLUTION_HEIGHT - radius)
                    );
                var body = ballLand.CreateCircle(radius, 1, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-50, 50),
                    random.Next(-50, 50));
                body.SetRestitution(1); // Restitution denotes objects ricocheting off of one another
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                balls.Add(new BallSprite(radius, body));
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var ball in balls) ball.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var ball in balls) ball.Update(gameTime);
            ballLand.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var ball in balls) ball.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}