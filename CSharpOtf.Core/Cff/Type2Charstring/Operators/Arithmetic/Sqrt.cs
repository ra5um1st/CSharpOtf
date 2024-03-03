using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    internal class Sqrt : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var arg = context.ArgumentStack.Dequeue();
            var result = Execute(arg);
            context.ArgumentStack.Enqueue(result);
        }

        public T2Float Execute(T2Float num)
        {
            return Convert.ToSingle(Math.Sqrt(num));
        }
    }
}
