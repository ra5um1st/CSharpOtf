using System;
using System.Text;
using OpenGL.TextDrawing.OpenType;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class TableRecordTagReader : IElementByteReader<string, TableRecord>
        {
            public string Read(TableRecord target, byte[] bytes, ref int offset)
            {
                var tagSize = 4;
                var span = bytes.AsSpan(offset, tagSize);
                var tag = Encoding.UTF8.GetString(span).Trim();
                offset += tagSize;

                return tag;
            }
        }
    }
}