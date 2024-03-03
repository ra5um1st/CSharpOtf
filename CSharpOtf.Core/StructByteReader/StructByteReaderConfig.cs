namespace OpenGL.TextDrawing.StructByteReader
{
    public class StructByteReaderConfig
    {
        public static StructByteReaderConfig Default { get; } = new StructByteReaderConfig()
        {
            IsBigEndian = true,
        };

        public bool IsBigEndian { get; set; }
    }
}