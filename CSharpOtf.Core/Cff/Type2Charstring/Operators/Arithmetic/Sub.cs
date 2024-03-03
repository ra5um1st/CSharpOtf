using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Sub : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var arg1 = context.ArgumentStack.Dequeue();
            var arg2 = context.ArgumentStack.Dequeue();
            var result = Execute(arg1, arg2);
            context.ArgumentStack.Enqueue(result);
        }

        public T2Float Execute(T2Float arg1, T2Float arg2)
        {
            return arg1 - arg2;
        }
    }
}
