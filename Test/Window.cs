using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.CompilerServices;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTKGUI.Shaders;
using OpenTKGUI.GUIElements;
using OpenTKGUI.FontRendering;
using OpenTKGUI;

namespace Test
{
    class Window : GameWindow
    {
        Label Label;
        Frame Border;
        Frame Frame;
        public Window(GameWindowSettings gws, NativeWindowSettings nws) : base (gws, nws)
        {
        }

        protected override void OnLoad()
        {
            Random rng = new Random();

            GUIManager.Init(this);
            FontManager.Init("Fonts");

            Label = new Label("The quick brown fox jumps over the lazy dog", FontManager.GetFont("Arial", 32)) { LocalPosition = new Vector2(5, 2) };
            Border = new Frame() { Size = new Vector2(Label.Size.X + 10, 40), Color = Color4.Black };
            Border.LocalPosition = new Vector2(Size.X / 2 - Border.Size.X / 2, Size.Y / 2 - Border.Size.Y / 2);
            Frame = new Frame() { Size = Border.Size - new Vector2(2, 2), LocalPosition = new Vector2(1, 1), Color = Color4.White };

            Frame.OnMouseEvent = new Func<MouseButtonEventArgs, bool>((a) =>
            {
                if (a.Button == MouseButton.Left && a.IsPressed)
                    Frame.Color = new Color4((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble(), 1);
                return true;
            });

            Border.AddChild(Frame);
            Frame.AddChild(Label);

            GUIManager.Root.AddChild(Border);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.ClearColor(Color.LightBlue);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GUIManager.Draw(new Vector2(0, 0));
            Context.SwapBuffers();
        }
    }
}
