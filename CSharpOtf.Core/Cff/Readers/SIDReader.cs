using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff.Readers
{
    public class SIDReader : IElementByteReader<SID, Range1>
    {
        public SID Read(Range1 target, byte[] bytes, ref int offset)
        {
            return BigEndianPrimitiveReader<ushort>.Read(bytes, ref offset);
        }
    }
}