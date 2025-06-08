using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace Toryngine;

public class Engine : GameWindow
{
    Scene? scene;
    public Matrix4 projection;
    Shader? shader;

    //--------------------------------------------------------------------------------------------

    public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings) { }

    // Start
    protected override void OnLoad()
    {
        base.OnLoad();

        scene = new Scene();

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        shader = new Shader("Assets/Shaders/shader.vert", "Assets/Shaders/shader.frag");
        shader?.Use();
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

        foreach (var obj in scene!.objects)
            obj.Draw(shader!);

        SwapBuffers();
    }

    // Update
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        Input.Update(KeyboardState, MouseState);

        // if (IsKeyPressed(Keys.W)) scene!.objects[0].GetComponent<Rigidbody>().ApplyForce(new Vector2(0, 5f));
        
        Title = $"OpenTK Test | FPS: {1f / args.Time:0}";

        var mousePos = MousePosition;
        // Console.WriteLine($"Mouse Position: X={mousePos.X:0.00}, Y={mousePos.Y:0.00}");

        foreach (var obj in scene!.objects)
            obj.Update();
    }

    // Resize
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        float fixedSize = 1.0f;
        float aspect = Size.X / (float)Size.Y;

        projection = Matrix4.CreateOrthographicOffCenter(
            -fixedSize * aspect, fixedSize * aspect,
            -fixedSize, fixedSize,
            -1, 1);
        GL.Viewport(0, 0, Size.X, Size.Y);

        shader?.Use();
        shader?.Set("uProjection", projection);
    }

}
