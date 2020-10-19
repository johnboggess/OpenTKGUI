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
        public static GUIElement Root;
        public static GUIElement FocusedElement = null;

        internal static Matrix4 _Transform;

        private static int farClipping = 10000;
        public static void Init(GameWindow gameWindow)
        {
            Shader.LoadShaders();

            GameWindow = gameWindow;
            Root = new GUIElement();

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
            Root.Draw(offset, -farClipping);
        }


        public static Vector2 GUIMousePosition()
        {
            Vector2 result = new Vector2(GameWindow.MousePosition.X, GameWindow.Size.Y - GameWindow.MousePosition.Y);
            return result;
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
