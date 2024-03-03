namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Get : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var i = (int)context.ArgumentStack.Peek();
            var val = context.TransientArray[i];
            context.ArgumentStack.Enqueue(val);
        }
    }
}
