namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class HMoveTo : MoveOperator
    {
        public override T2Point MoveTo(T2ProgramContext context)
        {
            context.TryDefineCharstringWidth(false);

            return new T2Point(context.ArgumentStack.Dequeue(), 0);
        }
    }
}
