﻿using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework.Input;

/**/
// Load in your Splash Screen here
/**/

namespace Kong_Engine.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splashScreen3")));

            AudioManager.LoadSound("splashStart", "splashStart");
            AudioManager.PlaySound("splashStart");
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new MainMenuState());
            }

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
        }
    }
}