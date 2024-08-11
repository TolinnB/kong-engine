using Microsoft.Xna.Framework;

public class Camera
{
    public Matrix Transform { get; private set; }
    public Vector2 Position { get; set; }
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; } = 0.0f;

    private int viewportWidth;
    private int viewportHeight;
    private int worldWidth;
    private int worldHeight;

    public Camera(int viewportWidth, int viewportHeight, int worldWidth, int worldHeight)
    {
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        this.worldWidth = worldWidth;
        this.worldHeight = worldHeight;
        Position = Vector2.Zero;
    }

    public void Update()
    {
        Transform =
            Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom) *
            Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f, viewportHeight * 0.5f, 0));

        // Clamp the camera position to the world bounds
        Position = Vector2.Clamp(Position, new Vector2(viewportWidth * 0.5f / Zoom, viewportHeight * 0.5f / Zoom),
                                 new Vector2(worldWidth - (viewportWidth * 0.5f / Zoom), worldHeight - (viewportHeight * 0.5f / Zoom)));
    }

    public void Move(Vector2 amount)
    {
        Position += amount;
    }
}
