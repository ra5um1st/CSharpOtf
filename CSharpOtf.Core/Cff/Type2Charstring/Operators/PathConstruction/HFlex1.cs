namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    /// <summary>
    /// |- dx1 dy1 dx2 dy2 dx3 dx4 dx5 dy5 dx6 hflex1 (12 36) |-
    /// </summary>
    public class HFlex1 : FlexOperator
    {
        public override IFlexCurve AppendFlexCurve(T2ProgramContext context)
        {
            var point1 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + context.CurrentPoint;
            var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
            var point3 = new T2Point(context.ArgumentStack.Dequeue(), 0) + point2;
            var point4 = new T2Point(context.ArgumentStack.Dequeue(), 0) + point3;
            var point5 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point4;
            var point6 = new T2Point(context.ArgumentStack.Dequeue() + point5.X, point1.Y);

            var curve1 = new T2BezierCurve(context.CurrentPoint, point1, point2, point3);
            var curve2 = new T2BezierCurve(point3, point4, point5, point6);

            return new T2HFlexCurve(curve1, curve2);
        }
    }
}
