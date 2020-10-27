using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKGUI
{
    public struct Margin
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Margin(float left = 0, float right = 0, float top = 0, float bottom = 0)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
