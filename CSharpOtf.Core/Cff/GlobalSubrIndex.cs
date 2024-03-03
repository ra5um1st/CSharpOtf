using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff
{
    public struct GlobalSubrIndex : ICffIndex
    {
        public ushort Count { get; set; }
        public OffSize ElementOffset { get; set; }

        [ElementByteReader(typeof(OffsetByteReader))]
        [DynamicArrayLength(nameof(Count), AdditionalLength = 1)]
        public Offset[] ArrayOffset { get; set; }

        [ElementByteReader(typeof(GlobalSubrIndexByteReader))]
        [DynamicArrayLength(nameof(Count))]
        public byte[][] Data { get; set; }
    }
}
