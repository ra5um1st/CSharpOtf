using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface IMoveOperator
    {
        T2Point MoveTo(T2ProgramContext context);
    }
}
