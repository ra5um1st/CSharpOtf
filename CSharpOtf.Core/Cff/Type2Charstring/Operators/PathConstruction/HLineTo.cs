using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class HLineTo : LineOperator
    {
        // dx1 {dya dxb}*
        // {dxa dyb}+
        public override IEnumerable<T2LineSegment> AppendLines(T2ProgramContext context)
        {
            var isEven = context.ArgumentStack.Count % 2 == 0;
            var linesCount = context.ArgumentStack.Count;
            var lines = new List<T2LineSegment>(linesCount);

            if (!isEven)
            {
                var hLinePoint2 = new T2Point(context.ArgumentStack.Dequeue(), 0) + context.CurrentPoint;
                lines.Add(new T2LineSegment(context.CurrentPoint, hLinePoint2));
                context.CurrentPoint = hLinePoint2;
            }

            while (lines.Count < linesCount)
            {
                ReadLines(context, lines, isEven);
            }

            return lines;
        }

        private static void ReadLines(T2ProgramContext context, List<T2LineSegment> lines, bool isEven)
        {
            T2Point hLinePoint2;
            T2Point vLinePoint2;

            if (isEven)
            {
                hLinePoint2 = new T2Point(context.ArgumentStack.Dequeue(), 0) + context.CurrentPoint;
                lines.Add(new T2LineSegment(context.CurrentPoint, hLinePoint2));

                vLinePoint2 = new T2Point(0, context.ArgumentStack.Dequeue()) + hLinePoint2;
                lines.Add(new T2LineSegment(hLinePoint2, vLinePoint2));

                context.CurrentPoint = vLinePoint2;
            }
            else
            {

                vLinePoint2 = new T2Point(0, context.ArgumentStack.Dequeue()) + context.CurrentPoint;
                lines.Add(new T2LineSegment(context.CurrentPoint, vLinePoint2));

                hLinePoint2 = new T2Point(context.ArgumentStack.Dequeue(), 0) + vLinePoint2;
                lines.Add(new T2LineSegment(vLinePoint2, hLinePoint2));

                context.CurrentPoint = hLinePoint2;
            }
        }
    }
}
