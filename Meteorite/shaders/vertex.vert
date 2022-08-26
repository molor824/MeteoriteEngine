#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aColor;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 transform;

void main()
{
    gl_Position = transform * vec4(aPos, 1);
    fTexCoord = aTexCoord;
    fColor = vec4(aColor, 1);
}