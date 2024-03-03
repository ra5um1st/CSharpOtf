using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff
{
    public struct TopDictIndex : ICffIndex
    {
        public ushort Count { get; set; }
        public OffSize ElementOffset { get; set; }

        [DynamicArrayLength(nameof(Count), AdditionalLength = 1)]
        public byte[] ArrayOffset { get; set; }


        // TODO: Привести к единому числовому формату значения (CffInt). Ключ сделать byte
        [ElementByteReader(typeof(TopDictIndexByteReader))]
        public TopDictData Data { get; set; }
    }
}
