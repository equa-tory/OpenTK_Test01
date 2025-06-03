using StbImageSharp;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace Toryngine;

class Engine : GameWindow
{
    Vector2 triangleOffset = Vector2.Zero;
    Shader shader;
    int VAO, VBO;
    int texture;

    //--------------------------------------------------------------------------------------------

    public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    // Start
    protected override void OnLoad()
    {
        base.OnLoad();

        float[] vertices = {
            // positions    // tex coords
             0.0f,  0.5f,   0.5f, 1.0f,
            -0.5f, -0.5f,   0.0f, 0.0f,
             0.5f, -0.5f,   1.0f, 0.0f
        };

        VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        VAO = GL.GenVertexArray();
        GL.BindVertexArray(VAO);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        shader = new Shader("shader.vert", "shader.frag");
        shader.Use();


        using (var stream = File.OpenRead("texture.png"))
        {
            var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                          PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
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
        // window.SwapBuffers();
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(shader.Handle);
        GL.BindVertexArray(VAO);
        // GL.Uniform2(offsetLocation, triangleOffset);
        shader.Set("uOffset", triangleOffset);
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
}