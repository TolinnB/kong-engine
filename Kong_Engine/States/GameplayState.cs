using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Kong_Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.System;
using System.Collections.Generic;
using Kong_Engine.ECS.Entity;

namespace Kong_Engine.States
{
    public class GameplayState : BaseGameState
    {
        private const string Player = "donkeyKong";
        private const string BackgroundTexture = "DKJunglejpg";

        private List<BaseEntity> _entities;
        private MovementSystem _movementSystem;
        private BaseEntity _playerEntity;

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));

            _playerEntity = new PlayerSprite(LoadTexture(Player));
            _entities = new List<BaseEntity> { _playerEntity };

            // Load enemy texture
            var enemyTexture = LoadTexture("kingKRool"); // Replace with your actual enemy texture path

            // Create and add enemy entity
            var enemyEntity = new EnemySprite(enemyTexture);
            _entities.Add(enemyEntity);

            _movementSystem = new MovementSystem();

            // Wrap the entities with the adapter
            AddGameObject(new EntityGameObjectAdapter(_playerEntity));
            AddGameObject(new EntityGameObjectAdapter(enemyEntity));
        }

        public override void Update(GameTime gameTime)
        {
            // Update the movement system with all entities
            _movementSystem.Update(_entities);

            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (var entity in _entities)
            {
                if (entity.HasComponent<TextureComponent>())
                {
                    var textureComponent = entity.GetComponent<TextureComponent>();
                    var positionComponent = entity.GetComponent<PositionComponent>();

                    spriteBatch.Draw(textureComponent.Texture, positionComponent.Position, Color.White);
                }
            }

            base.Render(spriteBatch);
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

            PreviousKeyboardState = currentKeyboardState;

            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    //NotifyEvent(Events.GAME_QUIT);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(-5, 0);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(5, 0);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveDown)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(0, 5);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveUp)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(0, -5);
                }
            });
        }
    }
}
