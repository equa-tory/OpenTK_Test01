using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Toryngine;

public class Scene
{
    public List<GameObject> objects = new List<GameObject>();


    Texture texture = new Texture("Assets/Textures/texture.png");
    Texture texture1 = new Texture("Assets/Textures/1texture.png");
    Texture tt = new Texture("Assets/Textures/logo-dark-round.png");

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
    float radius = 0.25f;
    List<float> cirVertices = new();
    List<uint> cirIndices = new();
    #endregion

    //--------------------------------------------------------------------------------------------

    public Scene()
    {
        GameObject quad = DrawQuad(texture: tt);
        quad.AddComponent(new Rigidbody());

        GameObject circle = DrawCircle(transform: new Transform(position: new Vector2(1f, 0.5f)));
        circle.AddComponent(new Rigidbody());
    }

    //--------------------------------------------------------------------------------------------

    public GameObject DrawQuad(Transform? transform = null, Color color = default, Texture? texture = null)
    {
        GameObject go = new GameObject(quadVertices, quadIndices, transform, color, texture);
        objects.Add(go);
        return go;
    }
    public GameObject DrawTriangle(Transform? transform = null, Color color = default, Texture? texture = null)
    {
        GameObject go = new GameObject(triVertices, triIndices, transform, color, texture);
        objects.Add(go);
        return go;
    }
    public GameObject DrawCircle(Transform? transform = null, Color color = default, Texture? texture = null)
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
        GameObject go = new GameObject(cirVertices.ToArray(), cirIndices.ToArray(), transform, color, texture);
        objects.Add(go);
        return go;
    }
}