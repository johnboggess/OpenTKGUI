using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Numerics;

namespace OpenTKGUI.FontRendering
{
    internal class Font
    {
        public string Face;
        public int Size;
        public Vector2 Spacing;
        public int LineHeight;
        public int Base;
        public int FontBitmapWidth;
        public int FontBitmapHeight;
        public TextureData FontBitmapData;
        public Texture FontBitmap;

        public Dictionary<char, Character> Characters = new Dictionary<char, Character>();
        public Dictionary<Tuple<char, char>, Kerning> Kernings = new Dictionary<Tuple<char, char>, Kerning>();


        public class Character
        {
            public char Char;
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public int XOffest;
            public int YOffset;
            public int XAdvance;
            public Font Font;

            public float[] UVRegion()
            {
                float BLx = X;
                float BLy = Y + Height;
                BLy = Font.FontBitmapHeight - BLy;

                float TRx = BLx + Width;
                float TRy = BLy + Height;

                return new float[] { BLx / (float)Font.FontBitmapWidth, BLy / (float)Font.FontBitmapHeight, TRx / (float)Font.FontBitmapWidth, TRy / (float)Font.FontBitmapHeight };
            }
        }

        public class Kerning
        {
            public char First;
            public char Second;
            public int Amount;
            public Font Font;
        }

        public bool KerningExists(char first, char second)
        {
            return Kernings.ContainsKey(Tuple.Create(first, second));
        }

        public int GetKerning(char first, char second)
        {
            if(KerningExists(first, second))
                return Kernings[Tuple.Create(first, second)].Amount;
            return 0;
        }
    }
}
