namespace OpenGL.TextDrawing.OpenType
{
    public struct TableDirectory
    {
        public uint SfntVersion;
        public ushort TableCount;
        public ushort SearchRange;
        public ushort EntrySelector;
        public ushort RangeShift;

        [DynamicArrayLength(nameof(TableCount))]
        public TableRecord[] TableRecords;
    }
}
