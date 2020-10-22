using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Drawing;
using OpenTKGUI.Enums;

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

        internal GUIElement _Parent;
        internal List<GUIElement> _Children = new List<GUIElement>();

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
            draw(parentGlobalPosition, depth);
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

        protected void draw(Vector2 parentGlobalPosition, int depth)
        {
            foreach (GUIElement child in _Children)
                child.Draw(parentGlobalPosition+_LocalPosition, depth+1);
        }

        internal virtual void _CalculateChildSize()
        {
            foreach (GUIElement child in _Children)
            {
                applyChildStretch(child);

                child._CalculateChildSize();

                applyAuto(child);
            }
        }

        protected virtual void applyChildStretch(GUIElement child)
        {
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    child.RenderSize = new Vector2(InnerRect.Size.X, child.RenderSize.Y);
                    child._LocalPosition = new Vector2(InnerRect.Left, child._LocalPosition.Y);
                    break;
                default:
                    child.RenderSize = new Vector2(child.Size.X, child.RenderSize.Y);
                    break;

            }
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    child.RenderSize = new Vector2(child.RenderSize.X, InnerRect.Size.Y);
                    child._LocalPosition = new Vector2(child._LocalPosition.X, InnerRect.Bottom);
                    break;
                default:
                    child.RenderSize = new Vector2(child.RenderSize.X, child.Size.Y);
                    break;
            }
        }

        protected virtual void applyAuto(GUIElement childToWrap)
        {
            if (HorizontalAlignment != HorizontalAlignment.Stretch && Size.X < 0)
            {
                RenderSize = new Vector2(childToWrap.RenderSize.X + BorderSize * 2f, RenderSize.Y);
            }

            if (VerticalAlignment != VerticalAlignment.Stretch && Size.Y < 0)
            {
                RenderSize = new Vector2(RenderSize.X, childToWrap.RenderSize.Y + BorderSize * 2f);
            }
        }

        internal virtual void _CalculateChildPosition()
        {
            if (_Children.Count == 0)
                return;

            foreach (GUIElement child in _Children)
            {
                applyChildAlignment(child);
                child._CalculateChildPosition();
            }
        }

        protected virtual void applyChildAlignment(GUIElement child)
        {
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    child._LocalPosition = new Vector2(InnerRect.Left, child._LocalPosition.Y);
                    break;
                case HorizontalAlignment.Right:
                    child._LocalPosition = new Vector2(InnerRect.Right - child.RenderSize.X, child._LocalPosition.Y);
                    break;
                case HorizontalAlignment.Center:
                    child._LocalPosition = new Vector2(InnerRect.Center.X - child.RenderSize.X / 2f, child._LocalPosition.Y);
                    break;
            }
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    child._LocalPosition = new Vector2(child._LocalPosition.X, InnerRect.Top - child.RenderSize.Y);
                    break;
                case VerticalAlignment.Bottom:
                    child._LocalPosition = new Vector2(child._LocalPosition.X, InnerRect.Bottom);
                    break;
                case VerticalAlignment.Center:
                    child._LocalPosition = new Vector2(child._LocalPosition.X, InnerRect.Center.Y - child.RenderSize.Y / 2f);
                    break;
            }
        }
    }
}
