using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Kong_Engine.Objects;
using Kong_Engine.States;
using nkast.Aether.Physics2D.Dynamics;

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
            // Handle the collision with the environment
            Console.WriteLine("Player collided with the environment!");
            // Add your collision handling logic here, such as stopping the player's movement
        }
    }
}
