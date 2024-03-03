namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class FlexOperator : ClearArgumentStackOperator, IFlexOperator
    {
        public override void Execute(T2ProgramContext context)
        {
            var flexCurve = AppendFlexCurve(context);
            context.CurrentPath.AddPathSegment(flexCurve);
            context.CurrentPoint = flexCurve.EndPoint;

            base.Execute(context);
        }

        public abstract IFlexCurve AppendFlexCurve(T2ProgramContext context);
    }
}
