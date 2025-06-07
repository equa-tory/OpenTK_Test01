using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Toryngine;

public class GameObject
{
    public Vector4 Color = Vector4.One;
    public Vector2 Position = Vector2.Zero;
    public Vector2 Size = Vector2.One;
    public Texture Texture = null;
    private int VAO, VBO, EBO;
    private int indexCount;

    //--------------------------------------------------------------------------------------------

    public GameObject(float[] vertices, uint[] indices,
        Texture texture = default, Color color = default, Vector2 position = default, Vector2 size = default)
    {
        Color = color == default ? Vector4.One : new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);

        Texture = texture == default ? null : texture;
        Position = position == default ? Vector2.Zero : position;
        Size = size == default ? Vector2.One : size;
        indexCount = indices.Length;

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
        shader.Set("uSize", Size);
        shader.Set("uColor", Color);
        
        if (Texture == null) Texture = new Texture("placeholder.png");
        Texture.Bind();

        GL.BindVertexArray(VAO);
        GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
    }
    
}