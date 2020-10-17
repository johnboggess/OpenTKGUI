﻿using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Mathematics;

using OpenTKGUI.Shaders;
using OpenTKGUI.Buffers;
using OpenTKGUI.GUIElements;
namespace OpenTKGUI.FontRendering
{
    internal class RChar : GUIElement
    {
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
            Size = new Vector2(FontCharacter.Width, FontCharacter.Height);
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            GUIManager._FontShader.Use();

            Font._FontBitmap.Bind(GUIManager._FontShader, "tex");
            GUIManager._FontShader.SetUniform4("TextColor", Color);
            GUIManager._FontShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            GUIManager._FontShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));
            GUIManager._FontShader.SetUniform1("uvs", FontCharacter.UVRegion());

            VertexArray.Square.Draw();

            base.Draw(parentGlobalPosition, depth);
        }
    }
}
