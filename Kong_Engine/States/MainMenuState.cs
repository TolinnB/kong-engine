using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.Objects;
using System.Collections.Generic;

/**/
// Level Selection screen for your game
/**/

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
            var backgroundTexture = Content.Load<Texture2D>("mainMenu2");
            _mainMenuBackground = new MainMenuImage(backgroundTexture);
            AddGameObject(_mainMenuBackground);

            _buttons = new List<Rectangle>
            {
                new Rectangle(100, 300, 200, 50),  // Level 1 Button
                new Rectangle(100, 400, 200, 50),  // Level 2 Button
                new Rectangle(100, 500, 200, 50)   // Level 3 Button
            };

            _levels = new List<BaseGameState>
            {
                new Level1State(),
                new Level2State(),
                new Level3State()
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
            // No input manager needed for this
        }

        public override void Render(SpriteBatch spriteBatch)
        {

            base.Render(spriteBatch);

            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 1", new Vector2(378, 600), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 2", new Vector2(500, 300), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "Level 3", new Vector2(900, 580), Color.White);
        }

    }
}