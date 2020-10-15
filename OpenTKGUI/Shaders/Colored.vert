#version 330 core
layout (location = 0) in vec3 VertexPos;
layout (location = 1) in vec2 UV;
  
out vec4 fragColor;

uniform mat4 globalGUITransform;
uniform mat4 elementTransform;
uniform vec4 Color;

void main()
{
    gl_Position = globalGUITransform * elementTransform * vec4(VertexPos, 1.0);
    fragColor = Color;
}