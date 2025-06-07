using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Toryngine;

class Program
{
    static void Main(string[] args)
    {
        var windowSize = new Vector2i(800, 600);

        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = windowSize,
            Title = "Toryngine",
        };

        using var engine = new Engine(GameWindowSettings.Default, nativeWindowSettings);

        engine.KeyDown += (KeyboardKeyEventArgs e) =>
        {
            if (e.Key == Keys.Escape)
                engine.Close();
        };

        engine.Run();
    }
}