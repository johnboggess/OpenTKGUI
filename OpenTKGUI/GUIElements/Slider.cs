using System;
using System.Collections.Generic;
using System.Text;

using OpenTKGUI.Buffers;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGUI.GUIElements
{
    public class Slider : GUIControl
    {
        public Action<float, float> OnValueChanged;

        public Color4 BackgroundColor { get { return _Background.Color; } set { _Background.Color = value; } }
        public Color4 TrackColor { get { return _Track.Color; } set { _Track.Color = value; } }
        public Color4 ThumbColor { get { return _Thumb.Color; } set { _Thumb.Color = value; } }

        public float ThumbHeight
        {
            get { return _Thumb.Size.Y; }
            set { _Thumb.Size.Y = value; }
        }
        public float ThumbWidth
        {
            get { return _Thumb.Size.X; }
            set { _Thumb.Size.X = value; }
        }

        public float TrackHeight
        {
            get { return _Track.Size.Y; }
            set { _Track.Size.Y = value; }
        }

        public float MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        public float MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }
        public float Value
        {
            get { return _value; }
            set
            {
                float old = _value;
                _value = value;
                if (OnValueChanged != null)
                    OnValueChanged(old, _value);
            }
        }

        internal Frame _Background = new Frame();
        internal Frame _Track = new Frame() { Color = Color4.LightGray };
        internal Frame _Thumb = new Frame() { Color = Color4.Blue };

        private float _minValue = 0;
        private float _maxValue = 1;
        private float _value = .5f;

        private bool _thumbSelected = false;

        Random rnd = new Random();
        public Slider()
        {
            ThumbWidth = 5;
            ThumbHeight = 15;
            TrackHeight = 5;

            _ForceAddChild(_Background);
            _Background._ForceAddChild(_Track);
            _Track._ForceAddChild(_Thumb);

            _Background.HorizontalAlignment = Enums.HorizontalAlignment.Stretch;
            _Background.VerticalAlignment = Enums.VerticalAlignment.Stretch;

            _Track.HorizontalAlignment = Enums.HorizontalAlignment.Stretch;
            _Track.VerticalAlignment = Enums.VerticalAlignment.Center;

            _Thumb.OnGlobalMouseMove = new Action<MouseMoveEventArgs>((a) =>
            {
                if (_thumbSelected)
                {
                    float dValue = (_maxValue - _minValue) / (_Track.RenderSize.X);
                    float old = Value;
                    Value = MathF.Max(MathF.Min(_value + (a.DeltaX * dValue), _maxValue), _minValue);
                }
            });

            _Thumb.OnMouseButton = new Func<MouseButtonEventArgs, bool>((a) =>
            {
                if (a.Button == MouseButton.Left && a.IsPressed)
                    _thumbSelected = true;
                return false;
            });

            _Thumb.OnGlobalMouseButton = new Action<MouseButtonEventArgs>((a) =>
            {
                if (a.Button == MouseButton.Left && !a.IsPressed)
                    _thumbSelected = false;
            });
        }

        internal override void _CalculateChildPositions()
        {
            _Background._CalculatePosition();
            _Track._CalculatePosition();

            float xoffset = _Track.RenderSize.X * (Value / (MaxValue - MinValue));
            _Thumb._LocalPosition = new Vector2(xoffset - (ThumbWidth / 2f), -ThumbHeight / 2f + TrackHeight / 2f);
        }
    }
}
