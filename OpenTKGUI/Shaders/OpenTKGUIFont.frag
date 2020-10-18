#version 330 core
out vec4 FragColor;
  
in vec2 uv;

uniform sampler2D tex;
uniform vec4 TextColor;

void main()
{
    vec4 color = texture(tex, uv);
    //color.w = 0;
    //color.w = length(color)/1.0;
    FragColor = color * TextColor;
} 