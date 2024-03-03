using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public struct T2LineSegment : IPathSegment
    {
        public T2Point StartPoint { get; set; }
        public T2Point EndPoint { get; set; }

        public T2LineSegment(T2Point point1, T2Point point2)
        {
            StartPoint = point1;
            EndPoint = point2;
        }

        public IReadOnlyList<T2Point> GetPoints(int accuracy)
        {
            return new List<T2Point>();
        }

        public T2Point GetPoint(float t)
        {
            var diffPoint = EndPoint - StartPoint;

            return StartPoint + diffPoint * t;
        }

        public override string ToString()
        {
            return $"{GetType().Name} {StartPoint} {EndPoint}";
        }
    }
}