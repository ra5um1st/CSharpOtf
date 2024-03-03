using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Callgsubr : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            //var argument = context.ArgumentStack.Dequeue();
            var argument = context.ArgumentStack.Last();
            context.ArgumentStack = new Queue<T2Float>(context.ArgumentStack.RemoveLast());
            var subrIndex = context.GetSubrIndex(argument, CffSubrIndexType.GlobalSubrIndex);
            var subr = context.GlobalSubroutines[subrIndex];
            subr.Execute(context);
        }
    }
}
