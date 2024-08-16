using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine
{
    public class Stereoscopic3DManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private bool _isEnabled;
        private float _eyeSeparation;
        private float _focusDistance;

        public Stereoscopic3DManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _isEnabled = false;
            _eyeSeparation = 0.02f; // Adjust this value for the 3D effect strength
            _focusDistance = 1.0f;
        }

        public bool IsEnabled => _isEnabled;

        public void Toggle3D()
        {
            _isEnabled = !_isEnabled;
        }

        public void Render(BaseGameState currentState)
        {
            if (!_isEnabled || currentState == null) return;

            // Render for left eye
            Matrix leftEyeMatrix = Matrix.CreateTranslation(-_eyeSeparation, 0, 0) * Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up) * Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphicsDevice.Viewport.AspectRatio, 0.1f, 100f);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, leftEyeMatrix);
            currentState.Render(_spriteBatch);
            _spriteBatch.End();

            // Render for right eye
            Matrix rightEyeMatrix = Matrix.CreateTranslation(_eyeSeparation, 0, 0) * Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up) * Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphicsDevice.Viewport.AspectRatio, 0.1f, 100f);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, rightEyeMatrix);
            currentState.Render(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
