using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Kong_Engine.ECS.System
{
    public class ParticleSystem
    {
        private List<ParticleComponent> particles;
        private Texture2D texture;
        private Random random;

        public ParticleSystem(Texture2D texture)
        {
            this.texture = texture;
            particles = new List<ParticleComponent>();
            random = new Random();
        }

        public void AddParticle(Vector2 position)
        {
            var velocity = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
            var color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            var size = (float)random.NextDouble() * 0.5f;
            var lifeTime = (float)random.NextDouble() * 2;

            particles.Add(new ParticleComponent(position, velocity, color, size, lifeTime));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);
                if (particles[i].LifeTime <= 0)
                {
                    particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in particles)
            {
                particle.Draw(spriteBatch, texture);
            }
        }
    }
}
