namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public interface IElementByteReader<TResult, TTarget>
        {
            TResult Read(TTarget target, byte[] bytes, ref int offset);
        }
    }
}