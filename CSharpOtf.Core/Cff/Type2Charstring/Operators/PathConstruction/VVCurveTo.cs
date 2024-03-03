﻿using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    /// <summary>
    /// |- dx1? {dya dxb dyb dyc}+ vvcurveto (26) |-
    /// </summary>
    public class VVCurveTo : CurveOperator
    {
        public override IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context)
        {
            var lastPoint = context.CurrentPoint;
            var isFirstCurveVertical = context.ArgumentStack.Count % 4 == 1;
            var curvesCount = context.ArgumentStack.Count / 4;
            var curves = new List<T2BezierCurve>(curvesCount);

            while (curves.Count < curvesCount) 
            {
                T2Point point1;
                T2Point point2;
                T2Point point3;

                if (isFirstCurveVertical)
                {
                    isFirstCurveVertical = false;

                    point1 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + lastPoint;
                    point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
                    point3 = new T2Point(0, context.ArgumentStack.Dequeue()) + point2;
                }
                else
                {
                    point1 = new T2Point(0, context.ArgumentStack.Dequeue()) + lastPoint;
                    point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
                    point3 = new T2Point(0, context.ArgumentStack.Dequeue()) + point2;
                }

                curves.Add(new T2BezierCurve(lastPoint, point1, point2, point3));
                lastPoint = point3;
            }

            return curves;
        }
    }
}
