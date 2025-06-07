using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using ImGuiNET;

namespace Toryngine;

public class Engine : GameWindow
{
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

    Vector2 triangleOffset = Vector2.Zero;
    Shader shader = new Shader("shader.vert", "shader.frag");
    List<GameObject> objects = new();
    Matrix4 projection;

    //--------------------------------------------------------------------------------------------

    public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    // Start
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        // shader = new Shader("shader.vert", "shader.frag");
        shader.Use();

        Texture texture = new Texture("texture.png");
        Texture texture1 = new Texture("1texture.png");
        Texture tt = new Texture("logo-dark-round.png");

        // objects.Add(new GameObject(quadVertices, quadIndices,
        //     position: new Vector2(0f, -1f), color:Color.Green, size: new Vector2(5, 1)));

        objects.Add(new GameObject(quadVertices, quadIndices,
            texture: tt));

        GenerateCircle(cirVertices, cirIndices, radius, segments);
        objects.Add(new GameObject(cirVertices.ToArray(), cirIndices.ToArray(),
            texture, position: new Vector2(-0.5f, .5f), size: new Vector2(1, 1)));

        objects.Add(new GameObject(cirVertices.ToArray(), cirIndices.ToArray(),
            texture, position: new Vector2(0.5f, 0f), size: new Vector2(1, 1)));
    }

    // Cleanup
    protected override void OnUnload()
    {
        base.OnUnload();
    }

    // Rendering
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var obj in objects)
            obj.Draw(shader);

        SwapBuffers();
    }

    // Update
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (IsKeyDown(Keys.Up)) triangleOffset.Y += 0.001f;
        if (IsKeyDown(Keys.Down)) triangleOffset.Y -= 0.001f;
        if (IsKeyDown(Keys.Left)) triangleOffset.X -= 0.001f;
        if (IsKeyDown(Keys.Right)) triangleOffset.X += 0.001f;

        Title = $"OpenTK Test | FPS: {1f / args.Time:0}";

        var mousePos = MousePosition;
        // Console.WriteLine($"Mouse Position: X={mousePos.X:0.00}, Y={mousePos.Y:0.00}");

        foreach (var obj in objects)
            obj.Update();
    }

    // Resize
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        float fixedSize = 1.0f; // например, фиксируем высоту = 2 (от -1 до 1)
        float aspect = Size.X / (float)Size.Y;

        projection = Matrix4.CreateOrthographicOffCenter(
            -fixedSize * aspect, fixedSize * aspect,
            -fixedSize, fixedSize,
            -1, 1);
        GL.Viewport(0, 0, Size.X, Size.Y);

        shader.Use();
        shader.Set("uProjection", projection);
    }

    //--------------------------------------------------------------------------------------------

    private void GenerateCircle(List<float> cirVertices, List<uint> cirIndices, float radius = 0.25f, int segments = 32)
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
    }

}
