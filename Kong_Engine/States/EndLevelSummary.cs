using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.Enum;

namespace Kong_Engine.States
{
    public class EndLevelSummary : BaseGameState
    {
        private bool _enterPressed;
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("endLevelSummary")));
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right))
            {
                if (!_enterPressed)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }
            }
        }

        protected override void SetInputManager()
        {
            // No input manager needed for splash screen
        }
    }
}
