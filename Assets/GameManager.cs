using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Toryngine;

public class GameManager : Component
{
    public float player1Pos = 0f;
    public Transform? player1Transform;

    public float player2Pos = 0f;
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
            if (player2Transform.Position.X > 0.35f) player2Transform.Position.X -= 0.01f;
        }
        if (Input.IsKeyDown(Keys.Right))
        {
            if (player2Transform.Position.X < 0.740f) player2Transform.Position.X += 0.01f;
        }

        if (Input.IsKeyDown(Keys.A))
        {
            if (player1Transform.Position.X > -0.740f) player1Transform.Position.X -= 0.01f;
        }
        if (Input.IsKeyDown(Keys.D))
        {
            if (player1Transform.Position.X < -0.35f) player1Transform.Position.X += 0.01f;
        }
    }
}