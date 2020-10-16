using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
namespace OpenTKGUI.Shaders
{
    internal class TexturedShader : Shader
    {
        public TexturedShader() : base("Shaders/Textured.vert", "Shaders/Textured.frag") { }
    }
}
