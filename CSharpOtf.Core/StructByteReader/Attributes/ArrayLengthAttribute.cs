using System;

namespace OpenGL.TextDrawing
{
    public abstract class ArrayLengthAttribute : Attribute, ITargetMemberArrayLength
    {
        public abstract int GetArrayLength(object target);
    }
}