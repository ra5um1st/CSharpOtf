using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class RLineTo : LineOperator
    {
        // {dxa dya}+
        public override IEnumerable<T2LineSegment> AppendLines(T2ProgramContext context)
        {
            var linesCount = context.ArgumentStack.Count / 2;
            var lines = new List<T2LineSegment>(linesCount);

            while (lines.Count < linesCount)
            {
                var point2 = new T2Point(context.ArgumentStack.Dequeue(), context.ArgumentStack.Dequeue()) + context.CurrentPoint;
                lines.Add(new T2LineSegment(context.CurrentPoint, point2));
                context.CurrentPoint = point2;
            }

            return lines;
        }
    }
}
