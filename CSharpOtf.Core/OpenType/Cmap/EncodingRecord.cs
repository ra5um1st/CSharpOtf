using System;
using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.OpenType
{
    public struct EncodingRecord
    {
        public ushort PlatformID { get; set; }
        public ushort EncodingID { get; set; }
        public uint SubtableOffset { get; set; }

        public Format4Subtable GetFormat4Subtable(byte[] bytes, ref int offset)
        {
            if (PlatformID != 3 && EncodingID != 1)
            {
                throw new ArgumentException();
            }

            var tableOffset = Convert.ToInt32(SubtableOffset) + offset;

            return StructByteReader<Format4Subtable>.Read(bytes, ref tableOffset);
        }
    }
}
