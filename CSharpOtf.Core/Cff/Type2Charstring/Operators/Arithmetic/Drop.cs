namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Drop : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            context.ArgumentStack.Dequeue();
        }
    }
}
