using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.Objects
{
    public class EntityGameObjectAdapter : BaseGameObject
    {
        private readonly BaseEntity _entity;

        public EntityGameObjectAdapter(BaseEntity entity)
        {
            _entity = entity;
        }

        public override void Update(GameTime gameTime)
        {
            // Forward the update call to the entity if needed
            // _entity.Update(gameTime);  // If BaseEntity has an Update method
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (_entity.HasComponent<TextureComponent>() && _entity.HasComponent<PositionComponent>())
            {
                var textureComponent = _entity.GetComponent<TextureComponent>();
                var positionComponent = _entity.GetComponent<PositionComponent>();
                spriteBatch.Draw(textureComponent.Texture, positionComponent.Position, Color.White);
            }
        }

        public BaseEntity GetEntity()
        {
            return _entity;
        }
    }
}
