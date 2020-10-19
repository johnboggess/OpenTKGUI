using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using OpenTKGUI.Buffers;
using OpenTKGUI.Shaders;

namespace OpenTKGUI.GUIElements
{
    public class Image : GUIElement
    {
        internal TextureData _TextureData;
        internal Texture _Texture;

        public Image(string imageFilePath)
        {
            _TextureData = TextureData.LoadTextureData(imageFilePath);
            _Texture = _TextureData.CreateTexture(TextureUnit.Texture0);
            Size = new Vector2(_Texture.Width, _Texture.Height);
        }

        public override void Draw(Vector2 parentGlobalPosition, int depth)
        {
            Shader.TexturedShader.Use();

            _Texture.Bind(Shader.TexturedShader, "tex");
            Shader.TexturedShader.SetUniformMatrix("globalGUITransform", false, GUIManager._Transform);
            Shader.TexturedShader.SetUniformMatrix("elementTransform", false, Transform * Matrix4.CreateTranslation(new Vector3(parentGlobalPosition.X, parentGlobalPosition.Y, depth)));

            VertexArray.Square.Draw();

            base.Draw(parentGlobalPosition, depth);
        }
    }
}
