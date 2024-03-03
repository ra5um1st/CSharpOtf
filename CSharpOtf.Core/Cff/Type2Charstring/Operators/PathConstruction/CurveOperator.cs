using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class CurveOperator : ClearArgumentStackOperator, ICurveOperator
    {
        public override void Execute(T2ProgramContext context)
        {
            var curves = AppendCurves(context).Cast<IPathSegment>();
            context.CurrentPath.AddPathSegments(curves);
            context.CurrentPoint = curves.Last().EndPoint;

            base.Execute(context);
        }

        public abstract IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context);
    }
}
