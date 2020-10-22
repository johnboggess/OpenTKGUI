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
        public float MinWidth = 0;
        public float MinHeight = 0;

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

        public Color4 BackgroundColor { get { return _frame.Color; } set { _frame.Color = value; } }

        Frame _frame;
        Label _lbl;

        public TextBox(Font font)
        {
            Focusable = true;

            _frame = new Frame();
            _frame.HorizontalAlignment = Enums.HorizontalAlignment.Stretch;
            _frame.VerticalAlignment = Enums.VerticalAlignment.Stretch;

            _lbl = new Label("", font);

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

        internal override void _CalculateChildSize()
        {
            _lbl.Size = Size;
            applyAuto(_lbl);
            _lbl._CalculateChildSize();
            applyChildStretch(_frame);
        }

        protected override void applyAuto(GUIElement childToWrap)
        {
            if (HorizontalAlignment != HorizontalAlignment.Stretch && Size.X < 0)
            {
                RenderSize = new Vector2(MathF.Max(childToWrap.RenderSize.X + BorderSize * 2f, MinWidth), RenderSize.Y);
            }

            if (VerticalAlignment != VerticalAlignment.Stretch && Size.Y < 0)
            {
                RenderSize = new Vector2(RenderSize.X, MathF.Max(childToWrap.RenderSize.Y + BorderSize * 2f, MinHeight));
            }
        }
    }
}
