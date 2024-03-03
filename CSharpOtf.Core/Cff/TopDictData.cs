using System;

namespace OpenGL.TextDrawing.Cff
{
    public class TopDictData
    {
        public SID? Version { get; set; } // 0 SID –, FontInfo
        public SID? Notice { get; set; } // 1 SID –, FontInfo
        public SID? Copyright { get; set; } // 12 0 SID –, FontInfo
        public SID? FullName { get; set; } // 2 SID –, FontInfo
        public SID? FamilyName { get; set; } // 3 SID –, FontInfo
        public SID? Weight { get; set; } // 4 SID –, FontInfo
        public bool IsFixedPitch { get; set; } // 12 1 boolean 0 (false), FontInfo
        public CffFloat ItalicAngle { get; set; } // 12 2 number 0, FontInfo
        public CffFloat UnderlinePosition { get; set; } = -100; // 12 3 number –100, FontInfo
        public CffFloat UnderlineThickness { get; set; } = 50; // 12 4 number 50, FontInfo
        public CffFloat PaintType { get; set; } // 12 5 number 0
        public CffFloat CharstringType { get; set; } = 2; // 12 6 number 2
        public CffFloat[] FontMatrix { get; set; } = new CffFloat[] { 0.001, 0, 0, 0.001, 0, 0 }; // 12 7 array 0.001 0 0 0.001 0 0
        public CffFloat? UniqueID { get; set; } // 13 number –
        public CffFloat[] FontBBox { get; set; } = new CffFloat[] { 0, 0, 0, 0 }; // 5 array 0 0 0 0
        public CffFloat StrokeWidth { get; set; } // 12 8 number 0
        public CffFloat[] XUID { get; set; } = Array.Empty<CffFloat>(); // 14 array –
        public CffInt Charset { get; set; } // 15 number 0, charset offset (0)
        public CffInt Encoding { get; set; } // 16 number 0, encoding offset (0)
        public CffInt? CharStrings { get; set; } // 17 number –, CharStrings offset  (0)
        public Private? Private { get; set; } // 18 number  number  –, Private DICT size  and offset (0)
        public CffInt? SyntheticBase { get; set; } // 12 20 number –, synthetic base font  index
        public SID? PostScript { get; set; } // 12 21 SID –, embedded  PostScript language  code
        public SID? BaseFontName { get; set; } // 12 22 SID –, (added as needed  by Adobe-based  technology)
        public CffInt[] BaseFontBlend { get; set; } = Array.Empty<CffInt>(); // 12 23 delta –, (added as needed  by Adobe-based  technology)
    }
}
