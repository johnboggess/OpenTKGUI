using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Drawing;
using OpenTKGUI.Enums;
using System.Linq.Expressions;
using Microsoft.Win32.SafeHandles;
using System.Xml.Serialization;

namespace OpenTKGUI.GUIElements
{
    public class GUIElement
    {
        public Matrix4 Transform = Matrix4.Identity;

        public bool Focusable = false;

        public Color4 Color = Color4.White;
        public Color4 BorderColor = Color4.Black;
        public float BorderSize = 0;

        public Func<MouseButtonEventArgs, bool> OnMouseButton;
        public Action<MouseButtonEventArgs> OnGlobalMouseButton;

        public Action<MouseMoveEventArgs> OnGlobalMouseMove;

        public Action<KeyboardKeyEventArgs> OnKeyDown;
        public Action<KeyboardKeyEventArgs> OnKeyUp;
        public Action<TextInputEventArgs> OnTextInput;

        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlignment = VerticalAlignment.Top;

        public float MinWidth = 0;
        public float MinHeight = 0;

        internal GUIElement _Parent;
        internal List<GUIElement> _Children = new List<GUIElement>();

        protected bool notifyWhenDoneSizing = false;

        internal Vector2 _LocalPosition
        {
            get { return new Vector2(Transform.M41, Transform.M42); }
            set
            {
                Transform.M41 = value.X;
                Transform.M42 = value.Y;
            }
        }

        private Vector2 _globalPosition
        {
            get
            {
                if (IsRoot() || !HasParent())
                    return _LocalPosition;
                return _LocalPosition + Parent._globalPosition;
            }
        }

        public Vector2 Size = Vector2.Zero;

        public Vector2 RenderSize
        {
            get { return Transform.ExtractScale().Xy; }
            internal set
            {
                Transform.M11 = value.X;
                Transform.M22 = value.Y;
            }
        }

        public Rectangle InnerRect
        {
            get { return new Rectangle(BorderSize, BorderSize, RenderSize.X - BorderSize*2f, RenderSize.Y - BorderSize*2f); }
        }
        public Rectangle OuterRect { get { return new Rectangle(_LocalPosition, RenderSize); } }

        public GUIElement Parent
        {
            get { return _Parent; }
        }

        public bool IsFocused
        {
            get { return GUIManager.FocusedElement == this; }
        }

        internal void _ForceAddChild(GUIElement element)
        {
            GUIManager._QueueElementToBeAdded(this, element);
        }

        public void Remove()
        {
            GUIManager._QueueElementForRemoval(this);
        }

        public bool HasParent()
        {
            return Parent != null;
        }

        public bool ParentIsRoot()
        {
            return Parent == GUIManager.Root;
        }

        public bool IsRoot()
        {
            return this == GUIManager.Root;
        }

        public bool IsPointInside(Vector2 point)
        {
            Vector2 bl = _globalPosition;
            Vector2 tr = bl + RenderSize;

            return bl.X <= point.X && tr.X >= point.X && bl.Y <= point.Y && tr.Y >= point.Y;
        }

        public virtual void Draw(Vector2 parentGlobalPosition, int depth)
        {
            _DrawChildren(parentGlobalPosition, depth);
        }

        internal void _MouseButtonEvent(MouseButtonEventArgs args)
        {
            if (IsPointInside(GUIManager.GUIMousePosition()))
            {
                if (GUIManager.FocusedElement == null && Focusable)
                    GUIManager.FocusedElement = this;
                if (OnMouseButton != null)
                {
                    if (!OnMouseButton.Invoke(args))
                        return;
                }
            }
            foreach (GUIElement child in _Children)
                child._MouseButtonEvent(args);
        }

        internal void _GlobalMouseButtonEvent(MouseButtonEventArgs args)
        {
            if (OnGlobalMouseButton != null)
                OnGlobalMouseButton.Invoke(args);
            foreach (GUIElement child in _Children)
                child._GlobalMouseButtonEvent(args);
        }

        internal void _GlobalMouseMoveEvent(MouseMoveEventArgs args)
        {
            if (OnGlobalMouseMove != null)
                OnGlobalMouseMove.Invoke(args);
            foreach (GUIElement child in _Children)
                child._GlobalMouseMoveEvent(args);
        }

