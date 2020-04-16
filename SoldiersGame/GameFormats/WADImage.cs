using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    public abstract class WADImage
    {
        //string _parent;
        //public string Parent
        //{
        //    get => _parent;
        //    set
        //    {
        //        if (_parent == value) return;
        //        _parent = value;
        //        _parentIndex = -1;
        //        if (_parent?.EndsWith(".wad", StringComparison.InvariantCultureIgnoreCase) == true)
        //        {
        //            var sub = _parent.Substring(0, _parent.Length - 4);
        //            int firstNumIndex = -1;
        //            for (int i = _parent.Length - 1; i >= 0; --i)
        //            {

        //            }
        //        }
        //    }
        //}
        //int _parentIndex;
        //public int ParentIndex
        //{ get; }

        public string Name { get; set; }
        public UInt32 Width { get; protected set; }
        public UInt32 Height { get; protected set; }
        public WADImagePixel[,] Pixels { get; protected set; }
        protected byte[] _rawData;
        public int RawDataSize
        {
            get
            {
                if (_rawData == null) return 0;
                return _rawData.Length;
            }
        }

        public byte[] GetHeader()
        {
            if (_rawData == null || _rawData.Length < 64) return null;
            var b = new byte[64];
            Array.Copy(_rawData, 0, b, 0, 64);
            return b;
        }

        public UInt32 ColorPaletteIndex { get; protected set; }
        public byte[] Palette { get; set; }
        public void ApplyPalette()
        {
            if (Pixels == null || Palette == null) return;
            for (int h = 0; h < Height; ++h)
            {
                for (int w = 0; w < Width; ++w)
                {
                    var pix = Pixels[h, w];
                    pix.Color[0] = Palette[pix.PaletteValue * 2];
                    pix.Color[1] = Palette[pix.PaletteValue * 2 + 1];
                }
            }
        }
        public List<WADImagePixel> GetPaletteColors()
        {
            if (Palette == null || Palette.Length < 512) return null;
            var list = new List<WADImagePixel>();
            for (int i = 0; i < 512; i += 2)
            {
                var pix = new WADImagePixel();
                pix.Color[0] = Palette[i];
                pix.Color[1] = Palette[i+1];
                list.Add(pix);
            }
            return list;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "<not set>" : Name;
        }
    }
}
