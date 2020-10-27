using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKGUI
{
    public struct Padding
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Padding(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
