using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    private Matrix transform;
    private Vector2 position;
    private Viewport viewport;
    private float zoom;

    public Camera(Viewport newViewport)
    {
        viewport = newViewport;
        position = Vector2.Zero;
        zoom = 1.0f;
        transform = Matrix.Identity;
    }

    public Matrix Transform
    {
        get { return transform; }
    }

    public void Move(Vector2 direction)
    {
        position += direction;
        Update();
    }

    public void Update()
    {
        transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                    Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
    }

    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
        Update();
    }

    public void SetZoom(float newZoom)
    {
        zoom = newZoom;
        Update();
    }
}
