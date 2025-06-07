using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using StbImageSharp;


namespace Toryngine;

public class Texture
{
    public int Handle { get; private set; }

    public Texture(string path)
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle); // set texture to frag shader (uniform sampler2D uTexture)

        StbImage.stbi_set_flip_vertically_on_load(1);

        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        // Image settings
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); // generate mipmaps (diff sizes of images)
    }

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}