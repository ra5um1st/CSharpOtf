using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    /// <summary>
    /// |- {dxa dya dxb dyb dxc dyc}+ dxd dyd rcurveline (24) |-
    /// </summary>
    public class RCurveLine : ClearArgumentStackOperator
    {
        private static readonly RRCurveTo RRCurveTo = new RRCurveTo();
        private static readonly RLineTo RLineTo = new RLineTo();

        public override void Execute(T2ProgramContext context)
        {
            var (Curves, Line) = AppendCurvesAndLine(context);
            context.CurrentPath.AddPathSegments(Curves.Cast<IPathSegment>());
            context.CurrentPath.AddPathSegment(Line);
            context.CurrentPoint = context.CurrentPath.EndPoint;

            base.Execute(context);
        }

        public (IEnumerable<T2BezierCurve> Curves, T2LineSegment Line) AppendCurvesAndLine(T2ProgramContext context)
        {
            // TODO: убрать этот костыль с програм контекстом
            var curvesCount = (context.ArgumentStack.Count - 2) / 6;
            var localContext = new T2ProgramContext(context.CffTable, context.LocalSurboutineBias, context.GlobalSurboutineBias, context.Index)
            {
                CurrentPath = context.CurrentPath,
                CurrentPoint = context.CurrentPoint
            };

            for (int i = 0; i < curvesCount; i++)
            {
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
                localContext.ArgumentStack.Enqueue(context.ArgumentStack.Dequeue());
            }

            var curves = RRCurveTo.AppendCurves(localContext);

            context.CurrentPoint = curves.Last().EndPoint;
            context.CurrentPath = localContext.CurrentPath;

            var line = RLineTo.AppendLines(context).First();

            return (curves, line);
        }
    }
}
