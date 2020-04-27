using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoldierTactics.GameFormats
{
    public class WADImagePixel
    {
        public byte Opacity; // 255 = opaque, 127 = semi transparent, 0 = transparent

        // Encoding of Color:
        // 2 byte (16 bit)
        // Byte 0: G2 G1 G0 B4 B3 B2 B1 B0
        // Byte 1: R4 R3 R2 R1 R0 G5 G4 G3

        public byte PaletteValue;
        public byte[] Color { get; private set; } // Ignored if Opacity is 0

        public WADImagePixel()
        {
            Color = new byte[2];
        }

        public override string ToString()
        {
            return "A:" + Opacity + ", R: " + R + ", G:" + G + ", B: " + B;
        }

        public byte R
        {
            get
            {
                if (Color == null || Color.Length == 0) return 0;
                int r = ((Color[1] & 0xF8) >> 3) & 0x1F; // This applies the bitmask. See above
                return (byte)(r * 255 / 32); // Scale to 255
            }
        }
        public byte G
        {
            get
            {
                if (Color == null || Color.Length == 0) return 0;
                int g =
                    (((Color[0] & 0xE0) >> 5) & 0x07) |
                    (((Color[1] & 0x07) << 3) & 0x38); // This applies the bitmask. See above
                return (byte)(g * 255 / 64); // Scale to 255
            }
        }

        public byte B
        {
            get
            {
                if (Color == null || Color.Length == 0) return 0;
                int b = Color[0] & 0x1F; // This applies the bitmask. See above
                return (byte)(b * 255 / 32); // Scale to 255
            }
        }
    }
}
