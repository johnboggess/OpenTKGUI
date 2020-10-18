using SixLabors;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL4;
namespace OpenTKGUI
{
    internal class TextureData
    {
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        int _handle;
        private bool _disposed = false;
        private int _width;
        private int _height;

        private static Dictionary<string, TextureData> _loadedTextures = new Dictionary<string, TextureData>();

        public static TextureData LoadTextureData(string filePath, FlipMode flipMode = FlipMode.Vertical)
        {
            if (_loadedTextures.ContainsKey(filePath))
                return _loadedTextures[filePath];

            TextureData textureData = new TextureData();
            textureData._handle = GL.GenTexture();

            SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);
            image.Mutate(x => x.Flip(flipMode));
            List<byte> pixels = new List<byte>();
            for (int r = 0; r < image.Height; r++)
            {
                Span<Rgba32> span = image.GetPixelRowSpan(r);
                for (int c = 0; c < image.Width; c++)
                {
                    pixels.Add(span[c].R);
                    pixels.Add(span[c].G);
                    pixels.Add(span[c].B);
                    pixels.Add(span[c].A);
                }
            }

            textureData._width = image.Width;
            textureData._height = image.Height;

            GL.BindTexture(TextureTarget.Texture2D, textureData._handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.SrgbAlpha, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            textureData.UnBind();

            _loadedTextures.Add(filePath, textureData);

            return textureData;
        }

        public Texture CreateTexture(TextureUnit textureUnit)
        {
            return new Texture(this, textureUnit);
        }

        public void Bind(TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void UnBind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteTexture(_handle);
        }
    }
}
