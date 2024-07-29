using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Kong_Engine.Objects;
using Kong_Engine.States;

namespace Kong_Engine.ECS.System
{
    public class CollisionSystem
    {
        private readonly AudioManager _audioManager;
        private readonly MainGame _game;

        public CollisionSystem(AudioManager audioManager, MainGame game)
        {
            _audioManager = audioManager;
            _game = game;
        }

        public void Update(IEnumerable<BaseEntity> entities)
        {
            var entitiesList = entities.ToList();

            for (int i = 0; i < entitiesList.Count; i++)
            {
                for (int j = i + 1; j < entitiesList.Count; j++)
                {
                    var entityA = entitiesList[i];
                    var entityB = entitiesList[j];

                    if (entityA.HasComponent<CollisionComponent>() && entityB.HasComponent<CollisionComponent>())
                    {
                        var collisionA = entityA.GetComponent<CollisionComponent>();
                        var collisionB = entityB.GetComponent<CollisionComponent>();
                        var positionA = entityA.GetComponent<PositionComponent>();
                        var positionB = entityB.GetComponent<PositionComponent>();

                        collisionA.BoundingBox = new Rectangle((int)positionA.Position.X, (int)positionA.Position.Y, collisionA.BoundingBox.Width, collisionA.BoundingBox.Height);
                        collisionB.BoundingBox = new Rectangle((int)positionB.Position.X, (int)positionB.Position.Y, collisionB.BoundingBox.Width, collisionB.BoundingBox.Height);

                        if (collisionA.BoundingBox.Intersects(collisionB.BoundingBox))
                        {
                            HandleCollision(entityA, entityB);
                        }
                    }
                }
            }
        }

        private void HandleCollision(BaseEntity entityA, BaseEntity entityB)
        {
            if (entityA is PlayerSprite player && entityB is EnemySprite)
            {
                var playerLife = player.GetComponent<LifeComponent>();
                playerLife.Lives--;

                if (playerLife.Lives <= 0)
                {
                    Console.WriteLine("Player is dead!");
                    _game.SwitchState(new GameOverState()); // Switch to GameOverState
                }
                else
                {
                    Console.WriteLine("Player hit! Lives remaining: " + playerLife.Lives);
                    _audioManager.PlaySound("donkeyKongHurt");

                    // Apply knockback
                    var playerPosition = player.GetComponent<PositionComponent>();
                    var enemyPosition = entityB.GetComponent<PositionComponent>();

                    var knockbackDirection = Vector2.Normalize(playerPosition.Position - enemyPosition.Position);
                    player.Knockback = knockbackDirection * 50f; // Adjust the knockback strength

                    playerPosition.Position += player.Knockback;
                }
            }
            else if (entityA is EnemySprite && entityB is PlayerSprite)
            {
                HandleCollision(entityB, entityA); // Ensure both cases are handled
            }
        }
    }
}
