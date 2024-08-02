using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.System;
using Kong_Engine.Input;
using Kong_Engine.Input.Base;
using Kong_Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Kong_Engine.States.Base
{
    public abstract class BaseLevelState : BaseGameState
    {
        protected SpriteBatch SpriteBatch;
        protected TileMapManager TileMapManager;
        protected List<BaseEntity> Entities;
        protected PlayerSprite PlayerEntity;
        protected EnemySprite EnemyEntity;
        protected MovementSystem MovementSystem;
        protected CollisionSystem CollisionSystem;
        protected AudioManager AudioManager;
        protected InputManager InputManager;

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            AudioManager = new AudioManager(contentManager);
            SetInputManager();
        }

        public override void LoadContent()
        {
            Entities = new List<BaseEntity>();
            MovementSystem = new MovementSystem();
            CollisionSystem = new CollisionSystem(AudioManager);

            LoadLevelContent();
            InitializeEntities();
        }

        protected abstract void LoadLevelContent();
        protected abstract void InitializeEntities();
        protected abstract override void SetInputManager();

        public override void HandleInput()
        {
            InputManager.GetCommands(HandleCommand);
            PreviousKeyboardState = Keyboard.GetState();
        }

        protected virtual void HandleCommand(BaseInputCommand command)
        {
            if (command is GameplayInputCommand.PlayerMoveLeft) PlayerEntity.Move(new Vector2(-5, 0));
            if (command is GameplayInputCommand.PlayerMoveRight) PlayerEntity.Move(new Vector2(5, 0));
            if (command is GameplayInputCommand.PlayerMoveDown) PlayerEntity.Move(new Vector2(0, 5));
            if (command is GameplayInputCommand.PlayerMoveUp) PlayerEntity.Move(new Vector2(0, -5));
            if (command is GameplayInputCommand.GameExit) Game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            MovementSystem.Update(Entities);
            CollisionSystem.Update(Entities);
            PlayerEntity?.Update(gameTime);
            EnemyEntity?.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var transformMatrix = Matrix.CreateScale(1);
            TileMapManager?.Draw(transformMatrix);
            PlayerEntity?.Draw(spriteBatch, transformMatrix);
            EnemyEntity?.Draw(spriteBatch, transformMatrix);
            base.Render(spriteBatch);
        }
    }
}
