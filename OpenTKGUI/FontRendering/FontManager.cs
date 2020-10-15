using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Numerics;

using OpenTK.Graphics.OpenGL4;

namespace OpenTKGUI.FontRendering
{
    //font bitmaps generated form https://www.angelcode.com/products/bmfont/
    internal static class FontManager
    {
        static Dictionary<string, Font> _fonts = new Dictionary<string, Font>();
        public static void Init(string fontFolder)
        {
            string[] files = Directory.GetFiles(fontFolder);

            foreach(string str in files)
            {
                if(Path.GetExtension(str) == ".fnt")
                {
                    XDocument document = XDocument.Load(str);
                    Font f = ParseFont(document, fontFolder);
                    _fonts.Add(f.Face + f.Size, f);
                }
            }
        }

        public static Font GetFont(string face, int size)
        {
            return _fonts[face + size];
        }

        private static Font ParseFont(XDocument xDocument, string fontFolder)
        {
            XElement info = xDocument.Descendants(XName.Get("info")).FirstOrDefault();
            XElement common = xDocument.Descendants(XName.Get("common")).FirstOrDefault();

            Font font = new Font();

            XElement page = xDocument.Descendants(XName.Get("page")).FirstOrDefault();
            font.FontBitmapData = new TextureData(fontFolder + "\\" + page.Attribute(XName.Get("file")).Value);
            font.FontBitmap = font.FontBitmapData.CreateTexture(TextureUnit.Texture0);
            font.FontBitmap.TextureMagFilter = TextureMagFilter.Nearest;
            font.FontBitmap.TextureMinFilter = TextureMinFilter.Nearest;

            font.Face = info.Attribute(XName.Get("face")).Value;
            font.Size = int.Parse(info.Attribute(XName.Get("size")).Value);

            string spacing = info.Attribute(XName.Get("spacing")).Value;
            font.Spacing = new Vector2(int.Parse(spacing.Split(',')[0]), int.Parse(spacing.Split(',')[1]));

            font.LineHeight = int.Parse(common.Attribute(XName.Get("lineHeight")).Value);
            font.Base = int.Parse(common.Attribute(XName.Get("base")).Value);
            font.FontBitmapWidth = int.Parse(common.Attribute(XName.Get("scaleW")).Value);
            font.FontBitmapHeight = int.Parse(common.Attribute(XName.Get("scaleH")).Value);

            IEnumerable<XElement> chars = xDocument.Descendants(XName.Get("char"));
            foreach(XElement element in chars)
            {
                Font.Character character = new Font.Character();

                character.Char = (char)int.Parse(element.Attribute(XName.Get("id")).Value);
                character.X = int.Parse(element.Attribute(XName.Get("x")).Value);
                character.Y = int.Parse(element.Attribute(XName.Get("y")).Value);
                character.Width = int.Parse(element.Attribute(XName.Get("width")).Value);
                character.Height = int.Parse(element.Attribute(XName.Get("height")).Value);
                character.XOffest = int.Parse(element.Attribute(XName.Get("xoffset")).Value);
                character.YOffset = int.Parse(element.Attribute(XName.Get("yoffset")).Value);
                character.XAdvance = int.Parse(element.Attribute(XName.Get("xadvance")).Value);

                character.Font = font;

                font.Characters.Add(character.Char, character);
            }

            IEnumerable<XElement> kernings = xDocument.Descendants(XName.Get("kerning"));
            foreach (XElement element in kernings)
            {
                Font.Kerning kerning = new Font.Kerning();

                kerning.First = (char)int.Parse(element.Attribute(XName.Get("first")).Value);
                kerning.Second = (char)int.Parse(element.Attribute(XName.Get("second")).Value);
                kerning.Amount = int.Parse(element.Attribute(XName.Get("amount")).Value);

                kerning.Font = font;

                font.Kernings.Add(Tuple.Create(kerning.First, kerning.Second), kerning);
            }
            return font;
        }
    }
}
