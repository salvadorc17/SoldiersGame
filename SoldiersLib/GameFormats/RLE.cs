using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    /*
    * WAD RLE File Format:
    *
    * Header:
    * +-------------+----+
    * | Name        | 32 | null terminated
    * | Size        |  8 |
    * | Unknown     |  8 | Starts with 03 00 00 00 // Use as RLE flag
    * | Height      |  4 |
    * | Width       |  4 |
    * | Color depth |  2 | 08 00
    * | Unkown      |  6 | 00 00 04 00 00 00
    * +-------------+----+
    *
    * Pixel Data:
    * Transparent pixel:      FF + Count, Example: FF 05 = 5 transparent pixels
    * Semi-Transparent pixel: FE + Count + Palette values
    *                Example: FE 04 01 02 03 04 = 4 Semi transparent pixels
    * Opaque pixels have no identifier. Just their count and palette value is used
     *               Example: 02 0A 0B = 2 pixels
    *
    * Line Offsets:
    * +------------------------+---+
    * | Size of following data | 4 | Including Name, Width, ...
    * | Name                   | 4 | First four letters of RLE name, lowercase
    * | Width                  | 4 | Same as in header
    * | Height                 | 4 | Same as in header
    * +------------------------+---+
    * Offset data: Data is 4 byte long.
    * Each line has its own value, so number of values is same as height.
    *
    * Color Palette Index:
    * 4 byte value containing the index of the color palette of the WAD file.
    * 1 = 00 00 00 01
    * Palette index starts with 0
    */
    public class RLE : WADImage
    {
        private UInt32[] _lineOffsets;



        private RLE()
        {

        }

        /// <summary>
        /// Read an RLE image from an array of data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <exception cref="InvalidOperationException">Thrown if data is invalid.</exception>
        public RLE(byte[] data, int index = 0)
        {
            // Header is 64 byte
            if (data.Length < (index + 64))
            {
                Trace.WriteLine("RLE: Data is not RLE Image. Not enough bytes to read header");
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
                Trace.WriteLine("RLE: Data size is too large to handle");
                throw new InvalidOperationException("Data size is too large to handle!");
            }
            int dataSize = (int)dataSize64;

            // Unsure if Flag is really used for file format
            // Using name instead
            // -----------------------------------------------
            // Check RLE Flag
            Name = Encoding.ASCII.GetString(data, index, 32).TrimEnd('\0');
            //if (data[start] != 3)
            if (!Name.EndsWith("RLE"))
            {
                Trace.WriteLine("RLE: " + Name + ": Not a RLE");
                throw new InvalidOperationException("" + Name + ": Not a RLE!");
            }
            // -----------------------------------------------

            // Check color depth
            start = index + 32 + 8 + 8 + 8;
            if (data[start] != 8 && data[start + 1] != 0)
            {
                Trace.WriteLine("RLE: Color depth flag is not set to 8");
                throw new InvalidOperationException("Color depth flag is not set to 8!");
            }

            // Full Data Size:
            // + 64 byte Header
            // + dataSize Pixel Data
            // + 4*4 Line Offsets Header
            // + Height*4 Line Offsets Data
            // + 4 Byte Color Palette ID

            start = index + 32 + 8 + 8;
            Height =
                (UInt32)data[start++] |
                (UInt32)data[start++] << 8 |
                (UInt32)data[start++] << 16 |
                (UInt32)data[start++] << 24;

            UInt64 requiredSize64 = 64 + dataSize64 + 4 * 4 + 4 * Height + 4;
            if (requiredSize64 > Int32.MaxValue)
            {
                Trace.WriteLine("RLE: " + Name + ": Data size is too large to handle");
                throw new InvalidOperationException(Name + ": Data size is too large to handle!");
            }
            int requiredSize = (int)requiredSize64;

            if (data.Length < (index + requiredSize))
            {
                Trace.WriteLine("RLE: " + Name + ": Data is not RLE Image. Not enough bytes for data");
                throw new InvalidOperationException(Name + ": Not enough bytes for data!");
            }
            // =====================================================================
            // RAW Data is available
            // =====================================================================
            _rawData = new byte[requiredSize];
            Array.Copy(data, index, _rawData, 0, requiredSize);
            start = 32 + 8 + 8 + 4;
            Width =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;
            //Name = Encoding.ASCII.GetString(_rawData, 0, 32).TrimEnd('\0');

            // Offset Data
            start = 64 + dataSize;
            UInt32 offsetDataSize =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;
            string offsetName = Encoding.ASCII.GetString(data, (int)start, 4).TrimEnd('\0');
            start += 4;
            UInt32 offsetWidth =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;
            UInt32 offsetHeight =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;

            if (offsetDataSize != Height * 4 + 12)
            {
                Trace.WriteLine("RLE: "+ Name +": Invalid offset data size (" + offsetDataSize + ", expected " + (Height * 4 + 12) + ")");
                //throw new InvalidOperationException("Invalid offset data size!");
            }
            if (offsetWidth != Width)
            {
                Trace.WriteLine("RLE: " + Name + ": Invalid offset width (" + offsetWidth + ", expected " + Width + ")");
                //throw new InvalidOperationException("Invalid offset width!");
            }
            if (offsetWidth != Height)
            {
                Trace.WriteLine("RLE: " + Name + ": Invalid offset height (" + offsetWidth + ", expected " + Height + ")");
                //throw new InvalidOperationException("Invalid offset height!");
            }
            _lineOffsets = new UInt32[Height];
            start = 64 + dataSize + 16;
            for (int i = 0; i < Height; ++i)
            {
                _lineOffsets[i] =
                    (UInt32)_rawData[start++] |
                    (UInt32)_rawData[start++] << 8 |
                    (UInt32)_rawData[start++] << 16 |
                    (UInt32)_rawData[start++] << 24;
                // Line offsets must be rising numbers
                if (i > 0 && _lineOffsets[i - 1] > _lineOffsets[i])
                {
                    Trace.WriteLine("RLE: " + Name + ": Offset data invalid");
                    throw new InvalidOperationException(Name + ": Offset data invalid!");
                }
            }
            ColorPaletteIndex =
                (UInt32)_rawData[start++] |
                (UInt32)_rawData[start++] << 8 |
                (UInt32)_rawData[start++] << 16 |
                (UInt32)_rawData[start++] << 24;

            // Read the pixels
            var pixels = new WADImagePixel[Width * Height];
            int currentPixel = 0;
            int max = 64 + dataSize;
            int pixelsToGo = 0;
            byte opacity = 0;
            for (int i = 64; i < max; ++i)
            {
                if (currentPixel >= pixels.Length) throw new InvalidOperationException("RLE: " + Name + ": There is too much pixel data.");

                if (_rawData[i] == 0xFF)
                {
                    // Add transparent pixels
                    opacity = 0;
                    if (i + 1 >= max) throw new InvalidOperationException("RLE: " + Name + ": End of data reached while parsing pixels.");
                    pixelsToGo = _rawData[++i];
                    for (int j = 0; j < pixelsToGo; ++j)
                    {
                        if (currentPixel >= pixels.Length) throw new InvalidOperationException("RLE: " + Name + ": There is too much pixel data.");
                        pixels[currentPixel++] = new WADImagePixel() { Opacity = opacity };
                    }
                }
                else if (_rawData[i] == 0xFE)
                {
                    opacity = 127;
                    if (i + 1 >= max) throw new InvalidOperationException("RLE: " + Name + ": End of data reached while parsing pixels.");
                    pixelsToGo = _rawData[++i];
                    for (int j = 0; j < pixelsToGo; ++j)
                    {
                        if (currentPixel >= pixels.Length) throw new InvalidOperationException("RLE: There is too much pixel data.");
                        if (i + 1 >= max) throw new InvalidOperationException("RLE: End of data reached while parsing pixels.");
                        pixels[currentPixel++] = new WADImagePixel() { Opacity = opacity, PaletteValue = _rawData[++i] };
                    }
                }
                else
                {
                    opacity = 255;
                    pixelsToGo = _rawData[i];
                    for (int j = 0; j < pixelsToGo; ++j)
                    {
                        if (currentPixel >= pixels.Length) throw new InvalidOperationException("RLE: " + Name + ": There is too much pixel data.");
                        if (i + 1 >= max) throw new InvalidOperationException("RLE: " + Name + ": End of data reached while parsing pixels.");
                        pixels[currentPixel++] = new WADImagePixel() { Opacity = opacity, PaletteValue = _rawData[++i] };
                    }
                }
            }

            Pixels = new WADImagePixel[Height, Width];
            currentPixel = 0;
            for (int h = 0; h < Height; ++h)
            {
                for (int w = 0; w < Width; ++w)
                    Pixels[h, w] = pixels[currentPixel++];
            }
        }
    }
}
