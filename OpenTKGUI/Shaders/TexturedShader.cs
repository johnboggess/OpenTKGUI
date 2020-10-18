using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
namespace OpenTKGUI.Shaders
{
    internal class TexturedShader : Shader
    {
        public TexturedShader() : base("Shaders/OpenTKGUITextured.vert", "Shaders/OpenTKGUITextured.frag") { }
    }
}
