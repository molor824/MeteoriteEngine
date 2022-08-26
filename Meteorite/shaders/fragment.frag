#version 330 core

out vec4 FragColor;

in vec2 fTexCoord;
in vec4 fColor;

uniform sampler2D texture0;
uniform vec4 textureColor;

void main()
{
    FragColor = texture(texture0, fTexCoord) * fColor * textureColor;
}