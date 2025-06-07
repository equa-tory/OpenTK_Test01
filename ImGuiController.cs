using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Toryngine;

public class ImGuiController
{
    private int _vertexArray;
    private int _vertexBuffer;
    private int _indexBuffer;
    private int _shaderProgram;
    private int _fontTexture;

    private GameWindow _window;

    public ImGuiController(GameWindow window)
    {
        _window = window;
        ImGui.CreateContext();
        ImGui.StyleColorsDark();

        // Настройка IO
        var io = ImGui.GetIO();
        io.Fonts.AddFontDefault();

        // Создание текстуры для ImGui
        // var io = ImGui.GetIO();

        io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

        _fontTexture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // Установить ID текстуры в ImGui
        io.Fonts.SetTexID((IntPtr)_fontTexture);
        io.Fonts.ClearTexData();

        // Создать шейдеры, VBO, VAO, текстуру шрифта
        // Реализация ниже...

        _window.FramebufferResize += OnResize;
    }

    private void OnResize(FramebufferResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    public void Update(GameWindow wnd, float deltaSeconds)
    {
        var io = ImGui.GetIO();
        io.DisplaySize = new System.Numerics.Vector2(wnd.Size.X, wnd.Size.Y);
        io.DeltaTime = deltaSeconds;

        // Обработка ввода и прочего (добавим позже)

        ImGui.NewFrame();
    }

    public void Render()
    {
        ImGui.Render();
        // Вызвать рендер ImGuiDrawData (добавим позже)
    }

    public void Dispose()
    {
        // Очистка ресурсов (добавим позже)
    }
}