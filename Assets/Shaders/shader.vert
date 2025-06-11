#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoord;

uniform vec2 uSize;
uniform float uRotation;
uniform vec2 uOffset;
uniform mat4 uProjection;

out vec2 texCoord;

void main()
{
    vec2 scaled = aPosition * uSize;

    float cosTheta = cos(uRotation);
    float sinTheta = sin(uRotation);
    vec2 rotated = vec2(
        scaled.x * cosTheta - scaled.y * sinTheta,
        scaled.x * sinTheta + scaled.y * cosTheta
    );
    gl_Position = vec4(rotated + uOffset, 0.0, 1.0);
    texCoord = aTexCoord;
}