using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class And : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var num1 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 2);
            var num2 = context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 1);

            context.ArgumentStack.Enqueue(num1 != 0 && num2 != 0 ? 1 : 0);
        }
    }
}
