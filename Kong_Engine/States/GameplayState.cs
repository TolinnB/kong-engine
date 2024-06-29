using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.States
{
    public class GameplayState : BaseGameState
    {
        
        private const string Player = "donkeyKong";

        private const string BackgroundTexture = "DKJunglejpg";
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
            AddGameObject(new PlayerSprite(LoadTexture(Player)));
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }
    }
}