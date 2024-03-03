using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public struct T2FlexCurve : IFlexCurve
    {
        public int Depth { get; }
        public T2BezierCurve Curve1 { get; }
        public T2BezierCurve Curve2 { get; }

        public T2Point StartPoint => Curve1.StartPoint;

        public T2Point EndPoint => Curve2.EndPoint;

        public T2FlexCurve(int depth, T2BezierCurve curve1, T2BezierCurve curve2)
        {
            Depth = depth;
            Curve1 = curve1;
            Curve2 = curve2;
        }

        public bool IsCurved(float currentDepth)
        {
            return currentDepth < Depth / 100f;
        }

        public IReadOnlyList<T2Point> GetPoints(int accuracy)
        {
            throw new System.NotImplementedException();
        }
    }
}
