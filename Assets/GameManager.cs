using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Toryngine;

public class GameManager : Component
{
    public float speed = 0.0015f;

    public Transform? player1Transform;
    public Transform? player2Transform;

    public override void Update()
    {
        base.Update();

        if (player1Transform == null || player2Transform == default)
        {
            Console.WriteLine("[ERR] player2Transform or player1Transform is null!");
            return;
        }

        if (Input.IsKeyDown(Keys.Left))
        {
            if (player2Transform.Position.X > 0.35f) player2Transform.Position.X -= speed;
        }
        if (Input.IsKeyDown(Keys.Right))
        {
            if (player2Transform.Position.X < 0.740f) player2Transform.Position.X += speed;
        }

        if (Input.IsKeyDown(Keys.A))
        {
            if (player1Transform.Position.X > -0.740f) player1Transform.Position.X -= speed;
        }
        if (Input.IsKeyDown(Keys.D))
        {
            if (player1Transform.Position.X < -0.35f) player1Transform.Position.X += speed;
        }
    }
}