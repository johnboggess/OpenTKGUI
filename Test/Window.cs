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
        //Label Label;
        Frame Frame;
        //TextBox TextBox;
        //Slider Slider;
        //Label sldrLbl;
        //OpenTKGUI.GUIElements.Image img;
        public Window(GameWindowSettings gws, NativeWindowSettings nws) : base (gws, nws)
        {
        }

        protected override void OnLoad()
        {
            Random rng = new Random();

            GUIManager.Init(this);
            FontManager.Init("Fonts");

            Frame = new Frame();
            Frame.Size = new Vector2(300, 100);
            Frame.BorderSize = 2;
            Frame.BorderColor = Color4.Black;
            Frame.Color = Color4.LightGray;
            Frame.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Left;
            Frame.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Top;
            GUIManager.Root.AddChild(Frame);

            Frame frame = new Frame();
            frame.Color = Color4.Purple;
            frame.Size = new Vector2(-1, -1);
            frame.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Center;
            frame.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Center;
            frame.BorderSize = 1;
            frame.BorderColor = Color.Red;

            OpenTKGUI.GUIElements.Image image = new OpenTKGUI.GUIElements.Image("Fonts\\Arial32_0.png");

            Label label = new Label("The quick brown fox jumps over the lazy dog WA fj", FontManager.GetFont("Arial", 32));
            label.Size = new Vector2(-1, -1);

            TextBox textBox = new TextBox(FontManager.GetFont("Arial", 32));
            textBox.MinWidth = 32;
            textBox.Size = new Vector2(-1, 32);

            Slider slider = new Slider();
            slider.Size = new Vector2(100, 32);
            
            Frame.AddChild(slider);

            /*Label = new Label("The quick brown fox jumps over the lazy dog WA fj", FontManager.GetFont("Arial", 32)) { LocalPosition = new Vector2(5, 2) };
            Frame = new Frame();
            Frame.LocalPosition = new Vector2(Size.X / 2f - Label.Size.X / 2f, Size.Y/2f);
            Frame.Size = new Vector2(Label.Size.X + 10, Label.Size.Y);
            Frame.BorderSize = 1;

            TextBox = new TextBox(FontManager.GetFont("Arial", 32));
            TextBox.Size = new Vector2(500, 40);
            TextBox.Text = "Text box";
            TextBox.BorderSize = 1;

            Slider = new Slider();
            Slider.Size = new Vector2(200, 40);
            Slider.LocalPosition = new Vector2(0, 100);
            Slider.OnValueChanged = new Action<float, float>((oldV, newV) =>
            {
                sldrLbl.Text = newV.ToString();
            });
            Slider.BorderSize = 1;

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


            Frame.AddChild(Label);

            GUIManager.Root.AddChild(Frame);
            GUIManager.Root.AddChild(TextBox);
            GUIManager.Root.AddChild(Slider);
            GUIManager.Root.AddChild(sldrLbl);
            GUIManager.Root.AddChild(img);*/

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
