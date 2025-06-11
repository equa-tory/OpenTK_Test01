using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Toryngine;

public class Rigidbody : Component
{
    public Vector2 Velocity = Vector2.Zero;
    private static Vector2 Acceleration = new Vector2(0, -9.8f);
    public List<GameObject> sceneObjects = new();

    //--------------------------------------------------------------------------------------------

    public void ApplyForce(Vector2 force)
    {
        Velocity += force;
    }

    public override void Update()
    {
        foreach (var obj in sceneObjects)
        {
            if (obj == owner) continue;

            if (CheckCollision(owner, obj))
            {
                Velocity *= new Vector2(-1f, -1f);

                owner.transform.Position += Velocity.Normalized() * 0.01f;
            }
        }

        Velocity += Acceleration * 0.001f;
        owner!.transform!.Position += Velocity * 0.001f;

        // TODO: Tmp screen border teleport
        if (owner.transform.Position.X < -1f) owner.transform.Position.X = 1f;
        if (owner.transform.Position.X > 1f) owner.transform.Position.X = -1f;
        if (owner.transform.Position.Y < -1f) owner.transform.Position.Y = 1f;
        if (owner.transform.Position.Y > 1f) owner.transform.Position.Y = -1f;

        // TODO: Tmp screen border collision
        if (owner.transform.Position.Y - owner.transform.Scale.Y * 0.5f < -1f) // TODO: 0.5f wrong value
        {
            owner.transform.Position.Y = -1f + owner.transform.Scale.Y * 0.5f;
            Velocity.Y = -Velocity.Y * 0.5f; // отскок с потерей скорости
        }
    }

    //--------------------------------------------------------------------------------------------
    

    private bool CheckCollision(GameObject a, GameObject b)
    {
        var amin = a.transform.Position - a.transform.Scale * 0.5f;
        var amax = a.transform.Position + a.transform.Scale * 0.5f;

        var bmin = b.transform.Position - b.transform.Scale * 0.5f;
        var bmax = b.transform.Position + b.transform.Scale * 0.5f;

        return amax.X > bmin.X && amin.X < bmax.X &&
            amax.Y > bmin.Y && amin.Y < bmax.Y;
    }
}