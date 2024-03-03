namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class ClearArgumentStackOperator : IExecutiveOperator
    {
        public virtual void Execute(T2ProgramContext context)
        {
            context.ArgumentStack.Clear();
        }
    }
}
