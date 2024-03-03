using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class BigEndianInt24Reader : IElementByteReader<Int24, object>
        {
            public Int24 Read(object target, byte[] bytes, ref int offset)
            {
                var value = bytes[offset] << 16 | bytes[offset + 1] << 8 | bytes[offset + 2];
                offset += Int24.GetSize();

                return value;
            }
        }
    }
}