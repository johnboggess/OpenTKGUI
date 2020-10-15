using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;

using OpenTKGUI.Shaders;
namespace OpenTKGUI
{
    internal class Texture
    {
        public TextureUnit TextureUnit;
        public TextureWrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                _wrapMode = value;
                _textureData.Bind(TextureUnit);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wrapMode);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wrapMode);
                _textureData.UnBind();
            }
        }
        public TextureMinFilter TextureMinFilter
        {
            get { return _textureMinFilter; }
            set
            {
                _textureMinFilter = value;
                _textureData.Bind(TextureUnit);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_textureMinFilter);
                _textureData.UnBind();
            }
        }
        public TextureMagFilter TextureMagFilter
        {
            get { return _textureMagFilter; }
            set
            {
                _textureMagFilter = value;
                _textureData.Bind(TextureUnit);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
                _textureData.UnBind();
            }
        }

        TextureWrapMode _wrapMode = TextureWrapMode.Repeat;
        TextureMinFilter _textureMinFilter = TextureMinFilter.Linear;
        TextureMagFilter _textureMagFilter = TextureMagFilter.Linear;

        TextureData _textureData;

        public Texture(TextureData data, TextureUnit textureUnit)
        {
            _textureData = data;
            TextureUnit = textureUnit;

            _textureData.Bind(TextureUnit);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wrapMode);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
            _textureData.UnBind();
        }

        public void Bind(Shader shader, string uniformName)
        {
            shader.SetUniform1(uniformName, (int)TextureUnit - (int)TextureUnit.Texture0);
            _textureData.Bind(TextureUnit);
        }

        public void UnBind()
        {
            _textureData.UnBind();
        }
    }
}
