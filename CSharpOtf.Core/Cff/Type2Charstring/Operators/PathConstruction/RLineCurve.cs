using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    /// <summary>
    /// |- {dxa dya}+ dxb dyb dxc dyc dxd dyd rlinecurve (25) |-
    /// </summary>
    public class RLineCurve : ClearArgumentStackOperator
    {
        private static readonly RLineTo RLineTo = new RLineTo();
        private static readonly RRCurveTo RRCurveTo = new RRCurveTo();

        public override void Execute(T2ProgramContext context)
        {
            var (Lines, Curve) = AppendLinesAndCurve(context);
            context.CurrentPath.AddPathSegments(Lines.Cast<IPathSegment>());
            context.CurrentPath.AddPathSegment(Curve);
            context.CurrentPoint = context.CurrentPath.EndPoint;

            base.Execute(context);
        }

        public (IEnumerable<T2LineSegment> Lines, T2BezierCurve Curve) AppendLinesAndCurve(T2ProgramContext context)
        {
            // TODO: убрать этот костыль с програм контекстом
            var linesCount = (context.ArgumentStack.Count - 6) / 2;
            var localContext = new T2ProgramContext(context.CffTable, context.LocalSurboutineBias, context.GlobalSurboutineBias, context.Index)
            {
                CurrentPath = context.CurrentPath,
                CurrentPoint = context.CurrentPoint
            };

            for (int i = 0; i < linesCount; i++)
            {
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
            }

            var lines = RLineTo.AppendLines(localContext);

            context.CurrentPoint = localContext.CurrentPoint;
            context.CurrentPath = localContext.CurrentPath;

            var curve = RRCurveTo.AppendCurves(context).First();

            return (lines, curve);
        }
    }
}
