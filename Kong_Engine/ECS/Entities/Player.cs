using Kong_Engine.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kong_Engine.ECS.Entities
{
    internal class Player
    {
        public class PlayerEntity
        {
            public PlayerEntity()
            {
                EventManager.Subscribe(Events.PLAYER_JUMP, OnPlayerJump);
                EventManager.Subscribe(Events.PLAYER_DAMAGE, OnPlayerHit);
            }

            private void OnPlayerJump(object arg)
            {
                Console.WriteLine("Player jumped!");
                
            }

            private void OnPlayerHit(object arg)
            {
                Console.WriteLine("Player hit!");
                
            }
        }
    }
}
