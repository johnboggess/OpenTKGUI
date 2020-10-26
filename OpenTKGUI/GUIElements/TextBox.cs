using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTKGUI.FontRendering;
using OpenTKGUI.Buffers;
using OpenTKGUI.Enums;

namespace OpenTKGUI.GUIElements
{
    public class TextBox : GUIControl
    {
        public string Text
        {
            get { return _lbl.Text; }
            set { _lbl.Text = value; }
        }

        public new float BorderSize { get { return _frame.BorderSize; } set { _frame.BorderSize = value; } }

        public Color4 TextColor
        {
            get { return _lbl.TextColor; }
            set { _lbl.TextColor = value; }
        }

        public Color4 BackgroundColor { get { return _frame.Color; } set { _frame.Color = value; } }
        public new Color4 BorderColor { get { return _frame.BorderColor; } set { _frame.BorderColor = value; } }

        Frame _frame;
        Label _lbl;

        public TextBox(Font font)
        {
            Focusable = true;

            _frame = new Frame();
            _frame.HorizontalAlignment = Enums.HorizontalAlignment.Stretch;
            _frame.VerticalAlignment = Enums.VerticalAlignment.Stretch;

            _lbl = new Label("", font);
            _lbl.Size = new Vector2(-1, font.LineHeight);

            _ForceAddChild(_frame);
            _ForceAddChild(_lbl);

            OnTextInput = new Action<TextInputEventArgs>((a) =>
            {
                Text += a.AsString;
            });

            OnKeyDown = new Action<KeyboardKeyEventArgs>((a) =>
            {
                if (a.Key == Keys.Backspace && Text.Length > 0)
                    Text = Text.Substring(0, Text.Length - 1);
            });
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            _frame.Draw(parentGlobalPosition + _LocalPosition, depth + 2);
            _lbl.Draw(parentGlobalPosition + _LocalPosition, depth + 2);

            base.Draw(parentGlobalPosition, depth);
        }

        internal override void _CalculateSize()
        {
            base._CalculateSize();
        }
    }
}
