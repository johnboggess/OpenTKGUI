using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTKGUI.FontRendering;
using OpenTKGUI.Buffers;

namespace OpenTKGUI.GUIElements
{
    public class TextBox : GUIElement
    {
        public string Text 
        { 
            get { return _lbl.Text; }
            set { _lbl.Text = value; }
        }

        public Color4 TextColor
        {
            get { return _lbl.TextColor; }
            set { _lbl.TextColor = value; }
        }

        public Color4 BackgroundColor = Color4.White;

        Label _lbl;

        public TextBox(Font font)
        {
            Focusable = true;
            _lbl = new Label("", font);

            OnTextInput = new Action<TextInputEventArgs>((a) =>
            {
                Text += a.AsString;
            });

            OnKeyDown = new Action<KeyboardKeyEventArgs>((a) =>
            {
                if (a.Key == Keys.Backspace)
                    Text = Text.Substring(0, Text.Length-1);
            });
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            GUIManager._ColoredShader.Use();

            GUIManager._ColoredShader.SetUniform4("Color", BackgroundColor);
            GUIManager._ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            _lbl.Draw(parentGlobalPosition + LocalPosition, depth + 1);

            base.Draw(parentGlobalPosition, depth);
        }
    }
}
