using StbImageSharp;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Toryngine;

class Program
{
    static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "OpenTK Test",
        };

        using var window = new GameWindow(GameWindowSettings.Default, nativeWindowSettings);

        window.KeyDown += (KeyboardKeyEventArgs e) =>
        {
            if (e.Key == Keys.Escape)
                window.Close();
        };

        float[] vertices = {
            // positions    // tex coords
             0.0f,  0.5f,   0.5f, 1.0f,
            -0.5f, -0.5f,   0.0f, 0.0f,
             0.5f, -0.5f,   1.0f, 0.0f
        };

        int VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        int vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        // GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
        // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        string vertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec2 aPosition;
        layout (location = 1) in vec2 aTexCoord;
        uniform vec2 uOffset;
        out vec2 texCoord;

        void main()
        {
            texCoord = aTexCoord;
            gl_Position = vec4(aPosition + uOffset, 0.0, 1.0);
        }";

        string fragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;
        uniform sampler2D uTexture;
        in vec2 texCoord;

        void main()
        {
            FragColor = texture(uTexture, texCoord);
        }";
            // FragColor = vec4(1.0, 0.5, 0.2, 1.0);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        int offsetLocation = GL.GetUniformLocation(shaderProgram, "uOffset");
        GL.UseProgram(shaderProgram);

        int texture;
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


        Vector2 triangleOffset = Vector2.Zero;

        window.RenderFrame += (FrameEventArgs e) =>
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            // window.SwapBuffers();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vertexArrayObject);
            GL.Uniform2(offsetLocation, triangleOffset);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            window.SwapBuffers();
        };

        window.UpdateFrame += (FrameEventArgs e) =>
        {
            // перемещение, столкновения и т.д.
            if (window.IsKeyDown(Keys.Up)) triangleOffset.Y += 0.001f;
            if (window.IsKeyDown(Keys.Down)) triangleOffset.Y -= 0.001f;
            if (window.IsKeyDown(Keys.Left)) triangleOffset.X -= 0.001f;
            if (window.IsKeyDown(Keys.Right)) triangleOffset.X += 0.001f;

            window.Title = $"OpenTK Test | FPS: {1f / e.Time:0}";

            var mousePos = window.MousePosition;
            Console.WriteLine($"Mouse Position: X={mousePos.X:0.00}, Y={mousePos.Y:0.00}");
        };

        window.Run();
    }
}