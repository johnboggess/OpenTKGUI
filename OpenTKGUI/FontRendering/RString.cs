using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using OpenTKGUI;
using OpenTKGUI.Shaders;

namespace OpenTKGUI.FontRendering
{
    internal class RString
    {
        private List<RChar> _chars = new List<RChar>();
        public Transform Transform = new Transform();

        public static RString Create(string str, Font font)
        {
            List<RChar> chars = new List<RChar>();
            for(int i = 0; i < str.Length; i++)
            {
                chars.Add(new RChar(str[i], font));
            }
            return new RString(chars);
        }

        public RString(List<RChar> chars)
        {
            _chars = chars;

            int cursorX = 0;
            for (int i = 0; i < _chars.Count; i++)
            {
                int xoffset = cursorX + _chars[i].XOffest;

                _chars[i].Transform.Position.X += xoffset;

                cursorX += _chars[i].XAdvance;
                if (i < chars.Count - 1)
                    cursorX += _chars[i].Font.GetKerning(_chars[i].Char, _chars[i + 1].Char);

                int yoffset = chars[i].Font.LineHeight - chars[i].Height;
                yoffset -= chars[i].YOffset;

                _chars[i].Transform.Position.Y += yoffset;
            }
        }

        public void Draw(Shader shader, Matrix4 guiTransform)
        {
            for(int i = _chars.Count - 1; i >= 0; i--)
                _chars[i].Draw(shader, Transform.GetMatrix(), guiTransform);
        }

        public void Draw(Shader shader, Transform guiTransform)
        {
            Draw(shader, guiTransform.GetMatrix());
        }

        public void Draw(Shader shader)
        {
            Draw(shader, Matrix4.Identity);
        }

        public void SetTextColor(Color4 Color)
        {
            foreach (RChar chr in _chars)
                chr.Color = Color;
        }

        public void SetTextColor(Color4 Color, int charIndex)
        {
            _chars[charIndex].Color = Color;
        }

        public int GetWidth()
        {
            int cursorX = 0;
            for (int i = 0; i < _chars.Count; i++)
            {
                cursorX += _chars[i].XAdvance;
                if (i < _chars.Count - 1)
                    cursorX += _chars[i].Font.GetKerning(_chars[i].Char, _chars[i + 1].Char);
            }
            return cursorX;
        }
    }
}
