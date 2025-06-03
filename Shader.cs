using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Toryngine;

public class Shader
{
    public int Handle { get; private set; }
    
    public Shader(string vertexPath, string fragmentPath)
    {
        string vertexSource = File.ReadAllText(vertexPath);
        string fragmentSource = File.ReadAllText(fragmentPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use() => GL.UseProgram(Handle);

    public void Set(string name, Vector2 value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform2(location, value);
    }

    public void Set(string name, int value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(location, value);
    }
}
