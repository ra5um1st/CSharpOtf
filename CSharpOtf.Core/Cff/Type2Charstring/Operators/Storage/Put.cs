using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Put : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var val = (int)context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 2);
            var i = (int)context.ArgumentStack.ElementAt(context.ArgumentStack.Count - 1);
            context.TransientArray.EnsureCapacity(i);
            context.TransientArray[i] = val;
        }
    }
}
