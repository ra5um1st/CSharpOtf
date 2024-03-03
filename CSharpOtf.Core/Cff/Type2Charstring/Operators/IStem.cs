using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface IStem
    {
        bool IsVertical { get; }
        public T2Float Stem1 { get; }
        public T2Float Stem2 { get; }
    }
}