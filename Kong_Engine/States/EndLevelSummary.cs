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
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                if (!_enterPressed)
                {
                    NotifyEvent(Events.GAME_QUIT);
                    _enterPressed = true;
                }
            }

            PreviousKeyboardState = currentKeyboardState; // Update the previous state
        }

        protected override void SetInputManager()
        {
            // No input manager needed for end level summary
        }
    }

}
