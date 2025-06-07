using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Toryngine;

public class GameObject
{
    public Vector2 Position;
    public Texture Texture;
    private int VAO, VBO, EBO;

    public GameObject(Texture texture, Vector2 position, float[] vertices, uint[] indices)
    {
        Texture = texture;
        Position = position;

        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();

        GL.BindVertexArray(VAO);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    public void Draw(Shader shader)
    {        
        shader.Set("uOffset", Position);
        Texture.Bind();
        GL.BindVertexArray(VAO);
        // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }
}