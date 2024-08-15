using Kong_Engine.ECS.Entity;
using Kong_Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Kong_Engine.ECS.Component;

/**/
// Unique collision for top-down games. This takes the collision layer from a TMX file
// and creates collision
/**/

namespace Kong_Engine.ECS.System
{
    public class TopDownCollisionSystem
    {
        private readonly AudioManager _audioManager;
        private readonly MainGame _game;
        private readonly TileMapManager _tileMapManager;

        public TopDownCollisionSystem(AudioManager audioManager, MainGame game, TileMapManager tileMapManager)
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

            foreach (var rectangle in _tileMapManager.CollisionRectangles)
            {
                if (playerBoundingBox.Intersects(rectangle))
                {
                    HandleCollisionWithEnvironment(player, rectangle);
                    break;  // Stop after the first collision to avoid multiple adjustments
                }
            }
        }

        private void HandleCollisionWithEnvironment(RaccoonSprite player, Rectangle collisionRectangle)
        {
            var playerBoundingBox = player.GetBoundingBox();
            var playerPosition = player.GetComponent<PositionComponent>().Position;

            Vector2 depth = GetIntersectionDepth(playerBoundingBox, collisionRectangle);

            if (depth != Vector2.Zero)
            {
                float absDepthX = Math.Abs(depth.X);
                float absDepthY = Math.Abs(depth.Y);

                // Adjust player's position based on collision depth
                if (absDepthX < absDepthY)
                {
                    // Adjust horizontal position
                    playerPosition.X += depth.X;
                    player.StopHorizontalMovement();  // Stop horizontal movement after adjusting
                }
                else
                {
                    // Adjust vertical position
                    playerPosition.Y += depth.Y;
                    player.StopVerticalMovement();  // Stop vertical movement after adjusting
                }

                // Update player's position component
                player.GetComponent<PositionComponent>().Position = playerPosition;
            }
        }

        private Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}
