namespace OpenGL.TextDrawing.OpenType
{
    public struct Format4Subtable
    {
        public ushort Format { get; set; }
        public ushort Length { get; set; }
        public ushort Language { get; set; }
        public ushort SegCountX2 { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }

        [ElementByteReader(typeof(UInt16SegCountArrayReader))]
        public ushort[] EndCode { get; set; }
        public ushort ReversedPad { get; set; }

        [ElementByteReader(typeof(UInt16SegCountArrayReader))]
        public ushort[] StartCode { get; set; }

        [ElementByteReader(typeof(Int16SegCountArrayReader))]
        public short[] IdDelta { get; set; }

        [ElementByteReader(typeof(UInt16SegCountArrayReader))]
        public ushort[] IdRangeOffset { get; set; }

        [ElementByteReader(typeof(UInt16SegCountArrayReader))]
        public ushort[] GlyphIdArray { get; set; }
    }
}
