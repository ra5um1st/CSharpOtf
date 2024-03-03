using OpenGL.TextDrawing.Cff.Readers;

namespace OpenGL.TextDrawing.Cff
{
    public class Cff
    {
        [StructReaderIgnore]
        public int Offset { get; set; }

        public Header Header { get; set; }
        [DynamicOffset(typeof(NameIndexDynamicOffset))]
        public NameIndex NameIndex { get; set; }
        public TopDictIndex TopDictIndex { get; set; }
        public StringIndex StringIndex { get; set; }
        public GlobalSubrIndex GlobalSubrIndex { get; set; }

        [ElementByteReader(typeof(CharStringsIndexReader))]
        public CharStringsIndex CharStringsIndex { get; set; }

        [ElementByteReader(typeof(CharsetFormat1Reader))]
        public CharsetFormat1 CharsetFormat1 { get; set; }

        [StructReaderIgnore]
        public PrivateDict PrivateDict { get; set; }

        public int GetSubrBias(CffSubrIndexType subrIndexType, LocalSubrIndex localSubrIndex)
        {
            int bias;
            var subrsCount = subrIndexType == CffSubrIndexType.LocalSubrIndex ?
                localSubrIndex.Count : // There should be a LocalSubrIndex readed from PrivateDict if exists
                GlobalSubrIndex.Count;

            var charstringType = TopDictIndex.Data.CharstringType;

            if (charstringType == 1)
            {
                bias = 0;
            }
            else if (subrsCount < 1240)
            {
                bias = 107;
            }
            else if (subrsCount < 33900)
            {
                bias = 1131;
            }
            else
            {
                bias = 32768;
            }

            return bias;
        }
    }

    public enum CffSubrIndexType
    {
        LocalSubrIndex,
        GlobalSubrIndex,
    }
}
