using System.Collections.Generic;
using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff
{
    public class CharsetFormat1Reader : IElementByteReader<CharsetFormat1, Cff>
    {
        public CharsetFormat1 Read(Cff target, byte[] bytes, ref int offset)
        {
            var charsetOffset = target.Offset + target.TopDictIndex.Data.Charset;
            var charsetFormat1 = new CharsetFormat1();
            var glyphsCount = target.CharStringsIndex.Count;

            charsetFormat1.Format = bytes[charsetOffset++];
            var ranges1 = new List<Range1>(glyphsCount);

            for (int i = 0, glyphsReaded = 0; glyphsReaded < glyphsCount; glyphsReaded += 1 + ranges1[i].NLeft, i++)
            {
                ranges1.Add(StructByteReader<Range1>.Read(bytes, ref charsetOffset));
            }

            charsetFormat1.Ranges1 = ranges1.ToArray();
            offset = charsetOffset;

            return charsetFormat1;
        }
    }
}
