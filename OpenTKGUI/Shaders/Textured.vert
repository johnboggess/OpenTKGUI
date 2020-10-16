#version 330 core
layout (location = 0) in vec3 VertexPos;
layout (location = 1) in vec2 UV;
  
out vec2 uv;

uniform mat4 globalGUITransform;
uniform mat4 elementTransform;

void main()
{
    gl_Position = globalGUITransform * elementTransform * vec4(VertexPos, 1.0);
    uv = UV;
}