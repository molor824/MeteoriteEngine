namespace Meteorite;

internal static class DefaultShaders
{
    internal static Shader Shader3D = new(
        @"layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec4 aColor;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 transform;

void main()
{
    vec4 position = transform * vec4(aPos, 1);
	gl_Position = position;
    fTexCoord = aTexCoord;
    fColor = aColor;
}",
        @"out vec4 FragColor;

in vec2 fTexCoord;
in vec4 fColor;

uniform sampler2D texture0;
uniform vec4 tint;

void main()
{
    FragColor = texture(texture0, fTexCoord) * fColor * tint;
}");

    internal static Shader Shader2D = new(
        @"layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec4 aColor;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 transform;

void main() {
    vec4 position = transform * vec4(aPos, 1);
    gl_Position = position;
    fTexCoord = aTexCoord;
    fColor = aColor;
}",
        @"out vec4 FragColor;

in vec4 fColor;
in vec2 fTexCoord;

uniform sampler2D texture0;
uniform vec4 tint;

void main() {
    FragColor = texture(texture0, fTexCoord) * fColor * tint;
}");
}