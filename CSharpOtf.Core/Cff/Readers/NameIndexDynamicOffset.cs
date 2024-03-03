namespace OpenGL.TextDrawing
{
    public class NameIndexDynamicOffset : IDynamicOffset
    {
        public int GetOffset(object target)
        {
            var cff = (Cff.Cff)target;

            return cff.Header.Size - 4;
        }
    }
}