namespace OpenGL.TextDrawing.OpenType
{
    public struct Cmap
    {
        public ushort Version { get; set; }
        public ushort NumTables { get; set; }

        [DynamicArrayLength(nameof(NumTables))]
        public EncodingRecord[] EncodingRecords { get; set; }
    }
}
