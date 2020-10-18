using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTKGUI.Shaders;
using OpenTKGUI.Buffers;

namespace OpenTKGUI.GUIElements
{
    public class Frame : GUIElement
    {
        public Color4 Color = Color4.White;
        public Color4 BorderColor = Color4.Black;
        public float BorderSize = 0;

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            GUIManager._ColoredShader.Use();

            Vector2 oldPos = LocalPosition;
            Vector2 oldSize = Size;

            Size -= new Vector2(BorderSize * 2f, BorderSize * 2f);
            LocalPosition += new Vector2(BorderSize, BorderSize);
            GUIManager._ColoredShader.SetUniform4("Color", Color);
            GUIManager._ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            Size = oldSize;
            LocalPosition = oldPos;
            GUIManager._ColoredShader.SetUniform4("Color", BorderColor);
            GUIManager._ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            base.Draw(parentGlobalPosition, depth);
        }

    }
}
