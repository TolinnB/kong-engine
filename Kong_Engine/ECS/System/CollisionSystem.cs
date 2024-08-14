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
                    HandleCollisionWithEnvironment(player, rectangle); // Pass the rectangle here
                    break;
                }
            }
        }

        private void HandleCollisionWithEnvironment(RaccoonSprite player, Rectangle collisionRectangle)
        {
            // Get the player's bounding box and position
            var playerBoundingBox = player.GetBoundingBox();
            var playerPosition = player.GetComponent<PositionComponent>().Position;

            // Calculate the intersection depth between the player and the collision rectangle
            Vector2 depth = GetIntersectionDepth(playerBoundingBox, collisionRectangle);

            // If there is a collision (depth values are not zero)
            if (depth != Vector2.Zero)
            {
                // Calculate the absolute overlap distances
                float absDepthX = Math.Abs(depth.X);
                float absDepthY = Math.Abs(depth.Y);

                // Determine which direction to move the player based on the smallest overlap
                if (absDepthX < absDepthY)
                {
                    // Adjust horizontal position
                    playerPosition.X += depth.X;
                    player.StopHorizontalMovement();
                }
                else
                {
                    // Adjust vertical position
                    playerPosition.Y += depth.Y;
                    player.StopVerticalMovement();
                }

                // Update player's position component
                player.GetComponent<PositionComponent>().Position = playerPosition;
            }
        }


        private Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0)
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}
