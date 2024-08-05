using Kong_Engine.ECS.Entity;
using Kong_Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

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
                    CheckPlayerCollisions(player);
                }
            }
        }

        private void CheckPlayerCollisions(PlayerSprite player)
        {
            var playerBoundingBox = player.GetBoundingBox();

            foreach (var rectangle in _tileMapManager.CollisionRectangles)
            {
                if (playerBoundingBox.Intersects(rectangle))
                {
                    HandleCollisionWithEnvironment(player);
                    break;
                }
            }
        }

        private void HandleCollisionWithEnvironment(PlayerSprite player)
        {
            // Handle the collision with the environment
            Console.WriteLine("Player collided with the environment!");
            // Add your collision handling logic here, such as stopping the player's movement
            player.Move(new Vector2(0, -1)); // Example: stop the player's movement
        }
    }
}
