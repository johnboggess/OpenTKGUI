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

        public float TrackHeight
        {
            get { return _trackHeight; }
            set { _trackHeight = value; }
        }
        public float TrackLengthPadding
        {
            get { return _trackLengthPadding; }
            set { _trackLengthPadding = value; }
        }

        public float ThumbHeight
        {
            get { return _thumbHeight; }
            set { _thumbHeight = value; }
        }
        public float ThumbWidth
        {
            get { return _thumbWidth; }
            set { _thumbWidth = value; }
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


        private float _trackHeight = 10;
        private float _trackLengthPadding = 5;

        private float _thumbHeight = 15;
        private float _thumbWidth = 5;

        private float _minValue = 0;
        private float _maxValue = 1;
        private float _value = .5f;

        private bool _thumbSelected = false;

        Random rnd = new Random();
        public Slider()
        {
            _ForceAddChild(_Background);
            _Background.AddChild(_Track);
            _Track.AddChild(_Thumb);

            _Background.HorizontalAlignment = Enums.HorizontalAlignment.Stretch;
            _Background.VerticalAlignment = Enums.VerticalAlignment.Stretch;

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

        internal override void _CalculateChildSize()
        {
            applyChildStretch(_Background);
            _Thumb.RenderSize = new Vector2(ThumbWidth, ThumbHeight);
            _Track.RenderSize = new Vector2(RenderSize.X - TrackLengthPadding * 2f, TrackHeight);
        }

        internal override void _CalculateChildPosition()
        {
            _Track._LocalPosition = new Vector2(TrackLengthPadding, _Background.RenderSize.Y / 2f - TrackHeight / 2f);

            float xoffset = _Track.RenderSize.X * (Value / (MaxValue - MinValue));
            _Thumb._LocalPosition = new Vector2(xoffset - (ThumbWidth / 2f), -ThumbHeight / 2f + TrackHeight / 2f);
        }

        /*internal override void _SizeChanged()
        {
            _Background.Size = Size;
            _Track.Size = new Vector2(Size.X - TrackLengthPadding * 2f, TrackHeight);
            _Track.LocalPosition = new Vector2(TrackLengthPadding, _Background.Size.Y / 2f - TrackHeight / 2f);

            _Thumb.Size = new Vector2(ThumbWidth, ThumbHeight);
            float xoffset = _Track.Size.X * (Value / (MaxValue - MinValue));
            _Thumb.LocalPosition = new Vector2(xoffset - (ThumbWidth / 2f), -ThumbHeight / 2f + TrackHeight / 2f);
        }*/
    }
}
