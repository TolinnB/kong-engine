using Kong_Engine.ECS.Entity;
using Kong_Engine.Objects;
using System.Collections.Generic;
using System;
using Kong_Engine.ECS.Component;

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
            var collisionComponent = player.GetComponent<CollisionComponent>();
            if (collisionComponent != null)
            {
                foreach (var contactEdge in collisionComponent.ContactList)
                {
                    if (contactEdge.Contact.IsTouching)
                    {
                        var otherBody = contactEdge.Other;
                        if (otherBody.UserData != null && otherBody.UserData is string userData)
                        {
                            if (userData == "collisionObject")
                            {
                                HandleCollisionWithEnvironment(player);
                            }
                        }
                    }
                }
            }
        }

        private void HandleCollisionWithEnvironment(PlayerSprite player)
        {
            // Handle the collision with the environment
            Console.WriteLine("Player collided with the environment!");
            // Add your collision handling logic here, such as stopping the player's movement
        }
    }
}
