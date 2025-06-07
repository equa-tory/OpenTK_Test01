using OpenTK.Mathematics;

namespace Toryngine;

public class Rigidbody : Component
{
    public Vector2 Velocity = Vector2.Zero;
    private static Vector2 Acceleration = new Vector2(0, -9.8f);

    //--------------------------------------------------------------------------------------------

    public void ApplyForce(Vector2 force)
    {
        Velocity += force;
    }

    public override void Update()
    {
        Velocity += Acceleration * 0.001f;
        owner!.transform!.Position += Velocity * 0.001f;
    }
}