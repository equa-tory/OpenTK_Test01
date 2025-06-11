using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Toryngine;

public class GameManager : Component
{
    public float player2Pos = 0f;
    public Transform? player2Transform;

    public override void Update()
    {
        base.Update();

        if (player2Transform == null)
        {
            Console.WriteLine("[ERR] player2Transform is null!");
            return;
        }

        if (Input.IsKeyDown(Keys.Left)) player2Pos -= 0.01f;
        else if (Input.IsKeyDown(Keys.Right)) player2Pos += 0.01f;
    }
}