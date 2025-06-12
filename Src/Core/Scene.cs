using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Toryngine;

public class Scene
{
    public List<GameObject> objects = new ();


    Texture texture = new Texture("Assets/Textures/texture.png");
    Texture texture1 = new Texture("Assets/Textures/1texture.png");
    Texture tt = new Texture("Assets/Textures/logo-dark-round.png");
    Texture maxwell = new Texture("Assets/Textures/maxwell.png");

    #region Shapes
    float[] triVertices = {
        // positions    // tex coords
            0.0f,  0.5f,   0.5f, 1.0f, // top
        -0.5f, -0.5f,   0.0f, 0.0f, // bottom left
            0.5f, -0.5f,   1.0f, 0.0f, // bottom right
    };
    uint[] triIndices = { 0, 1, 2 };

    float[] quadVertices = {
        // positions   // tex coords
        -0.5f,  0.5f,   0.0f, 1.0f, // top left
        -0.5f, -0.5f,   0.0f, 0.0f, // bottom left
        0.5f, -0.5f,   1.0f, 0.0f, // bottom right
        0.5f,  0.5f,   1.0f, 1.0f // top right
    };
    uint[] quadIndices = { 0, 1, 2, 2, 3, 0 };
    
    // Cirlce
    int segments = 32;
    float radius = 0.5f;
    List<float> cirVertices = new();
    List<uint> cirIndices = new();
    #endregion

    //--------------------------------------------------------------------------------------------

    public Scene()
    {
        Init();
    }

    public void Init()
    {
        GameObject gm = new GameObject(null, null, "GameManager");
        gm.AddComponent(new GameManager());
        objects.Add(gm);

        GameObject wall = DrawQuad("Wall", transform: new Transform(new Vector2(0, -0.75f), new Vector2(0.2f, 0.75f)), texture: texture);
        wall.AddComponent(new Collider());

        Transform ballTransform = new Transform(new Vector2(-.5f, 0f), new Vector2(.1f,.1f));
        GameObject ball = DrawCircle("Ball", ballTransform, Color.Pink, texture1);
        ball.AddComponent(new Rigidbody());
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.sceneObjects = objects;

        GameObject player1 = DrawQuad("Player1", new Transform(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, 0.1f), -.5f), color: Color.Blue);
        GameObject player2 = DrawQuad("Player2", new Transform(new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.1f), .5f), color: Color.Orange);
        player1.AddComponent(new Collider());
        player2.AddComponent(new Collider());

        gm.GetComponent<GameManager>().player1Transform = player1.GetComponent<Transform>();
        gm.GetComponent<GameManager>().player2Transform = player2.GetComponent<Transform>();
    }

    //--------------------------------------------------------------------------------------------

    public GameObject DrawQuad(string? name = default, Transform? transform = null, Color color = default, Texture? texture = null)
    {
        GameObject go = new GameObject(quadVertices, quadIndices, name, transform, color, texture);
        objects.Add(go);
        return go;
    }
    public GameObject DrawTriangle(string? name = default, Transform? transform = null, Color color = default, Texture? texture = null)
    {
        GameObject go = new GameObject(triVertices, triIndices, name, transform, color, texture);
        objects.Add(go);
        return go;
    }
    public GameObject DrawCircle(string? name = default, Transform? transform = null, Color color = default, Texture? texture = null)
    {
        // Circle draw
        // center
        cirVertices.Add(0); cirVertices.Add(0); // xy
        cirVertices.Add(0.5f); cirVertices.Add(0.5f); // uv

        for (int i = 0; i <= segments; i++)
        {
            float angle = i / (float)segments * MathF.PI * 2;
            float x = MathF.Cos(angle) * radius;
            float y = MathF.Sin(angle) * radius;

            cirVertices.Add(x); cirVertices.Add(y);
            cirVertices.Add((x + 1) / 2); // uv x
            cirVertices.Add((y + 1) / 2); // uv y

            if (i > 0)
            {
                cirIndices.Add(0);
                cirIndices.Add((uint)i);
                cirIndices.Add((uint)(i + 1));
            }
        }

        // Draw
        GameObject go = new GameObject(cirVertices.ToArray(), cirIndices.ToArray(), name, transform, color, texture);
        objects.Add(go);
        return go;
}
    }
