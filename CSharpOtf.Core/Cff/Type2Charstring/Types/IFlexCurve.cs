namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface IFlexCurve : IPathSegment
    {
        T2BezierCurve Curve1 { get; }
        T2BezierCurve Curve2 { get; }

        public bool IsCurved(float currentDepth);
    }
}