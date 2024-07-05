using Kong_Engine.Objects;
using Kong_Engine.States.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.States
{
    public class MainMenuState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("mainMenu")));
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new GameplayState());
            }

            PreviousKeyboardState = currentKeyboardState;
        }

        protected override void SetInputManager()
        {
            // Implement later
        }
    }

}
