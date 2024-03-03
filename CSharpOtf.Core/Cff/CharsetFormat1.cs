using OpenGL.TextDrawing.Cff.Readers;

namespace OpenGL.TextDrawing.Cff
{
    public struct CharsetFormat1
    {
        public byte Format { get; set; }
        public Range1[] Ranges1 { get; set; }
    }

    public struct Range1
    {
        [ElementByteReader(typeof(SIDReader))]
        public SID SID { get; set; }
        public byte NLeft { get; set; }
    }
}
