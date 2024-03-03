using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Index : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var i = context.ArgumentStack.Dequeue();
            Execute(context.ArgumentStack, (int)i);
        }

        public void Execute(Queue<T2Float> args, int i)
        {
            var arg = args.ElementAt(i);
            args.Enqueue(arg);
        }
    }
}
