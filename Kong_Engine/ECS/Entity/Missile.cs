using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.Entity
{
    public class Missile : BaseEntity
    {
        private Texture2D _texture;
        private Rectangle _sourceRectangle;
        private float _speed;
        private Rectangle _boundingBox;

        public Missile(Texture2D texture, Rectangle sourceRectangle, Vector2 position, float speed)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle;
            _speed = speed;
            AddComponent(new PositionComponent { Position = position });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Width, sourceRectangle.Height)
            });
            _boundingBox = new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Width, sourceRectangle.Height);
        }

        public void Update(GameTime gameTime)
        {
            var positionComponent = GetComponent<PositionComponent>();
            positionComponent.Position -= new Vector2(0, _speed) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update bounding box
            _boundingBox.X = (int)positionComponent.Position.X;
            _boundingBox.Y = (int)positionComponent.Position.Y;

            // Update collision component bounding box
            var collisionComponent = GetComponent<CollisionComponent>();
            if (collisionComponent != null)
            {
                collisionComponent.BoundingBox = _boundingBox;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var positionComponent = GetComponent<PositionComponent>();
            spriteBatch.Draw(_texture, positionComponent.Position, _sourceRectangle, Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }
    }
}
