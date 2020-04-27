using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    public class WAD
    {

        private List<WADImage> images;

        public List<WADImage> Images
        {
            get
            {
                if (images == null) return null;
                return images;
            }
        }

        public int Files
        {
            get
            {
                if (images == null) return 0;
                return images.Count;
            }
        }

        public string Name;

        public string Folder;

        public int PaletteSize = 512 + 13;
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


        public WAD(string path)
        {
            Folder = path;

            Name = Path.GetFileName(path);

            if (Folder != null)
                Extract();


        }


        public void Extract()
        {
            images = new List<WADImage>();
            if (!File.Exists(Folder))
            {
                Trace.WriteLine("WAD: The file " + Folder + " does not exist!");
                
            }
            var fs = new FileStream(Folder, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            var bytes = br.ReadBytes((int)(new FileInfo(Folder).Length));
            br.Close();
            fs.Close();
            if (bytes.Length < 400 + 4 + 512 + 13 + 4)
            {
                Trace.WriteLine("WAD: File too small!");
                
            }
            Trace.WriteLine("WAD: Reading file '" + Folder + "'");
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
                        bmp.Name = name;
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
                        
                    }
                }
                else if (name.EndsWith("RLE"))
                {
                    RLE rle = null;
                    try
                    {
                        rle = new RLE(bytes, index);
                        rle.Name = name;
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
                        
                    }
                }
                else
                {
                    Trace.WriteLine("WAD: Error filename does not end with BMP or RLE: " + name);
                    
                }
            }
            Trace.WriteLine("WAD Extract() success!");
            
        }
    }
}
