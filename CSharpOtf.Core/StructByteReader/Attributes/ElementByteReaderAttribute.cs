using System;

namespace OpenGL.TextDrawing
{
    public class ElementByteReaderAttribute : Attribute
    {
        public Type ElementByteReaderType { get; }

        public ElementByteReaderAttribute(Type elementByteReaderType)
        {
            ElementByteReaderType = elementByteReaderType;
        }
    }
}