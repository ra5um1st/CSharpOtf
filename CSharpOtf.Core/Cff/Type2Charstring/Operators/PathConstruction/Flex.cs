using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Flex : FlexOperator
    {
        private static readonly RRCurveTo RRCurveTo = new RRCurveTo();

        public override IFlexCurve AppendFlexCurve(T2ProgramContext context)
        {
            var curves = RRCurveTo.AppendCurves(context);
            var curve1 = curves.ElementAt(1);
            var curve2 = curves.ElementAt(2);
            var depth = (int)context.ArgumentStack.Dequeue();

            return new T2FlexCurve(depth, curve1, curve2);
        }
    }
}
