using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTKGUI.FontRendering;

namespace OpenTKGUI.GUIElements
{
    public class Button : Frame
    {
        public Color4 TextColor { get { return _lbl.Color; } set { _lbl.Color = value; } }
        public new Color4 Color { get { return _originalBackground; } set { _originalBackground = value; } }
        public Color4 HoverColor = Color4.LightGray;
        public Color4 ClickColor = Color4.Gray;
        public Font Font { get { return _lbl.Font; } set { _lbl.Font = value; } }

        Label _lbl;
        Color4 _originalBackground;

        public Button(string txt, Font ft)
        {
            Color = Color4.White;

            _lbl = new Label(txt, ft);

            _lbl.Size = new Vector2(-1, -1);
            _lbl.HorizontalAlignment = Enums.HorizontalAlignment.Center;
            _lbl.VerticalAlignment = Enums.VerticalAlignment.Center;

            _ForceAddChild(_lbl);

            OnMouseEnter = new Action<OpenTK.Windowing.Common.MouseMoveEventArgs>((a) =>
            {
                base.Color = HoverColor;
            });

            OnMouseExit = new Action<OpenTK.Windowing.Common.MouseMoveEventArgs>((a) =>
            {
                base.Color = _originalBackground;
            });

            OnMouseButton = new Func<OpenTK.Windowing.Common.MouseButtonEventArgs, bool>((a) =>
            {
                OnMouseButtonAdjustColor(a);
                return false;
            });
        }

        public void OnMouseButtonAdjustColor(MouseButtonEventArgs args)
        {
            if (args.IsPressed && args.Button == MouseButton.Left)
                base.Color = ClickColor;
            if (!args.IsPressed && args.Button == MouseButton.Left)
                base.Color = HoverColor;
        }
    }
}
