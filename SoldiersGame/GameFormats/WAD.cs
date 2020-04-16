using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    public static class WAD
    {
        const int PaletteSize = 512 + 13;
        /* WAD File format:
         *   Name          Bytes  Index
         * +--------------+-----+
         * | Header       | 400 |   0
         * | #ColPalettes |   4 | 400
         * | Col Palette  | 512 | 404 |\
         * | Unknown      |  13 |     |/ This block is repeated for each palette
         * | #Images      |   4 | 404 + (#ColPalettes*525)
         * | Images       |     |
         * +--------------+-----+
         */

        public static List<WADImage> Extract(string wadFile)
        {
            var images = new List<WADImage>();
            if (!File.Exists(wadFile))
            {
                Trace.WriteLine("WAD: The file " + wadFile + " does not exist!");
                return images;
            }
            var fs = new FileStream(wadFile, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            var bytes = br.ReadBytes((int)(new FileInfo(wadFile).Length));
            br.Close();
            fs.Close();
            if (bytes.Length < 400 + 4 + 512 + 13 + 4)
            {
                Trace.WriteLine("WAD: File too small!");
                return images;
            }
            Trace.WriteLine("WAD: Reading file '" + wadFile + "'");
            // Read color palette count:
            int paletteCnt = (bytes[400] | bytes[401] << 8 | bytes[402] << 16 | bytes[403] << 24);
            Trace.WriteLine("WAD: Palette count: " + paletteCnt);
            byte[][] palettes = new byte[paletteCnt][];
            // Read color palettes:
            int index = 404;
            for (int i = 0; i < paletteCnt; ++i)
            {
                var array = new byte[PaletteSize];
                Array.Copy(bytes, index + i * PaletteSize, array, 0, PaletteSize);
                palettes[i] = array;
            }
            // Read image count
            index = 404 + paletteCnt * PaletteSize;
            int imageCnt =
                bytes[index++] |
                bytes[index++] << 8 |
                bytes[index++] << 16 |
                bytes[index++] << 24;
            Trace.WriteLine("WAD: image count: " + imageCnt);

            // Now come BMP or RLE files
            while (index < bytes.Length)
            {
                // Get Filename:
                var name = Encoding.ASCII.GetString(bytes, index, 32).TrimEnd('\0');
                if (name.EndsWith("BMP"))
                {
                    BMP bmp = null;
                    try
                    {
                        bmp = new BMP(bytes, index);
                        if (bmp.ColorPaletteIndex < palettes.Length)
                        {
                            bmp.Palette = palettes[bmp.ColorPaletteIndex];
                            bmp.ApplyPalette();
                        }
                        else
                            Trace.WriteLine("WAD: Warning! BMP image has invalid palette index");
                        images.Add(bmp);
                        index += bmp.RawDataSize;
                        continue;
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("WAD: Error reading BMP: " + name + ", " + e);
                        return images;
                    }
                }
                else if (name.EndsWith("RLE"))
                {
                    RLE rle = null;
                    try
                    {
                        rle = new RLE(bytes, index);
                        if (rle.ColorPaletteIndex < palettes.Length)
                        {
                            rle.Palette = palettes[rle.ColorPaletteIndex];
                            rle.ApplyPalette();
                        }
                        else
                            Trace.WriteLine("WAD: Warning! RLE image has invalid palette index");
                        images.Add(rle);
                        index += rle.RawDataSize;
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("WAD: Error while reading RLE: " + name + ", " + e);
                        return images;
                    }
                }
                else
                {
                    Trace.WriteLine("WAD: Error filename does not end with BMP or RLE: " + name);
                    return images;
                }
            }
            Trace.WriteLine("WAD Extract() success!");
            return images;
        }
    }
}
