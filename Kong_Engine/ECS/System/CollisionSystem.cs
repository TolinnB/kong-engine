using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Kong_Engine.Objects;
<<<<<<< HEAD
using Kong_Engine.States;
using nkast.Aether.Physics2D.Dynamics;
=======
>>>>>>> main

namespace Kong_Engine.ECS.System
{
    public class CollisionSystem
    {
        private readonly AudioManager _audioManager;

        public CollisionSystem(AudioManager audioManager)
        {
            _audioManager = audioManager;
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
            var contactEdge = player.PlayerBody.ContactList;
            while (contactEdge != null)
            {
                if (contactEdge.Contact.IsTouching)
                {
                    var otherBody = contactEdge.Other;
                    if (otherBody.Tag != null && otherBody.Tag is string tag)
                    {
                        if (tag == "collisionObject")
                        {
                            HandleCollisionWithEnvironment(player);
                        }
                    }
                }
                contactEdge = contactEdge.Next;
            }
        }

        private void HandleCollisionWithEnvironment(PlayerSprite player)
        {
<<<<<<< HEAD
            // Handle the collision with the environment
            Console.WriteLine("Player collided with the environment!");
            // Add your collision handling logic here, such as stopping the player's movement
=======
            if (entityA is PlayerSprite player && entityB is EnemySprite)
            {
                var playerLife = player.GetComponent<LifeComponent>();
                playerLife.Lives--;

                if (playerLife.Lives <= 0)
                {
                    Console.WriteLine("Player is dead!");
                    // Handle game over logic
                }
                else
                {
                    Console.WriteLine("Player hit! Lives remaining: " + playerLife.Lives);
                    _audioManager.PlaySound("hurtSound");

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
>>>>>>> main
        }
    }
}
