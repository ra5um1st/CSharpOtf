using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Exch : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var arg1 = context.ArgumentStack.Dequeue();
            var arg2 = context.ArgumentStack.Dequeue();

            var result = Execute(arg1, arg2);

            context.ArgumentStack.Enqueue(result.num1);
            context.ArgumentStack.Enqueue(result.num2);
        }

        public (T2Float num1, T2Float num2) Execute(T2Float num1, T2Float num2)
        {
            return (num2, num1);
        }
    }
}
