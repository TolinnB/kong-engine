using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.States.Base;
using Aether2D = nkast.Aether.Physics2D.Dynamics;
using System.Collections.Generic;
using Kong_Engine.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System;
using Kong_Engine.ECS.Entity;

namespace Kong_Engine.States
{
    public class PhysicsBallState : BaseGameState
    {
        private SpriteBatch spriteBatch;
        private List<BallSprite> balls;
        private Aether2D.World ballLand;
        private Aether2D.Body physicsBody;
        

        private const int DESIGNED_RESOLUTION_WIDTH = 1280;
        private const int DESIGNED_RESOLUTION_HEIGHT = 720;

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            ballLand = new Aether2D.World();
            ballLand.Gravity = Vector2.Zero;

            var top = 0;
            var bottom = DESIGNED_RESOLUTION_HEIGHT;
            var left = 0;
            var right = DESIGNED_RESOLUTION_WIDTH;
            var edges = new Aether2D.Body[]
            {
                ballLand.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                ballLand.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
                ballLand.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom)),
                ballLand.CreateEdge(new Vector2(right, top), new Vector2(right, bottom))
            };

            foreach (var edge in edges)
            {
                edge.BodyType = Aether2D.BodyType.Static;
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
                var body = ballLand.CreateCircle(radius, 1, position, Aether2D.BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-50, 50),
                    random.Next(-50, 50));
                body.SetRestitution(1);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                balls.Add(new BallSprite(radius, body));
            }

            foreach (var ball in balls)
            {
                ball.LoadContent(Game.Content);
            }
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new MainMenuState());
            }

            PreviousKeyboardState = currentKeyboardState;
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var ball in balls) ball.Update(gameTime);
            ballLand.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (var ball in balls) 
                ball.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
    }
}
