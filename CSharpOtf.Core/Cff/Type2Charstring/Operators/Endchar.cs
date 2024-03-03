using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Endchar : ClearArgumentStackOperator
    {
        public override void Execute(T2ProgramContext context)
        {
            context.TryCloseCurrentPath();
            context.TryDefineCharstringWidth(true);

            base.Execute(context);
        }
    }
}
