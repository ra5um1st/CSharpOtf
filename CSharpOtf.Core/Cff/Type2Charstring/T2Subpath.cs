using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2Subpath
    {
        public List<ILineOperator> LineOperators { get; } = new();
        public List<ICurveOperator> CurveOperators { get; } = new();
        public List<RLineCurve> LineCurveOperators { get; } = new();
        public List<RCurveLine> CurveLineOperators { get; } = new();
        public List<IFlexOperator> FlexOperators { get; } = new();
    }
}
