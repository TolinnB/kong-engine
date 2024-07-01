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

        // Declare the _playerSprite field
        private PlayerSprite _playerSprite;

        public override void LoadContent()
        {
            // Load and add the background image
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));

            // Load and add the player sprite, and assign it to _playerSprite
            _playerSprite = new PlayerSprite(LoadTexture(Player));
            AddGameObject(_playerSprite);
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public override void HandleInput()
        {
            if (InputManager == null)
            {
                throw new InvalidOperationException("InputManager is not initialized.");
            }

            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                }
                else if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                }
            });
        }
    }
}
