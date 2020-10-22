using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

using OpenTKGUI.GUIElements;
using OpenTKGUI.Shaders;
using System.Runtime.CompilerServices;

namespace OpenTKGUI
{
    public static class GUIManager
    {
        public static GameWindow GameWindow;
        public static Root Root;
        public static GUIElement FocusedElement = null;

        internal static Matrix4 _Transform;
        internal static List<GUIElement> _QueuedForRemoval = new List<GUIElement>();
        internal static List<Tuple<GUIElement, GUIElement>> _QueuedToAdd = new List<Tuple<GUIElement, GUIElement>>();

        private static int farClipping = 10000;
        public static void Init(GameWindow gameWindow)
        {
            Shader.LoadShaders();

            GameWindow = gameWindow;
            Root = new Root();
            Root.RenderSize = new Vector2(gameWindow.Size.X, gameWindow.Size.Y);

            _Transform = Matrix4.Identity;
            _Transform.M41 = -1;
            _Transform.M42 = -1;
            _Transform.M43 = 0;

            _Transform.M11 = 1f / ((float)gameWindow.Size.X / 2f);
            _Transform.M22 = 1f / ((float)gameWindow.Size.Y / 2f);

            _Transform = Matrix4.CreateOrthographic(gameWindow.Size.X, gameWindow.Size.Y, 0, 1000);
            _Transform = Matrix4.CreateOrthographicOffCenter(0, gameWindow.Size.X, 0, gameWindow.Size.Y, 0, farClipping);

            _setUpEvents();
        }

        public static void Draw(Vector2 offset)
        {
            foreach(GUIElement removed in _QueuedForRemoval)
            {
                removed.Parent._Children.Remove(removed);
                removed._Parent = null;
            }
            foreach(Tuple<GUIElement, GUIElement> pair in _QueuedToAdd)
            {
                pair.Item2._Parent = pair.Item1;
                pair.Item1._Children.Add(pair.Item2);
            }

            _QueuedForRemoval.Clear();
            _QueuedToAdd.Clear();

            GUIManager.CalculatePositionsAndSizes();

            Root.Draw(offset, -farClipping);
        }

        public static void CalculatePositionsAndSizes()
        {
            Root._CalculateChildSize();
            Root._CalculateChildPosition();
        }

        public static Vector2 GUIMousePosition()
        {
            Vector2 result = new Vector2(GameWindow.MousePosition.X, GameWindow.Size.Y - GameWindow.MousePosition.Y);
            return result;
        }

        internal static void _QueueElementForRemoval(GUIElement elementToRemove)
        {
            if (elementToRemove.Parent == null)
                return;
            _QueuedForRemoval.Add(elementToRemove);
        }

        internal static void _QueueElementToBeAdded(GUIElement parent, GUIElement elementToAdd)
        {
            if (elementToAdd.Parent != null)
                elementToAdd.Remove();
            _QueuedToAdd.Add(Tuple.Create(parent, elementToAdd));
        }

        private static void _setUpEvents()
        {
            Action<MouseButtonEventArgs> mouseButton = new Action<MouseButtonEventArgs>((a) => { FocusedElement = null; Root._MouseButtonEvent(a); });
            Action<MouseButtonEventArgs> globalMouseButton = new Action<MouseButtonEventArgs>((a) => { Root._GlobalMouseButtonEvent(a); });
            GameWindow.MouseDown += mouseButton;
            GameWindow.MouseDown += globalMouseButton;
            GameWindow.MouseUp += mouseButton;
            GameWindow.MouseUp += globalMouseButton;

            GameWindow.MouseMove += new Action<MouseMoveEventArgs>((a) =>
            {
                Root._GlobalMouseMoveEvent(a);
            });

            GameWindow.KeyDown += new Action<KeyboardKeyEventArgs>((a) =>
            {
                if (FocusedElement != null && FocusedElement.OnKeyDown != null)
                    FocusedElement.OnKeyDown.Invoke(a);
            });

            GameWindow.KeyUp += new Action<KeyboardKeyEventArgs>((a) =>
            {
                if (FocusedElement != null && FocusedElement.OnKeyUp != null)
                    FocusedElement.OnKeyUp.Invoke(a);
            });
            
            GameWindow.TextInput += new Action<TextInputEventArgs>((a) =>
            {
                if (FocusedElement != null && FocusedElement.OnTextInput != null)
                    FocusedElement.OnTextInput.Invoke(a);
            });
        }
    }
}
