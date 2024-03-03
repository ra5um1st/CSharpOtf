using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public struct T2BezierCurve : IPathSegment
    {
        public T2Point StartPoint { get; set; }
        public T2Point ControlPoint1 { get; set; }
        public T2Point ControlPoint2 { get; set; }
        public T2Point EndPoint { get; set; }

        public T2BezierCurve(T2Point startPoint, T2Point controlPoint1, T2Point controlPoint2, T2Point endPoint)
        {
            StartPoint = startPoint;
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
            EndPoint = endPoint;
        }

        public T2BezierCurve(T2Point startPoint, T2Point controlPoint1, T2Point endPoint)
        {
            StartPoint = startPoint;
            ControlPoint1 = controlPoint1;
            EndPoint = endPoint;
            ControlPoint2 = default;
        }

        public IReadOnlyList<T2Point> GetPoints(int accuracy)
        {
            return new List<T2Point>();
        }

        public T2Point GetPoint(float t)
        {
            return new T2Point();
        }

        public override string ToString()
        {
            return $"{GetType().Name} {StartPoint} {ControlPoint1} {ControlPoint2} {EndPoint}";
        }
    }
}
