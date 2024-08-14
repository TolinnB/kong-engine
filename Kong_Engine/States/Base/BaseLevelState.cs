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
using System.Diagnostics;

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

            int screenWidth = game.GraphicsDevice.Viewport.Width;
            MovementSystem = new MovementSystem(screenWidth);
        }

        public override void LoadContent()
        {
            Entities = new List<BaseEntity>();

            LoadLevelContent();
            InitializeEntities();

            CollisionSystem = new CollisionSystem(AudioManager, Game, TileMapManager);
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
            if (PlayerEntity != null)
            {
                if (command is GameplayInputCommand.PlayerMoveLeft) PlayerEntity.Move(new Vector2(-5, 0));
                if (command is GameplayInputCommand.PlayerMoveRight) PlayerEntity.Move(new Vector2(5, 0));
                if (command is GameplayInputCommand.PlayerMoveDown) PlayerEntity.Move(new Vector2(0, 5));
                if (command is GameplayInputCommand.PlayerMoveUp) PlayerEntity.Move(new Vector2(0, -5));
            }
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

        protected abstract bool IsLevelCompleted();

        public override void Render(SpriteBatch spriteBatch)
        {
            // Use a scale of 1.0 if you don't want to scale the render output
            Matrix scaleMatrix = Matrix.CreateScale(1.0f);
            TileMapManager?.Draw(scaleMatrix);
            PlayerEntity?.Draw(spriteBatch, scaleMatrix);
            EnemyEntity?.Draw(spriteBatch, scaleMatrix);
        }

    }
}
