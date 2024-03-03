using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface ILineOperator
    {
        IEnumerable<T2LineSegment> AppendLines(T2ProgramContext context);
    }
}