using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class IfElse : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var s1 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 4);
            var s2 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 3);
            var v1 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 2);
            var v2 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 1);

            context.ArgumentStack.Enqueue(v1 <= v2 ? s2 : s1);
        }
    }
}
