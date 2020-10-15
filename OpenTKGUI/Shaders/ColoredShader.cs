using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
namespace OpenTKGUI.Shaders
{
    internal class ColoredShader : Shader
    {
        public ColoredShader() : base("Shaders/Colored.vert", "Shaders/Default.frag") { }
    }
}
