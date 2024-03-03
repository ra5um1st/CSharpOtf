using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Eq : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var num1 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 2);
            var num2 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 1);

            context.ArgumentStack.Enqueue(num1 == num2 ? 1 : 0);
        }
    }
}
