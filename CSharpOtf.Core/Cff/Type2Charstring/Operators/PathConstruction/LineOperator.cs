using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class LineOperator : ClearArgumentStackOperator, ILineOperator
    {
        public override void Execute(T2ProgramContext context)
        {
            var lines = AppendLines(context);
            context.CurrentPath.AddPathSegments(lines.Cast<IPathSegment>());

            base.Execute(context);
        }

        public abstract IEnumerable<T2LineSegment> AppendLines(T2ProgramContext context);
    }
}
