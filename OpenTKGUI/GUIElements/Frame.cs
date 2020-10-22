using System;
using System.Collections.Generic;
using System.Text;

using OpenTKGUI.Shaders;
using OpenTKGUI.Buffers;

using OpenTK.Mathematics;

namespace OpenTKGUI.GUIElements
{
    public class Frame : GUIContainer
    {
        public override void AddChild(GUIElement element, params object[] args)
        {
            if (_Children.Count > 0)
                throw new Exception("Frame can only have one child");
            base.AddChild(element);
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            Shader.ColoredShader.Use();

            Vector2 oldPos = _LocalPosition;
            Vector2 oldSize = RenderSize;

            RenderSize -= new Vector2(BorderSize * 2f, BorderSize * 2f);
            _LocalPosition += new Vector2(BorderSize, BorderSize);
            Shader.ColoredShader.SetUniform4("Color", Color);
            Shader.ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            Shader.ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            RenderSize = oldSize;
            _LocalPosition = oldPos;
            Shader.ColoredShader.SetUniform4("Color", BorderColor);
            Shader.ColoredShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            Shader.ColoredShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            base.Draw(parentGlobalPosition, depth);
        }
    }
}
