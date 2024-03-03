using System.Collections.Generic;
using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.Cff
{
    public struct PrivateDict
    {
        [StructReaderIgnore]
        public int Size { get; set; }

        [ElementByteReader(typeof(PrivateDictByteReader))]
        public Dictionary<object, List<object>> Data;
    }
}
