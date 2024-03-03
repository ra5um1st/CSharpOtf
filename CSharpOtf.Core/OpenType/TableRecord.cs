using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.OpenType
{
    public struct TableRecord
    {
        [ElementByteReader(typeof(TableRecordTagReader))]
        public string Tag;
        public uint CheckSum;
        public uint Offset;
        public uint Length;
    }
}
