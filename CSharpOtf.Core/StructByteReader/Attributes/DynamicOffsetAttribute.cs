using System;

namespace OpenGL.TextDrawing
{
    public class DynamicOffsetAttribute : Attribute
    {
        public Type DynamycOffsetType { get; }

        public DynamicOffsetAttribute(Type dynamycOffsetType)
        {
            DynamycOffsetType = dynamycOffsetType;
        }
    }
}