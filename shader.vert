#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform vec2 uSize;
uniform vec2 uOffset;
uniform mat4 uProjection;

out vec2 texCoord;

void main()
{
    vec2 scaled = aPosition * uSize;
    gl_Position = uProjection * vec4(scaled + uOffset, 0.0, 1.0);
    texCoord = aTexCoord;
}