using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface ICurveOperator
    {
        IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context);
    }
}