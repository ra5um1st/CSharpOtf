namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class MoveOperator : ClearArgumentStackOperator, IMoveOperator
    {
        public override void Execute(T2ProgramContext context)
        {
            context.TryCloseCurrentPath();
            context.CurrentPoint += MoveTo(context);

            base.Execute(context);
        }

        public abstract T2Point MoveTo(T2ProgramContext context);
    }
}
