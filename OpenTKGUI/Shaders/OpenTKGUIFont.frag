#version 330 core
out vec4 FragColor;
  
in vec2 uv;

uniform sampler2D tex;
uniform vec4 TextColor;

void main()
{
    vec4 color = texture(tex, uv);
    if(color.w == 0)
        discard;
    FragColor = color * TextColor;
} 