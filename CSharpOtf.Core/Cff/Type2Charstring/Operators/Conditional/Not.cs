using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Not : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var num1 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 1);

            context.ArgumentStack.Enqueue(num1 == 0 ? 1 : 0);
        }
    }
}
