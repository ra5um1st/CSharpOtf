using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff
{
    internal class CharStringsIndexReader : IElementByteReader<CharStringsIndex, Cff>
    {
        public CharStringsIndex Read(Cff target, byte[] bytes, ref int offset)
        {
            var charstringsOffset = target.Offset + target.TopDictIndex.Data.CharStrings.Value;
            
            return StructByteReader<CharStringsIndex>.Read(bytes, ref charstringsOffset);
        }
    }
}