        protected virtual void _DrawChildren(Vector2 parentGlobalPosition, int depth)
        {
            foreach (GUIElement child in _Children)
                child.Draw(parentGlobalPosition+_LocalPosition, depth+1);
        }

        internal virtual void _CalculateSize()
        {
            RenderSize = new Vector2(0, 0);

            if (Size.X >= 0 && HorizontalAlignment != HorizontalAlignment.Stretch)
                RenderSize = new Vector2(Size.X, RenderSize.Y);

            if (Size.Y >= 0 && VerticalAlignment != VerticalAlignment.Stretch)
                RenderSize = new Vector2(RenderSize.X, Size.Y);


            if (HorizontalAlignment == HorizontalAlignment.Stretch || VerticalAlignment == VerticalAlignment.Stretch)
                GUIManager._GUIElementsToStretch.Enqueue(this);


            Rectangle areaTakenByChildern = _CalculateChildSize();
            if (Size.X < 0)
                RenderSize = new Vector2(areaTakenByChildern.Size.X + BorderSize * 2f, RenderSize.Y);
            if (Size.Y < 0)
                RenderSize = new Vector2(RenderSize.X, areaTakenByChildern.Size.Y + BorderSize * 2f);

            RenderSize = new Vector2(MathF.Max(MinWidth, RenderSize.X), MathF.Max(MinHeight, RenderSize.Y));

            if (notifyWhenDoneSizing)
                GUIManager._GUIElementsToNotifyAfterSizing.Enqueue(this);

        }

        internal virtual Rectangle _CalculateChildSize()
        {
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (GUIElement child in _Children)
            {
                child._CalculateSize();
                Rectangle outerRect = child.OuterRect;
                minX = MathF.Min(minX, outerRect.Left);
                maxX = MathF.Max(maxX, outerRect.Right);
                minY = MathF.Min(minY, outerRect.Bottom);
                maxY = MathF.Max(maxY, outerRect.Top);
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        internal virtual void _CalculatePosition()
        {
            if (Parent == null)
                _LocalPosition = Vector2.Zero;
            else
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        _LocalPosition = new Vector2(Parent.InnerRect.Left, _LocalPosition.Y);
                        break;
                    case HorizontalAlignment.Right:
                        _LocalPosition = new Vector2(Parent.InnerRect.Right - RenderSize.X, _LocalPosition.Y);
                        break;
                    case HorizontalAlignment.Center:
                        _LocalPosition = new Vector2(Parent.InnerRect.Center.X - RenderSize.X / 2f, _LocalPosition.Y);
                        break;
                    case HorizontalAlignment.Stretch:
                        _LocalPosition = new Vector2(Parent.InnerRect.Left, _LocalPosition.Y);
                        break;
                }
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        _LocalPosition = new Vector2(_LocalPosition.X, Parent.InnerRect.Top - RenderSize.Y);
                        break;
                    case VerticalAlignment.Bottom:
                        _LocalPosition = new Vector2(_LocalPosition.X, Parent.InnerRect.Bottom);
                        break;
                    case VerticalAlignment.Center:
                        _LocalPosition = new Vector2(_LocalPosition.X, Parent.InnerRect.Center.Y - RenderSize.Y / 2f);
                        break;
                    case VerticalAlignment.Stretch:
                        _LocalPosition = new Vector2(_LocalPosition.X, Parent.InnerRect.Bottom);
                        break;
                }
            }
            _CalculateChildPositions();
        }

        internal virtual void _CalculateChildPositions()
        {
            foreach (GUIElement child in _Children)
                child._CalculatePosition();
        }

        internal virtual void _ApplyStretch()
        {
            if (HorizontalAlignment == Enums.HorizontalAlignment.Stretch)
                RenderSize = new Vector2(Parent.InnerRect.Size.X, RenderSize.Y);
            if (VerticalAlignment == Enums.VerticalAlignment.Stretch)
                RenderSize = new Vector2(RenderSize.X, Parent.InnerRect.Size.Y);
        }

        internal virtual void _DoneSizing() { }
    }
}
