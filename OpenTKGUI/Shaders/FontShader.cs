﻿using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
namespace OpenTKGUI.Shaders
{
    internal class FontShader : Shader
    {
        public FontShader() : base("Shaders/Font.vert", "Shaders/Font.frag") { }
    }
}
