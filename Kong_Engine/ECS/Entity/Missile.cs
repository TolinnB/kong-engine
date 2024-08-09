using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.Entity
{
    public class Missile : BaseEntity
    {
        private Texture2D _texture;
        private Rectangle _frame1;
        private Rectangle _frame2;
        private Rectangle _currentFrame;
        private float _speed;
        private Rectangle _boundingBox;
        private float _animationTime;
        private float _timeSinceLastFrameChange;

        public Missile(Texture2D texture, Rectangle frame1, Rectangle frame2, Vector2 position, float speed, float animationTime)
        {
            _texture = texture;
            _frame1 = frame1;
            _frame2 = frame2;
            _currentFrame = frame1;
            _speed = speed;
            _animationTime = animationTime;
            _timeSinceLastFrameChange = 0f;

            AddComponent(new PositionComponent { Position = position });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle((int)position.X, (int)position.Y, frame1.Width, frame1.Height)
            });
            _boundingBox = new Rectangle((int)position.X, (int)position.Y, frame1.Width, frame1.Height);
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

            // Update animation frame
            _timeSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeSinceLastFrameChange >= _animationTime)
            {
                _currentFrame = _currentFrame == _frame1 ? _frame2 : _frame1;
                _timeSinceLastFrameChange = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var positionComponent = GetComponent<PositionComponent>();
            spriteBatch.Draw(_texture, positionComponent.Position, _currentFrame, Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }
    }
}
