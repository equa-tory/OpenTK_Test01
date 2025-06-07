using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Toryngine;

public class GameObject
{
    private Dictionary<Type, Component> components;
    public Transform transform;

    public Vector4 Color = Vector4.One;
    public Texture? Texture = null;
    // public Shader? shader;
    private int VAO, VBO, EBO;
    private int indexCount;

    //--------------------------------------------------------------------------------------------

    // Start
    public GameObject(float[] vertices, uint[] indices, Transform? transform = default, Color color = default, Texture? texture = null)
    {
        // shader = new Shader("Assets/Shaders/shader.vert", "Assets/Shaders/shader.frag");
        // shader?.Use();

        components = new Dictionary<Type, Component>();
        this.transform = transform == default ? new Transform(Vector2.Zero, Vector2.One, 0) : transform;
        AddComponent(this.transform);

        Color = color == default ? Vector4.One : new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        Texture = texture;
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

    // Update
    public void Update()
    {
        foreach (var comp in components.Values)
            comp.Update();

        // TODO: Tmp screen border teleport
        if (transform.Position.X < -1f) transform.Position.X = 1f;
        if (transform.Position.X > 1f) transform.Position.X = -1f;
        if (transform.Position.Y < -1f) transform.Position.Y = 1f;
        if (transform.Position.Y > 1f) transform.Position.Y = -1f;

        // TODO: Tmp screen border collision
        Rigidbody rb = GetComponent<Rigidbody>();        
        if (transform.Position.Y - transform.Scale.Y * 0.5f < -1f) // TODO: 0.5f wrong value
        {
            transform.Position.Y = -1f + transform.Scale.Y * 0.5f;
            rb.Velocity.Y = -rb.Velocity.Y * 0.5f; // отскок с потерей скорости
        }
    }
    //--------------------------------------------------------------------------------------------
    public void AddComponent(Component comp)
    {
        comp.owner = this;
        components[comp.GetType()] = comp;
    }

    public T GetComponent<T>() where T : Component
    {
        return components.TryGetValue(typeof(T), out var value) ? (T) value : null!;
    }
    public void RemoveComponent<T>() where T : Component
    {
        components.Remove(typeof(T));
    }
    //--------------------------------------------------------------------------------------------

    public void Draw(Shader shader)
    {
        shader.Set("uOffset", transform.Position);
        shader.Set("uSize", transform.Scale);
        shader.Set("uColor", Color);

        if (Texture == null) Texture = new Texture("Assets/Textures/placeholder.png");
        Texture.Bind();

        GL.BindVertexArray(VAO);
        GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
    }

}