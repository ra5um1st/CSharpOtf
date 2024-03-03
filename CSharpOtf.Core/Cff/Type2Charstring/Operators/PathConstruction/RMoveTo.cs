namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class RMoveTo : MoveOperator
    {
        public override T2Point MoveTo(T2ProgramContext context)
        {
            context.TryDefineCharstringWidth(true);

            var dx = context.ArgumentStack.TryDequeue(out var dx1) ? dx1 : new T2Float(0);
            var dy = context.ArgumentStack.TryDequeue(out var dy1) ? dy1 : new T2Float(0);

            var point = new T2Point(dx, dy);

            return point;
        }
    }
}
