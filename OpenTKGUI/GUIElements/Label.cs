using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.Text;

using OpenTKGUI.FontRendering;
using OpenTKGUI.Enums;
using OpenTKGUI.Buffers;
using OpenTKGUI.Shaders;
using System.Runtime.CompilerServices;

namespace OpenTKGUI.GUIElements
{
    public class Label : GUIControl
    {
        public string Text
        {
            get { return _text; }
            set { _text = value; _RString = RString.Create(_text, _font); _RString.SetTextColor(_textColor) ; _matchTextSize(); }
        }
        public Font Font
        {
            get { return _font; }
            set { _font = value; _RString = RString.Create(_text, _font); _RString.SetTextColor(_textColor); _matchTextSize(); }
        }
        public Color4 TextColor
        {
            get { return _textColor; }
            set { _textColor = value; _RString.SetTextColor(_textColor); }
        }
        public Color4 BackgroundColor = Color4.Transparent;


        internal RString _RString;
        private string _text = "";
        private Font _font;
        private Color4 _textColor;

        public Label(string txt, Font font)
        {
            Font = font;
            Text = txt;
            TextColor = Color4.Black;
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            Shader.ColoredShader.Use();

            Shader.ColoredShader.SetUniform4("Color", BackgroundColor);
            Shader.ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            Shader.ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            _RString.Draw(parentGlobalPosition + _LocalPosition, depth + 1);

            base.Draw(parentGlobalPosition, depth);
        }

        internal override Rectangle _CalculateChildSize()
        {
            return new Rectangle(0, 0, _RString.GetWidth(), Font.LineHeight);
        }

        private void _matchTextSize()
        {
            Size = new Vector2(_RString.GetWidth(), _RString.GetHeight());
        }
    }
}
