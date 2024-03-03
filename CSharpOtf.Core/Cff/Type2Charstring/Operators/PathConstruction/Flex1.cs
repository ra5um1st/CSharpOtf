namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Flex1 : FlexOperator, IFlexOperator, IExecutiveOperator
    {
        public override IFlexCurve AppendFlexCurve(T2ProgramContext context)
        {
            var point1 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + context.CurrentPoint;
            var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point1;
            var point3 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point2;
            var point4 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point3;
            var point5 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + point4;
            var point6 = context.CurrentPoint;

            var sumPoint = point1 + point2 + point3 + point4 + point5;

            if (sumPoint.X > sumPoint.Y)
            {
                point6.X = context.ArgumentStack.Dequeue();
            }
            else
            {
                point6.Y = context.ArgumentStack.Dequeue();
            }

            var curve1 = new T2BezierCurve(context.CurrentPoint, point1, point2, point3);
            var curve2 = new T2BezierCurve(point3, point4, point5, point6);

            return new T2HFlexCurve(curve1, curve2);
        }
    }
}
