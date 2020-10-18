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
        TextBox TextBox;
        Slider Slider;
        Label sldrLbl;
        OpenTKGUI.GUIElements.Image img;
        public Window(GameWindowSettings gws, NativeWindowSettings nws) : base (gws, nws)
        {
        }

        protected override void OnLoad()
        {
            Random rng = new Random();

            GUIManager.Init(this);
            FontManager.Init("Fonts");

            Label = new Label("The quick brown fox jumps over the lazy dog WA fj", FontManager.GetFont("Arial", 32)) { LocalPosition = new Vector2(5, 2) };
            Border = new Frame() { Size = new Vector2(Label.Size.X + 10, 40), Color = Color4.Black };
            Border.LocalPosition = new Vector2(Size.X / 2 - Border.Size.X / 2, Size.Y / 2 - Border.Size.Y / 2);
            Frame = new Frame() { Size = Border.Size - new Vector2(2, 2), LocalPosition = new Vector2(1, 1), Color = Color4.White };

            TextBox = new TextBox(FontManager.GetFont("Arial", 32));
            TextBox.Size = new Vector2(500, 40);
            TextBox.Text = "Text box";

            Slider = new Slider();
            Slider.Size = new Vector2(200, 40);
            Slider.LocalPosition = new Vector2(0, 100);
            Slider.OnValueChanged = new Action<float, float>((oldV, newV) =>
            {
                sldrLbl.Text = newV.ToString();
            });

            sldrLbl = new Label(Slider.Value.ToString(), FontManager.GetFont("Arial", 32));
            sldrLbl.Size = new Vector2(200, 40);
            sldrLbl.LocalPosition = new Vector2(0, 140);

            img = new OpenTKGUI.GUIElements.Image("Fonts\\Arial32_0.png");
            img.LocalPosition = new Vector2(400, 32);

            Frame.OnMouseButton = new Func<MouseButtonEventArgs, bool>((a) =>
            {
                if (a.Button == MouseButton.Left && a.IsPressed)
                    Frame.Color = new Color4((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble(), 1);
                return true;
            });


            Border.AddChild(Frame);
            Frame.AddChild(Label);

            GUIManager.Root.AddChild(Border);
            GUIManager.Root.AddChild(TextBox);
            GUIManager.Root.AddChild(Slider);
            GUIManager.Root.AddChild(sldrLbl);
            GUIManager.Root.AddChild(img);

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
