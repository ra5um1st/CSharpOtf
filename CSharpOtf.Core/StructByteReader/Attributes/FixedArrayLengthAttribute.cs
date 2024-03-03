namespace OpenGL.TextDrawing
{
    public class FixedArrayLengthAttribute : ArrayLengthAttribute
    {
        public int ArrayLength { get; }

        public FixedArrayLengthAttribute(int arrayLength)
        {
            ArrayLength = arrayLength;
        }

        public override int GetArrayLength(object target)
        {
            return ArrayLength;
        }
    }
}