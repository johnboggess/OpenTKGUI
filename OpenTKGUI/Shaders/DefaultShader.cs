﻿using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
namespace OpenTKGUI.Shaders
{
    internal class DefaultShader : Shader
    {
        public DefaultShader() : base("Shaders/Default.vert", "Shaders/Default.frag") { }
    }
}