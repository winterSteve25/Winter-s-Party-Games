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
        private static Dictionary<Color, ColorPalette> _mapperInversed;

        static ColorPaletteExtension()
        {
            _mapper = new Dictionary<ColorPalette, Color>
            {
                { ColorPalette.White, Color.white },
                { ColorPalette.Black, Color.black },
                { ColorPalette.LightGray, new Color(60, 60, 60) },
                { ColorPalette.DarkGray, new Color(25, 25, 25) },
                { ColorPalette.Red, Color.red },
                { ColorPalette.Orange, new Color(250, 149, 25) },
                { ColorPalette.Yellow, Color.yellow },
                { ColorPalette.Blue, Color.blue },
                { ColorPalette.Green, Color.green },
                { ColorPalette.Lime, new Color(147, 255, 89) },
                { ColorPalette.Cyan, new Color(15, 255, 227) },
                { ColorPalette.Sky, new Color(71, 231, 255) },
                { ColorPalette.Magenta, new Color(237, 28, 181) },
                { ColorPalette.Brown, new Color(64, 34, 3) },
                { ColorPalette.Pink, new Color(223, 13, 255) },
                { ColorPalette.Purple, new Color(98, 2, 214) }
            };

            _mapperInversed = new Dictionary<Color, ColorPalette>
            {
                { Color.white, ColorPalette.White },
                { Color.black, ColorPalette.Black },
                { new Color(60, 60, 60), ColorPalette.LightGray },
                { new Color(25, 25, 25), ColorPalette.DarkGray },
                { Color.red, ColorPalette.Red },
                { new Color(250, 149, 25), ColorPalette.Orange },
                { Color.yellow, ColorPalette.Yellow },
                { Color.blue, ColorPalette.Blue },
                { Color.green, ColorPalette.Green },
                { new Color(147, 255, 89), ColorPalette.Lime },
                { new Color(15, 255, 227), ColorPalette.Cyan },
                { new Color(71, 231, 255), ColorPalette.Sky },
                { new Color(237, 28, 181), ColorPalette.Magenta },
                { new Color(64, 34, 3), ColorPalette.Brown },
                { new Color(223, 13, 255), ColorPalette.Pink },
                { new Color(98, 2, 214), ColorPalette.Purple }
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
            return (byte)_mapperInversed[color];
        }
    }
}