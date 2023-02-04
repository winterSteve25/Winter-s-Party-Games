using System;
using System.IO;
using System.IO.Compression;
using Games.Utils.Paint;
using UnityEngine;

namespace Utils.Data
{
    public static class DataUtilities
    {
        public static byte[] Compress(byte[] data)
        {
            byte[] compressArray = null;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                    {
                        deflateStream.Write(data, 0, data.Length);
                    }

                    compressArray = memoryStream.ToArray();
                }
            }
            catch (Exception exception)
            {
                // ignored
            }

            return compressArray;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] decompressedArray = null;
            try
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (MemoryStream compressStream = new MemoryStream(data))
                    {
                        using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
                        {
                            deflateStream.CopyTo(decompressedStream);
                        }
                    }
                    decompressedArray = decompressedStream.ToArray();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return decompressedArray;
        }
        
        private static Color DeserializeColor(byte data)
        {
            return ColorPaletteExtension.GetColorFromCode(data);
        }

        private static byte SerializeColor(Color color)
        {
            return ColorPaletteExtension.GetCodeFromColor(color);
        }

        public static byte[] SerializeTexture2D(Texture2D texture2D)
        {
            var colors = texture2D.GetPixels();
            var bytes = new byte[colors.Length];
            
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = SerializeColor(colors[i]);
            }
            
            var compressed = Compress(bytes);

            return compressed;
        }

        public static Color[] DeserializeTexture2DPixels(byte[] pixels)
        {
            var data = Decompress(pixels);

            var colors = new Color[data.Length];

            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = DeserializeColor(data[i]);
            }

            return colors;
        }
    }
}