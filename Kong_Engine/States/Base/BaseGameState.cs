using System;
using System.Collections.Generic;
using System.Linq;
using Kong_Engine.Enum;
using Kong_Engine.Input;
using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/**/
// Foundation state for managing states within your game.
// Handles loading content, updating objects, rendering and transitioning to the next state
/**/

namespace Kong_Engine.States.Base
{
    public abstract class BaseGameState
    {
        //Variables and constructors

        private ContentManager _contentManager;
        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

        protected KeyboardState PreviousKeyboardState { get; set; }
        protected InputManager InputManager { get; set; }
        protected ContentManager Content => _contentManager;
        protected MainGame Game { get; private set; }

        //Sets up main game reference and input manager
        public virtual void Initialize(ContentManager contentManager, MainGame game)
        {
            _contentManager = contentManager;
            Game = game;
            SetInputManager();
            PreviousKeyboardState = Keyboard.GetState(); // Stops it from going through all menus in one input
        }

        public abstract void LoadContent();
        public void UnloadContent() => _contentManager.Unload();
        public abstract void HandleInput();
        protected abstract void SetInputManager();

        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<Events> OnEventNotification;

        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);
            return texture ?? _contentManager.Load<Texture2D>("fallbackTexture");
        }

        protected void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);
        }

        protected void SwitchState(BaseGameState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }

        public void AddGameObject(BaseGameObject gameObject) => _gameObjects.Add(gameObject);

        public virtual void Update(GameTime gameTime)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }
        }
    }
}
