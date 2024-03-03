using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class HHCurveTo : CurveOperator
    {
        // dy1? {dxa dxb dyb dxc}+
        public override IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context)
        {
            var lastPoint = context.CurrentPoint;
            var isFirstCurveHorizontal = context.ArgumentStack.Count % 4 == 1;
            var curvesCount = context.ArgumentStack.Count / 4;
            var curves = new List<T2BezierCurve>(curvesCount);

            while (curves.Count < curvesCount)
            {
                T2Point point1;
                T2Point point2;
                T2Point point3;

                if (isFirstCurveHorizontal)
                {
                    isFirstCurveHorizontal = false;

                    var point1Y = context.ArgumentStack.Dequeue();
                    var point1X = context.ArgumentStack.Dequeue();
                    //var point1X = context.ArgumentStack.Dequeue();
                    //var point1Y = context.ArgumentStack.Dequeue();

                    point1 = new T2Point(point1X, point1Y) + lastPoint;
                    point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
                    point3 = new T2Point(context.ArgumentStack.Dequeue(), 0) + point2;
                }
                else
                {
                    point1 = new T2Point(context.ArgumentStack.Dequeue(), 0) + lastPoint;
                    point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
                    point3 = new T2Point(context.ArgumentStack.Dequeue(), 0) + point2;
                }

                curves.Add(new T2BezierCurve(lastPoint, point1, point2, point3));
                lastPoint = point3;
            }

            return curves;
        }
    }
}
