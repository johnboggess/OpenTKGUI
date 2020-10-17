using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
namespace OpenTKGUI.GUIElements
{
    public class GUIElement
    {
        public Matrix4 Transform = Matrix4.Identity;
        public Func<MouseButtonEventArgs, bool> OnMouseEvent;

        internal GUIElement _Parent;

        private List<GUIElement> _children = new List<GUIElement>();

        public Vector2 LocalPosition
        {
            get { return new Vector2(Transform.M41, Transform.M42); }
            set
            {
                Transform.M41 = value.X;
                Transform.M42 = value.Y;
            }
        }

        public float Depth
        {
            get { return Transform.M43; }
            set { Transform.M43 = value; }
        }

        public Vector2 GlobalPosition
        {
            get
            {
                if (IsRoot() || !HasParent())
                    return LocalPosition;
                return LocalPosition + Parent.GlobalPosition;
            }
        }

        public Vector2 Size
        {
            get { return Transform.ExtractScale().Xy; }
            set
            {
                Transform.M11 = value.X;
                Transform.M22 = value.Y;
            }
        }

        public GUIElement Parent
        {
            get { return _Parent; }
        }

        public void AddChild(GUIElement element)
        {
            element._Parent = this;
            _children.Add(element);
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

        public void RemoveChild(GUIElement element)
        {
            element._Parent = null;
            _children.Remove(element);
        }

        public bool IsPointInside(Vector2 point)
        {
            Vector2 bl = GlobalPosition;
            Vector2 tr = bl + Size;

            return bl.X <= point.X && tr.X >= point.X && bl.Y <= point.Y && tr.Y >= point.Y;
        }

        public virtual void Draw(Vector2 parentGlobalPosition, int depth)
        {
            draw(parentGlobalPosition, depth);
        }

        public virtual void OnResize()
        {
            foreach (GUIElement child in _children)
                child.OnResize();
        }

        public virtual void OnKeyPressed(char c)
        {
            foreach (GUIElement child in _children)
                child.OnKeyPressed(c);
        }

        public virtual void MouseButtonEvent(MouseButtonEventArgs args)
        {
            if (OnMouseEvent != null && IsPointInside(GUIManager.GUIMousePosition()))
                if (!OnMouseEvent.Invoke(args))
                    return;
            foreach (GUIElement child in _children)
                child.MouseButtonEvent(args);
        }

        protected void draw(Vector2 parentGlobalPosition, int depth)
        {
            foreach (GUIElement child in _children)
                child.Draw(parentGlobalPosition+LocalPosition, depth+1);
        }
    }
}
