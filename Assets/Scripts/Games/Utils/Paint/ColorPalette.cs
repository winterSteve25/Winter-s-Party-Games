using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.Utils.Paint
{
    public enum ColorPalette
    {
        White = 0,
        Black = 1,
        LightGray = 2,
        DarkGray = 3,
        Red = 4,
        Orange = 5,
        Yellow = 6,
        Blue = 7,
        Green = 8,
        Lime = 9,
        Cyan = 10,
        Sky = 11,
        Magenta = 12,
        Brown = 13,
        Pink = 14,
        Purple = 15
    }

    public static class ColorPaletteExtension
    {
        private static Dictionary<ColorPalette, Color> _mapper;

        static ColorPaletteExtension()
        {
            _mapper = new Dictionary<ColorPalette, Color>
            {
                { ColorPalette.White, Color.white },
                { ColorPalette.Black, Color.black },
                { ColorPalette.LightGray, ConvertFrom255(60, 60, 60) },
                { ColorPalette.DarkGray, ConvertFrom255(25, 25, 25) },
                { ColorPalette.Red, Color.red },
                { ColorPalette.Orange, ConvertFrom255(250, 149, 25) },
                { ColorPalette.Yellow, Color.yellow },
                { ColorPalette.Blue, Color.blue },
                { ColorPalette.Green, Color.green },
                { ColorPalette.Lime, ConvertFrom255(147, 255, 89) },
                { ColorPalette.Cyan, ConvertFrom255(15, 255, 227) },
                { ColorPalette.Sky, ConvertFrom255(71, 231, 255) },
                { ColorPalette.Magenta, ConvertFrom255(237, 28, 181) },
                { ColorPalette.Brown, ConvertFrom255(64, 34, 3) },
                { ColorPalette.Pink, ConvertFrom255(223, 13, 255) },
                { ColorPalette.Purple, ConvertFrom255(98, 2, 214) }
            };
        }

        public static Color GetColorFromCode(byte code)
        {
            return _mapper[(ColorPalette)code];
        }

        public static Color GetColorFromCode(ColorPalette code)
        {
            return _mapper[code];
        }

        public static byte GetCodeFromColor(Color color)
        {
            return (byte) _mapper.First(kv => CompareColors(kv.Value, color)).Key;
        }

        private static Color ConvertFrom255(float r, float g, float b, float a = 255)
        {
            return new Color(r/255, g/255, b/255, a/255);
        }

        private static bool CompareColors(Color a, Color b)
        {
            if (!(Math.Abs(a.r - b.r) < 0.2)) return false;
            if (!(Math.Abs(a.g - b.g) < 0.2)) return false;
            return Math.Abs(a.b - b.b) < 0.2;
        }
    }
}