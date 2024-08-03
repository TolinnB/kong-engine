using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.States
{
    public class EndLevelSummaryState : BaseGameState
    {
        private Texture2D _endLevelSummaryBackground;

        public override void LoadContent()
        {
            _endLevelSummaryBackground = Content.Load<Texture2D>("endLevelSummary");
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                NotifyEvent(Events.GAME_QUIT);
            }

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_endLevelSummaryBackground, new Vector2(0, 0), Color.White);
        }
    }
}
