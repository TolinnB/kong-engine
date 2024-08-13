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
                if (entity is RaccoonSprite player)
                {
                    CheckPlayerCollisions(player);
                }
            }
        }

        private void CheckPlayerCollisions(RaccoonSprite player)
        {
            var playerBoundingBox = player.GetBoundingBox();
            Console.WriteLine($"Player BoundingBox: {playerBoundingBox}");

            foreach (var rectangle in _tileMapManager.CollisionRectangles)
            {
                if (playerBoundingBox.Intersects(rectangle))
                {
                    Console.WriteLine($"Collision Detected with Rectangle: X={rectangle.X}, Y={rectangle.Y}");
                    HandleCollisionWithEnvironment(player);
                    break;
                }
            }
        }

        private void HandleCollisionWithEnvironment(RaccoonSprite player)
        {
            Console.WriteLine("Player collided with the environment!");
            player.Move(new Vector2(0, -1)); // Example: stop the player's movement
        }
    }
}
