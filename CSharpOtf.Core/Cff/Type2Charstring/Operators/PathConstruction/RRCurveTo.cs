using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class RRCurveTo : CurveOperator
    {
        public override IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context)
        {
            var lastPoint = context.CurrentPoint;
            var curvesCount = context.ArgumentStack.Count / 6;
            var curves = new List<T2BezierCurve>(curvesCount);

            while (curves.Count < curvesCount) 
            {
                var point1 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + lastPoint;
                var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
                var point3 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point2;

                curves.Add(new T2BezierCurve(lastPoint, point1, point2, point3));
                lastPoint = point3;
            }

            return curves;
        }
    }
}
