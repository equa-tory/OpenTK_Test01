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
                var mtv = ResolveCollision(owner, obj);
                owner.transform.Position += mtv;

                // owner.transform.Position += Velocity.Normalized() * 0.01f;

                if (mtv.X != 0) Velocity.X = -Velocity.X * 0.8f;
                if (mtv.Y != 0) Velocity.Y = -Velocity.Y * 0.8f;
            }
        }

        Velocity += Acceleration * 0.001f;
        owner!.transform!.Position += Velocity * 0.001f;

        // TODO: Tmp screen border teleport
        // if (owner.transform.Position.X < -1f) owner.transform.Position.X = 1f;
        // if (owner.transform.Position.X > 1f) owner.transform.Position.X = -1f;
        // if (owner.transform.Position.Y < -1f) owner.transform.Position.Y = 1f;
        // if (owner.transform.Position.Y > 1f) owner.transform.Position.Y = -1f;

        // Tmp screen border collision
        if (owner.transform.Position.Y - owner.transform.Scale.Y * 0.5f < -1f)
        {
            owner.transform.Position.Y = -1f + owner.transform.Scale.Y * 0.5f;
            Velocity.Y = -Velocity.Y * 0.5f;
        }
        else if (owner.transform.Position.Y + owner.transform.Scale.Y * 0.5f > 1f)
        {
            owner.transform.Position.Y = 1f - owner.transform.Scale.Y * 0.5f;
            Velocity.Y = -Velocity.Y * 0.5f;
        }

        if (owner.transform.Position.X - owner.transform.Scale.X * 0.5f < -1f)
        {
            owner.transform.Position.X = -1f + owner.transform.Scale.X * 0.5f;
            Velocity.X = -Velocity.X * 0.5f;
        }
        else if (owner.transform.Position.X + owner.transform.Scale.X * 0.5f > 1f)
        {
            owner.transform.Position.X = 1f - owner.transform.Scale.X * 0.5f;
            Velocity.X = -Velocity.X * 0.5f;
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

    bool CircleAABB(Vector2 circlePos, float radius, GameObject box)
    {
        var boxMin = box.transform.Position - box.transform.Scale * 0.5f;
        var boxMax = box.transform.Position + box.transform.Scale * 0.5f;

        // ближайшая точка на прямоугольнике к центру круга
        float x = Math.Clamp(circlePos.X, boxMin.X, boxMax.X);
        float y = Math.Clamp(circlePos.Y, boxMin.Y, boxMax.Y);

        float dx = x - circlePos.X;
        float dy = y - circlePos.Y;

        return dx * dx + dy * dy < radius * radius;
    }
    
    private Vector2 ResolveCollision(GameObject a, GameObject b)
    {
        var amin = a.transform.Position - a.transform.Scale * 0.5f;
        var amax = a.transform.Position + a.transform.Scale * 0.5f;

        var bmin = b.transform.Position - b.transform.Scale * 0.5f;
        var bmax = b.transform.Position + b.transform.Scale * 0.5f;

        Vector2 mtv = Vector2.Zero; // minimum translation vector

        float dx = (amax.X + amin.X) / 2 - (bmax.X + bmin.X) / 2;
        float px = (a.transform.Scale.X + b.transform.Scale.X) * 0.5f - MathF.Abs(dx);

        float dy = (amax.Y + amin.Y) / 2 - (bmax.Y + bmin.Y) / 2;
        float py = (a.transform.Scale.Y + b.transform.Scale.Y) * 0.5f - MathF.Abs(dy);

        if (px < py)
        {
            mtv.X = dx < 0 ? -px : px;
        }
        else
        {
            mtv.Y = dy < 0 ? -py : py;
        }

        return mtv;
    }
}