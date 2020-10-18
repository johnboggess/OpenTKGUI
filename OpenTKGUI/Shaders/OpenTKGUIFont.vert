#version 330 core
layout (location = 0) in vec3 VertexPos;
layout (location = 1) in vec2 UV;
  
out vec2 uv;

uniform mat4 globalGUITransform;
uniform mat4 elementTransform;
uniform float[4] uvs;

void main()
{
    gl_Position = globalGUITransform * elementTransform * vec4(VertexPos, 1.0);
    float uvX = uvs[int((VertexPos.x+1.0)/2.0) * 2];
    float uvY = uvs[(int((VertexPos.y+1.0)/2.0) * 2) + 1];
    uv = vec2(uvX, uvY);
}