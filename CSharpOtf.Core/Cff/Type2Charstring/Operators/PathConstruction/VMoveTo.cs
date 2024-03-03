namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class VMoveTo : MoveOperator
    {
        public override T2Point MoveTo(T2ProgramContext context)
        {
            context.TryDefineCharstringWidth(false);

            var point = new T2Point(0, context.ArgumentStack.Dequeue());

            return point;
        }
    }
}
