using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;

using OpenTKGUI.Shaders;
using OpenTKGUI.Buffers;
namespace OpenTKGUI.FontRendering
{
    internal class RChar
    {
        //public Transform Transform = new Transform();
        public Font.Character FontCharacter;
        public Color4 Color = Color4.Black;

        public Font Font { get { return FontCharacter.Font; } }
        public char Char { get { return FontCharacter.Char; } }
        public int XAdvance { get { return FontCharacter.XAdvance; } }
        public int XOffest { get { return FontCharacter.XOffest; } }
        public int YOffset { get { return FontCharacter.YOffset; } }
        public int Width { get { return FontCharacter.Width; } }
        public int Height { get { return FontCharacter.Height; } }

        public RChar(char c, Font font)
        {
            FontCharacter = font.Characters[c];
            //Transform.Scale = new Vector3(FontCharacter.Width, FontCharacter.Height, 1);
        }

        public void Draw(Shader shader, Matrix4 parentTransform, Matrix4 guiTransform)
        {
            shader.Use();
            shader.SetUniformMatrix("GUITransform", true, guiTransform);
            shader.SetUniformMatrix("ParentTransform", true, parentTransform);
            //shader.SetUniformMatrix("ModelTransform", true, Transform.GetMatrix());
            shader.SetUniform4("TextColor", new Vector4(Color.R, Color.G, Color.B, Color.A));
            shader.SetUniform1("uvs", FontCharacter.UVRegion());
            FontCharacter.Font.FontBitmap.Bind(shader, "tex");
            VertexArray.Square.Draw();
        }

        //public void Draw(Shader shader, Transform parent, Transform guiTransform)
        //{
            //Draw(shader, parent.GetMatrix(), guiTransform.GetMatrix());
        //}

    }
}
