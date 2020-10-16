using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using OpenTKGUI.Shaders;
using OpenTKGUI.GUIElements;
using OpenTKGUI;

namespace Test
{
    class Window : GameWindow
    {
        public Window(GameWindowSettings gws, NativeWindowSettings nws) : base (gws, nws)
        {
        }

        protected override void OnLoad()
        {
            GUIManager.Init(this);

            GUIElement last = GUIManager.Root;
            Random rng = new Random();
            for (int i = 0; i < 1; i++)
            {
                Image frame = new Image("Fonts\\Arial32_0.png");
                frame.LocalPosition = new Vector2(1, 1);
                frame.Size = new Vector2(100 - (2 * i), 100 - (2 * i));
                //frame.Color = new Color4((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble(), 1);
                last.AddChild(frame);
                last = frame;
            }

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
            GUIManager.Draw(Vector2.Zero);
            Context.SwapBuffers();
        }
    }
}
