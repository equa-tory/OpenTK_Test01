#version 330 core
out vec4 FragColor;
uniform sampler2D uTexture;
in vec2 texCoord;

void main()
{
    // FragColor = texture(uTexture, texCoord) * vec4(1,.5,0,1);
    FragColor = texture(uTexture, texCoord);
}