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
        public Color4 Color = Color4.Red;

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            GUIManager._ColoredShader.Use();

            GUIManager._ColoredShader.SetUniform4("Color", Color);
            GUIManager._ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.TextSquare.Draw();

            base.Draw(parentGlobalPosition, depth);
        }

    }
}
