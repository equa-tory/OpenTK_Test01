#version 330 core
in vec2 texCoord;

uniform sampler2D uTexture;
uniform vec4 uColor;

out vec4 FragColor;

void main()
{
    // FragColor = texture(uTexture, texCoord) * vec4(1,.5,0,1);
    FragColor = uColor * texture(uTexture, texCoord);
}