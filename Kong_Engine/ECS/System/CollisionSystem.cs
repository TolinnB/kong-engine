using Kong_Engine.ECS.Entity;
using Kong_Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.System
{
    public class CollisionSystem
    {
        private readonly AudioManager _audioManager;
        private readonly MainGame _game;
        private readonly TileMapManager _tileMapManager;

        public CollisionSystem(AudioManager audioManager, MainGame game, TileMapManager tileMapManager)
        {
            _audioManager = audioManager;
            _game = game;
            _tileMapManager = tileMapManager;
        }

        public void Update(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is PlayerSprite player)
                {
                    CheckPlayerCollisions(player, entities);
                }
            }
        }

        private void CheckPlayerCollisions(PlayerSprite player, IEnumerable<BaseEntity> entities)
        {
            var playerBoundingBox = player.GetBoundingBox();

            // Check for collisions with the environment
            foreach (var rectangle in _tileMapManager.CollisionRectangles)
            {
                if (playerBoundingBox.Intersects(rectangle))
                {
                    HandleCollisionWithEnvironment(player);
                    return; // Stop further collision checks if an environment collision is detected
                }
            }

            // Check for collisions with enemies
            foreach (var entity in entities)
            {
                if (entity is EnemySprite enemy)
                {
                    var enemyBoundingBox = enemy.GetComponent<CollisionComponent>().BoundingBox;

                    if (playerBoundingBox.Intersects(enemyBoundingBox))
                    {
                        HandleCollisionWithEnemy(player, enemy);
                        return; // Stop further collision checks if an enemy collision is detected
                    }
                }
            }
        }


        private void HandleCollisionWithEnvironment(PlayerSprite player)
        {
            Console.WriteLine("Player collided with the environment!");
            player.Move(new Vector2(0, -1)); // Example: stop the player's movement
        }

        private void HandleCollisionWithEnemy(PlayerSprite player, EnemySprite enemy)
        {
            Console.WriteLine("Player collided with an enemy!");

            // Apply a smaller knockback based on the direction of the enemy
            var knockbackDirection = player.GetComponent<PositionComponent>().Position.X < enemy.GetComponent<PositionComponent>().Position.X ? -1 : 1;
            player.Knockback = new Vector2(5f * knockbackDirection, -2f); // Much smaller knockback force

            // Play the hurt sound
            player.PlayHurtSound();
        }

    }

}
