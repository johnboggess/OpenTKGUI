﻿using System;
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
using OpenTKGUI.Enums;
using OpenTKGUI;
using System.ComponentModel.Design;

namespace Test
{
    class Window : GameWindow
    {
        Frame Frame;
        public Window(GameWindowSettings gws, NativeWindowSettings nws) : base (gws, nws)
        {
        }

        protected override void OnLoad()
        {
            Random rng = new Random();

            GUIManager.Init(this);
            FontManager.Init("Fonts");

            Frame = new Frame();
            Frame.Size = new Vector2(200, 200);
            Frame.BorderSize = 2;
            Frame.BorderColor = Color4.Black;
            Frame.Color = Color4.Turquoise;
            Frame.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Left;
            Frame.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Center;
            GUIManager.Root.AddChild(Frame);

            Frame frame = new Frame();
            frame.Color = Color4.Purple;
            frame.Size = new Vector2(10, 10);
            frame.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Stretch;
            frame.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Center;
            frame.BorderSize = 1;
            frame.BorderColor = Color.Red;

            OpenTKGUI.GUIElements.Image image = new OpenTKGUI.GUIElements.Image("Fonts\\Arial32_0.png");
            image.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Stretch;
            image.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Stretch;

            Label label = new Label("The quick brown fox jumps over the lazy dog WA fj", FontManager.GetFont("Arial", 32));
            label.Size = new Vector2(-1, -1);
            label.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Center;
            label.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Center;

            TextBox textBox = new TextBox(FontManager.GetFont("Arial", 32));
            textBox.MinWidth = 32;
            textBox.BorderSize = 1;
            textBox.Size = new Vector2(-1, 32);
            textBox.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Center;
            textBox.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Center;

            Slider slider = new Slider();
            slider.Size = new Vector2(100, 32);

            Button btn = new Button("Button Test", FontManager.GetFont("Arial", 32));
            btn.Size = new Vector2(150, 50);
            btn.BorderSize = 1;
            btn.BorderColor = Color4.Black;
            btn.OnMouseButton = new Func<MouseButtonEventArgs, bool>((a) =>
            {
                if(a.IsPressed && a.Button == MouseButton.Left)
                    Console.WriteLine("Clicked");
                btn.OnMouseButtonAdjustColor(a);
                return false;
            });

            HorizontalGrid horizontalGrid = new HorizontalGrid();
            horizontalGrid.Size = new Vector2(-1, 100);
            horizontalGrid.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Left;
            horizontalGrid.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Top;
            //horizontalGrid.AddChild(frame, HorizontalGrid.Units.Ratio, 1);
            //horizontalGrid.AddChild(textBox, HorizontalGrid.Units.Ratio, 2);

            VerticalGrid verticalGrid = new VerticalGrid();
            verticalGrid.Size = new Vector2(-1, 100);
            verticalGrid.HorizontalAlignment = OpenTKGUI.Enums.HorizontalAlignment.Left;
            verticalGrid.VerticalAlignment = OpenTKGUI.Enums.VerticalAlignment.Top;
            //verticalGrid.AddChild(frame, Units.Ratio, 1);
            //verticalGrid.AddChild(textBox, Units.Ratio, 2);


            Frame.AddChild(btn);


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
