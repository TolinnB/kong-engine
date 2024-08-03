using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.States
{
    public class MainMenuState : BaseGameState
    {
        private Texture2D _mainMenuBackground;

        public override void LoadContent()
        {
            _mainMenuBackground = Content.Load<Texture2D>("mainMenu");
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new Level1State());
            }

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mainMenuBackground, new Vector2(0, 0), Color.White);
        }
    }
}
