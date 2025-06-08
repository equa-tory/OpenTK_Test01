using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Toryngine;

public static class Input
{
    private static KeyboardState _keyboard;
    private static MouseState _mouse;

    public static void Update(KeyboardState keyboard, MouseState mouse)
    {
        _keyboard = keyboard;
        _mouse = mouse;
    }

    public static bool IsKeyDown(Keys key) => _keyboard.IsKeyDown(key);
    public static bool IsKeyPressed(Keys key) => _keyboard.IsKeyPressed(key);
    public static Vector2 MousePosition => _mouse.Position;
}
