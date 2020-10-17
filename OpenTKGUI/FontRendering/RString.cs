using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using OpenTKGUI;
using OpenTKGUI.Shaders;
using OpenTKGUI.GUIElements;

namespace OpenTKGUI.FontRendering
{
    internal class RString : GUIElement
    {
        private List<RChar> _chars = new List<RChar>();

        public static RString Create(string str, Font font)
        {
            List<RChar> chars = new List<RChar>();
            for(int i = 0; i < str.Length; i++)
            {
                if(font.Characters.ContainsKey(str[i]))
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

                _chars[i].LocalPosition += new Vector2(xoffset, 0);

                cursorX += _chars[i].XAdvance;
                if (i < chars.Count - 1)
                    cursorX += _chars[i].Font.GetKerning(_chars[i].Char, _chars[i + 1].Char);

                int yoffset = chars[i].Font.LineHeight - chars[i].Height;
                yoffset -= chars[i].YOffset;

                _chars[i].LocalPosition += new Vector2(0, yoffset);
            }
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            for (int i = _chars.Count - 1; i >= 0; i--)
                _chars[i].Draw(parentGlobalPosition, depth);
            base.Draw(parentGlobalPosition, depth);
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

        public int GetHeight()//todo: get height of string
        {
            /*int max = int.MinValue;
            int min = int.MaxValue;
            for (int i = 0; i < _chars.Count; i++)
            {
                int bottom = _chars[i].Font.LineHeight - _chars[i].Height;
                bottom -= _chars[i].YOffset;
                min = Math.Min(min, bottom);
                max = Math.Max(max, bottom + _chars[i].Height);
            }
            return max-min;*/

            if (_chars.Count == 0)
                return 0;
            return _chars[0].Font.LineHeight;
        }
    }
}
