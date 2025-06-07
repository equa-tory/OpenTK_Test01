using OpenTK.Mathematics;

namespace Toryngine;

public class Transform : Component
{
    public Vector2 Position = Vector2.Zero;
    public Vector2 Scale = Vector2.One;
    public float Rotation = 0f;

    public Transform(Vector2 position = default, Vector2 scale = default, float rotation = default)
    {
        Position = position == default ? Vector2.Zero : position;
        Scale = scale == default ? Vector2.One : scale;
        Rotation = rotation == default ? 0f : rotation;
    }
}