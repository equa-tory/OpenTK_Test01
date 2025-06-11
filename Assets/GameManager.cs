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

        if (player1Transform == null || player2Transform == default) {
            Console.WriteLine("[ERR] player2Transform or player1Transform is null!");
            return;
        }

        if (Input.IsKeyDown(Keys.Left)) player2Pos -= 0.01f;
        else if (Input.IsKeyDown(Keys.Right)) player2Pos += 0.01f;
    }
}