using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.Objects;
using System.Collections.Generic;

namespace Kong_Engine.States
{
    public class MainMenuState : BaseGameState
    {
        private MainMenuImage _mainMenuBackground;
        private List<Rectangle> _buttons;
        private List<BaseGameState> _levels;
        private MouseState _previousMouseState;

        public override void LoadContent()
        {
            // Load the background image
            var backgroundTexture = Content.Load<Texture2D>("mainMenu2");
            _mainMenuBackground = new MainMenuImage(backgroundTexture);
            AddGameObject(_mainMenuBackground);

            // Define buttons and their corresponding levels
            _buttons = new List<Rectangle>
            {
                new Rectangle(100, 300, 200, 50),  // Level 1 Button
                new Rectangle(100, 400, 200, 50),  // Level 2 Button
                new Rectangle(100, 500, 200, 50)   // Level 3 Button
            };

            _levels = new List<BaseGameState>
            {
                new Level1State(),  // Corresponds to Level 1 Button
                new Level2State(),  // Corresponds to Level 2 Button
                new Level3State()   // Corresponds to Level 3 Button
            };
        }

        public override void HandleInput()
        {
            var currentMouseState = Mouse.GetState();

            // Detect mouse clicks
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < _buttons.Count; i++)
                {
                    if (_buttons[i].Contains(currentMouseState.Position))
                    {
                        // Switch to the corresponding level
                        SwitchState(_levels[i]);
                        break;
                    }
                }
            }

            _previousMouseState = currentMouseState;
        }

        protected override void SetInputManager()
        {
            // No input manager needed for this simple menu
        }

        public override void Render(SpriteBatch spriteBatch)
        {

            // Render the background (base.Render might already be doing this)
            base.Render(spriteBatch);  // Assuming base.Render doesn't call Begin/End

            // Render buttons as text (you could use images or more sophisticated UI elements here)
            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 1", new Vector2(100, 300), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 2", new Vector2(100, 400), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 3", new Vector2(100, 500), Color.White);
        }

    }
}
