using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Kong_Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kong_Engine.States
{
    public class GameplayState : BaseGameState
    {
        private const string Player = "donkeyKong";
        private const string BackgroundTexture = "DKJunglejpg";
        private PlayerSprite _playerSprite;

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
            _playerSprite = new PlayerSprite(LoadTexture(Player));
            AddGameObject(_playerSprite);
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            if (InputManager == null)
            {
                throw new InvalidOperationException("InputManager is not initialized.");
            }

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
            {
                SwitchState(new EndLevelSummary());
            }

            PreviousKeyboardState = currentKeyboardState; // Update the previous state

            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    //NotifyEvent(Events.GAME_QUIT);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                }
                else if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                }
                else if (cmd is GameplayInputCommand.PlayerMoveDown)
                {
                    _playerSprite.MoveDown();
                }
                else if (cmd is GameplayInputCommand.PlayerMoveUp)
                {
                    _playerSprite.MoveUp();
                }
            });
        }
    }
}
