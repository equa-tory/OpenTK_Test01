using StbImageSharp;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace Toryngine;

public class Engine : GameWindow
{
    Vector2 triangleOffset = Vector2.Zero;
    Shader shader;
    Texture texture;
    List<GameObject> objects = new();
    Matrix4 projection;

    //--------------------------------------------------------------------------------------------

    public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    // Start
    protected override void OnLoad()
    {
        base.OnLoad();

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

        shader = new Shader("shader.vert", "shader.frag");
        shader.Use();

        // Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, Size.X, Size.Y, 0, -1, 1);
        // shader.Set("uProjection", projection);

        texture = new Texture("texture.png");

        // objects.Add(new GameObject(texture, new Vector2(0.0f, 0.0f), triVertices, triIndices));
        objects.Add(new GameObject(texture, new Vector2(1.0f, 1.0f), triVertices, triIndices));
        objects.Add(new GameObject(texture, new Vector2(0.0f, 0.0f), quadVertices, quadIndices));
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
        Console.WriteLine($"Mouse Position: X={mousePos.X:0.00}, Y={mousePos.Y:0.00}");
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        float aspect = Size.X / (float)Size.Y;
        if (aspect >= 1)
            projection = Matrix4.CreateOrthographicOffCenter(-aspect, aspect, -1, 1, -1, 1);
        else
            projection = Matrix4.CreateOrthographicOffCenter(-1, 1, -1 / aspect, 1 / aspect, -1, 1);

        GL.Viewport(0, 0, Size.X, Size.Y);

        shader.Use();
        shader.Set("uProjection", projection);
    }
}