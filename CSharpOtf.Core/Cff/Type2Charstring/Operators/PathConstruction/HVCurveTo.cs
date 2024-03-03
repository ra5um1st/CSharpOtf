﻿using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring.Operators.PathConstruction
{
    public class HVCurveTo : CurveOperator
    {
        public override IEnumerable<T2BezierCurve> AppendCurves(T2ProgramContext context)
        {
            var lastPoint = context.CurrentPoint;
            var curvesCount = context.ArgumentStack.Count / 4;
            var curves = new List<T2BezierCurve>(curvesCount);
            var hasLastArgument = context.ArgumentStack.Count % 2 == 1;
            var isFirstPattern = context.ArgumentStack.Count % 8 >= 4;

            while (curves.Count < curvesCount)
            {
                // dx1 dx2 dy2 dy3 {dya dxb dyb dxc dxd dxe dye dyf}* dxf?
                if (isFirstPattern)
                {
                    if (curves.Count == 0)
                    {
                        var lastCurve = GetEndsVerticalCurve(context, ref lastPoint);
                        
                        if (curves.Count == curvesCount - 1 && hasLastArgument)
                        {
                            lastCurve.EndPoint += new T2Point(context.ArgumentStack.Dequeue(), 0);
                        }

                        curves.Add(lastCurve);
                    }

                    if (context.ArgumentStack.Count / 8 >= 1)
                    {
                        curves.Add(GetEndsHorizontalCurve(context, ref lastPoint));
                        var lastCurve = GetEndsVerticalCurve(context, ref lastPoint);

                        if (curves.Count == curvesCount - 1 && hasLastArgument)
                        {
                            lastCurve.EndPoint += new T2Point(context.ArgumentStack.Dequeue(), 0);
                        }

                        curves.Add(lastCurve);
                    }
                }
                // {dxa dxb dyb dyc dyd dxe dye dxf}+ dyf?
                else
                {
                    curves.Add(GetEndsVerticalCurve(context, ref lastPoint));
                    var lastCurve = GetEndsHorizontalCurve(context, ref lastPoint);

                    if (curves.Count == curvesCount - 1 && hasLastArgument)
                    {
                        lastCurve.EndPoint += new T2Point(0, context.ArgumentStack.Dequeue());
                    }

                    curves.Add(lastCurve);
                }
            }

            return curves;
        }

        // dxa dxb dyb dyc
        private T2BezierCurve GetEndsVerticalCurve(T2ProgramContext context, ref T2Point lastPoint)
        {
            var point1 = new T2Point(context.ArgumentStack.Dequeue(), 0) + lastPoint;
            var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
            var point3 = new T2Point(0, context.ArgumentStack.Dequeue()) + point2;
            var curve = new T2BezierCurve(lastPoint, point1, point2, point3);
            lastPoint = point3;

            return curve;
        }

        // dya dxb dyb dxc
        private T2BezierCurve GetEndsHorizontalCurve(T2ProgramContext context, ref T2Point lastPoint)
        {
            var point1 = new T2Point(0, context.ArgumentStack.Dequeue()) + lastPoint;
            var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
            var point3 = new T2Point(context.ArgumentStack.Dequeue(), 0) + point2;
            var curve = new T2BezierCurve(lastPoint, point1, point2, point3);
            lastPoint = point3;

            return curve;
        }
    }
}
