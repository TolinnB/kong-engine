using Kong_Engine.Objects;
using Kong_Engine.States.Base;
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

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
        }
    }
}
