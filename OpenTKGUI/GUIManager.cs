using System;
using System.Collections.Generic;
using System.Text;

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

        internal static Shader _DefaultShader;
        internal static Shader _ColoredShader;
        internal static Shader _TexturedShader;
        internal static Shader _FontShader;
        internal static Matrix4 _Transform;

        private static int farClipping = 10000;
        public static void Init(GameWindow gameWindow)
        {
            GameWindow = gameWindow;
            Root = new GUIElement();
            _DefaultShader = new DefaultShader();
            _ColoredShader = new ColoredShader();
            _TexturedShader = new TexturedShader();
            _FontShader = new FontShader();

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

        public static void MousePressedEvent(MouseButtonEventArgs args)
        {
            Root.MouseButtonEvent(args);
        }

        public static Vector2 GUIMousePosition()
        {
            Vector2 result = new Vector2(GameWindow.MousePosition.X, GameWindow.Size.Y - GameWindow.MousePosition.Y);
            return result;
        }

        private static void _setUpEvents()
        {
            Action<MouseButtonEventArgs> mouseButton = new Action<MouseButtonEventArgs>((a) => Root.MouseButtonEvent(a));
            GameWindow.MouseDown += mouseButton;
            GameWindow.MouseUp += mouseButton;
        }
    }
}
