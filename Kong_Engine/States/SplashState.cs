using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.States
{
    public class SplashState : BaseGameState
    {
        private Camera _camera;

        public SplashState(Camera camera)
        {
            _camera = camera;
        }

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splashScreen3")));
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new MainMenuState(_camera));
            }

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
        }
    }
}
