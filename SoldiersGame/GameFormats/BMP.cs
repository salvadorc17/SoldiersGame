using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    /* WAD BMP File Format:
    * 
    * Header: (64 byte)
    * +-------------+----+
    * | Name        | 32 | null terminated    
    * | Size        |  8 | Height * (Width+2) + 1
    * | Unknown     |  8 | 00 00 00 00 00 00 00 00 // Use as flag
    * | Height      |  4 |
    * | Width       |  4 | 
    * | Color depth |  2 | 08 00
    * | Unkown      |  6 | 00 00 00 00 00 00 
    * +-------------+----+
    * 
    * Pixel Data:
    * Every byte is one pixel.
    * The first two pixels of each line must be repeated at the end of the line
    * B1 B2 B3 B4 B5 B6 B1 B2  <- First line of image (6 pixel width)
    * B7 B8 ........... B7 B8  <- Second line of image
    * B3 <- Last byte is 3rd byte of first line
    * 
    * Color Palette ID:
    * 4 byte value containing the index of the color palette of the WAD file.
    * 1 = 00 00 00 01
    * Palette index starts with 0
    */

    /// <summary>
    /// Represents a bmp in a WAD file
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if data is invalid.</exception>
    public class BMP : WADImage
    {
        public BMP(byte[] data, int index = 0)
        {
            // Header is 64 byte
            if (data.Length < (index + 64))
            {
                Trace.WriteLine("BMP: Data is not BMP Image. Not enough bytes to read header");
                throw new InvalidOperationException("Not enough bytes to read header!");
            }

            int start = index + 32;
            UInt64 dataSize64 =
                (UInt64)data[start++] |
                (UInt64)data[start++] << 8 |
                (UInt64)data[start++] << 16 |
                (UInt64)data[start++] << 24 |
                (UInt64)data[start++] << 32 |
                (UInt64)data[start++] << 40 |
                (UInt64)data[start++] << 48 |
                (UInt64)data[start++] << 56;
            if (dataSize64 > Int32.MaxValue)
            {
                Trace.WriteLine("BMP: Data size is too large to handle");
                throw new InvalidOperationException("Data size is too large to handle!");
            }
            int dataSize = (int)dataSize64;

            // Unsure if Flag is really used for file format
            // Using name instead
            // -----------------------------------------------
            // Check BMP Flag            
            Name = Encoding.ASCII.GetString(data, index, 32).TrimEnd('\0');
            //if (data[start] == 0x03) // !!! TODO danger !!! Should be 0
            if (!Name.EndsWith("BMP"))
            {
                Trace.WriteLine("BMP: Not a BMP");
                throw new InvalidOperationException("Not a BMP!");
            }
            // -----------------------------------------------

            // Check color depth
            start = index + 32 + 8 + 8 + 8;
            if (data[start] != 8 && data[start + 1] != 0)
            {
                Trace.WriteLine("BMP: " + Name + ": Color depth flag is not set to 8");
                throw new InvalidOperationException(Name + ": Color depth flag is not set to 8!");
            }

            // Full Data Size:
            // + 64 byte Header
            // + dataSize Pixel Data            
            // + 4 Byte Color Palette ID

            UInt64 requiredSize64 = 64 + dataSize64 + 4;
            if (requiredSize64 > Int32.MaxValue)
            {
                Trace.WriteLine("BMP: " + Name + ": Data size is too large to handle");
                throw new InvalidOperationException(Name + ": Data size is too large to handle!");
            }
            int requiredSize = (int)requiredSize64;

            if (data.Length < (index + requiredSize))
            {
                Trace.WriteLine("BMP: " + Name + ": Data is not BMP Image. Not enough bytes for data");
                throw new InvalidOperationException(Name + ": Not enough bytes for data!");
            }
            // =====================================================================
            // RAW Data is available
            // =====================================================================
            _rawData = new byte[requiredSize];
            Array.Copy(data, index, _rawData, 0, requiredSize);

            start = 32 + 8 + 8;
            Height =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;
            Width =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;            

            start = 64 + dataSize;
            ColorPaletteIndex =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;

            // Read the pixels   

            // Check if last byte of data and third byte are the same
            if (_rawData[64 + 3 - 1] != _rawData[64 + dataSize - 1])
            {
                throw new InvalidOperationException("BMP: " + Name + ": Pixel data invalid. The third byte is not repeated at the end.");
            }

            //for (int i = 64; i < 64 + dataSize; ++i)
            //{
            //    if ((i - 64) % (Width + 2) == 0) Debug.Write("\n");
            //    Debug.Write(string.Format("{0:X2} ", _rawData[i]));
            //}
            //Debug.Write("\n");

            var pixels = new WADImagePixel[Width * Height];
            int max = 64 + dataSize;
            int currentPixel = 0;
            for (int i = 64; i < max - 1; ++i) // last value was already checked
            {
                int lineIndex = (i - 64) % ((int)Width + 2);
                if (i > 64 && lineIndex == Width)
                {
                    // Reached end of line
                    // The first two bytes will be repeated
                    if (_rawData[i] != _rawData[i - Width])
                        throw new InvalidOperationException("BMP: " + Name + ": Pixel data invalid. First byte of line not repeated at the end.");
                    ++i;
                    if (_rawData[i] != _rawData[i - Width])
                        throw new InvalidOperationException("BMP: " + Name + ": Pixel data invalid. Second byte of line not repeated at the end.");
                    ++i;
                }
                if (i < max - 1 && currentPixel >= pixels.Length) throw new InvalidOperationException("BMP: There is too much pixel data.");
                if (i >= max - 1) break;
                pixels[currentPixel++] = new WADImagePixel() { Opacity = 255, PaletteValue = _rawData[i] };
            }

            Pixels = new WADImagePixel[Height, Width];
            currentPixel = 0;
            for (int h = 0; h < Height; ++h)
            {
                for (int w = 0; w < Width; ++w)
                    Pixels[h, w] = pixels[currentPixel++];
            }
        }

        public static bool IsBMP(byte[] data, int index = 0)
        {
            if (data.Length - index < 64)
            {
                Trace.WriteLine("BMP: Data is not BMP. Not enough bytes to read header");
                return false;
            }
            return data[index + 32 + 8] == 0;
        }

        public static BMP GetFromBytes(byte[] data, int index = 0)
        {
            if (!IsBMP(data, index)) return null;

            string name = Encoding.ASCII.GetString(data, index, 32);

            BMP bmp = null;
            try
            {
                bmp = new BMP(data, index);
            }
            catch { }
            return bmp;
        }
    }
}
