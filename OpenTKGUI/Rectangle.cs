using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKGUI
{
    public struct Rectangle
    {
        public Vector2 Origin;
        public Vector2 Size;

        public float X
        {
            get { return Origin.X; }
        }

        public float Y
        {
            get { return Origin.Y; }
        }

        public float Left { get { return Origin.X; } }
        public float Right { get { return Origin.X + Size.X; } }
        public float Bottom { get { return Origin.Y; } }
        public float Top { get { return Origin.Y+Size.Y; } }

        public Vector2 Center { get { return Origin + (Size * .5f); } }

        public Rectangle(Vector2 origin, Vector2 size)
        {
            Origin = origin;
            Size = size;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            Origin = new Vector2(x, y);
            Size = new Vector2(width, height);
        }
    }
}
