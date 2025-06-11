using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using ImGuiNET;

namespace Toryngine;

public class Engine : GameWindow
{
    Scene? scene;
    public Matrix4 projection;
    Shader? shader;
    private ImGuiController? imGuiController;

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

        imGuiController = new ImGuiController(Size.X, Size.Y);
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

        imGuiController?.Render();

        SwapBuffers();
    }

    // Update
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        Input.Update(KeyboardState, MouseState);

        imGuiController?.Update(this, (float)args.Time);

        ImGui.Begin("Debug");
        ImGui.Text($"FPS: {1f / args.Time:0}");
        ImGui.End();

        // Title = $"OpenTK Test | FPS: {1f / args.Time:0}";

        var mousePos = MousePosition;
        // Console.WriteLine($"Mouse Position: X={mousePos.X:0.00}, Y={mousePos.Y:0.00}");

        int index = 1;
        foreach (var obj in scene!.objects)
        {
            ImGui.PushID(index);
            if (ImGui.TreeNode($"{obj.name}##{index}"))
            {
                if (obj.GetComponent<Rigidbody>() != null)
                {
                    var rb = obj.GetComponent<Rigidbody>();
                    if (ImGui.Button("Throw"))
                        rb.ApplyForce(new Vector2(1f, 1f));
                    ImGui.Text($"Velocity: {rb.Velocity.X:0.00}, {rb.Velocity.Y:0.00}");
                }
                if (obj.GetComponent<Rotator>() != null)
                {
                    var rotr = obj.GetComponent<Rotator>();
                    var speed = rotr.speed;
                    if (ImGui.SliderFloat("Speed", ref speed, 0f, .05f))
                        rotr.speed = speed;
                }

                var transform = obj.GetComponent<Transform>();
                var pos = new System.Numerics.Vector2(transform.Position.X, transform.Position.Y);
                if (ImGui.SliderFloat2("Position", ref pos, -1f, 1f))
                    transform.Position = new OpenTK.Mathematics.Vector2(pos.X, pos.Y);
                var scale = new System.Numerics.Vector2(transform.Scale.X, transform.Scale.Y);
                if (ImGui.SliderFloat2("Scale", ref scale, -1f, 1f))
                    transform.Scale = new OpenTK.Mathematics.Vector2(scale.X, scale.Y);
                var rot = transform.Rotation;
                if (ImGui.SliderFloat("Rotation", ref rot, 0f, MathF.PI * 2f))
                    transform.Rotation = rot;

                var color = new System.Numerics.Vector4(obj.Color.X, obj.Color.Y, obj.Color.Z, obj.Color.W);
                if (ImGui.ColorEdit4("Color", ref color))
                    obj.Color = new OpenTK.Mathematics.Vector4(color.X, color.Y, color.Z, color.W);

                ImGui.TreePop();
            }
            ImGui.PopID();
            index++;
        }

        foreach (var obj in scene!.objects)
            obj.Update();



        // Restart game
        if (Input.IsKeyDown(Keys.R))
        {
            scene.objects.Clear();
            scene.Init();
        }
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

        imGuiController?.WindowResized(Size.X, Size.Y);
    }

}
