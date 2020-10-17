using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.Text;

using OpenTKGUI.FontRendering;
using OpenTKGUI.Buffers;

namespace OpenTKGUI.GUIElements
{
    public class Label : GUIElement
    {
        public string Text 
        { 
            get { return _text; } 
            set { _text = value; _RString = RString.Create(_text, _font); _matchTextSize(); }
        }
        public Font Font
        {
            get { return _font; }
            set { _font = value; _RString = RString.Create(_text, _font); _matchTextSize(); }
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
            GUIManager._ColoredShader.Use();

            GUIManager._ColoredShader.SetUniform4("Color", BackgroundColor);
            GUIManager._ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            _RString.Draw(parentGlobalPosition + LocalPosition, depth + 1);

            base.Draw(parentGlobalPosition, depth);
        }

        private void _matchTextSize()
        {
            Size = new Vector2(_RString.GetWidth(), _RString.GetHeight());
        }
    }
}
