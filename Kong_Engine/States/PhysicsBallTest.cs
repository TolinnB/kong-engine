using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Kong_Engine.States.Base;
using nkast.Aether.Physics2D.Dynamics;
using Kong_Engine.PhysicsTest;
using System;

namespace Kong_Engine.States
{
    public class PhysicsBallTest : BaseGameState
    {
        private List<BallSprite> balls;
        private World ballLand;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private const int DESIGNED_RESOLUTION_WIDTH = 1280;
        private const int DESIGNED_RESOLUTION_HEIGHT = 720;

        public override void Initialize()
        {
            ballLand = new World();
            ballLand.Gravity = Vector2.Zero;

            var top = 0;
            var bottom = DESIGNED_RESOLUTION_HEIGHT;
            var left = 0;
            var right = DESIGNED_RESOLUTION_WIDTH;
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
                body.SetRestitution(1);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                balls.Add(new BallSprite(radius, body));
            }
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var ball in balls) ball.LoadContent(Content);
        }

        public override void HandleInput()
        {
            // Add input handling here if needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var ball in balls) ball.Update(gameTime);
            ballLand.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (var ball in balls) ball.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            // Unload content here if needed
        }
    }
}
