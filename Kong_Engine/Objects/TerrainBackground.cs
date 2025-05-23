﻿using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class TerrainBackground : BaseGameObject
    {
        private Vector2 _scrollSpeed;
        private bool _isScrollingEnabled;

        public TerrainBackground(Texture2D texture, Vector2 scrollSpeed, bool isScrollingEnabled = true)
        {
            _texture = texture;
            _position = Vector2.Zero;
            _scrollSpeed = scrollSpeed;
            _isScrollingEnabled = isScrollingEnabled;
        }

        public bool IsScrollingEnabled
        {
            get { return _isScrollingEnabled; }
            set { _isScrollingEnabled = value; }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            int tilesX = _isScrollingEnabled ? (viewport.Width / _texture.Width + 2) : 1;
            int tilesY = _isScrollingEnabled ? (viewport.Height / _texture.Height + 1) : 1;

            for (int y = 0; y < tilesY; y++)
            {
                for (int x = 0; x < tilesX; x++)
                {
                    var destinationRectangle = new Rectangle(
                        (int)_position.X + x * _texture.Width,
                        (int)_position.Y + y * _texture.Height,
                        _texture.Width,
                        _texture.Height);

                    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        public void UpdateBackgroundPosition(Vector2 playerPosition)
        {
            if (_isScrollingEnabled)
            {
                _position.X = -playerPosition.X * _scrollSpeed.X % _texture.Width;
                _position.Y = -playerPosition.Y * _scrollSpeed.Y % _texture.Height;

                if (_position.X > 0)
                {
                    _position.X -= _texture.Width;
                }

                if (_position.Y > 0)
                {
                    _position.Y -= _texture.Height;
                }
            }
        }
    }
}
