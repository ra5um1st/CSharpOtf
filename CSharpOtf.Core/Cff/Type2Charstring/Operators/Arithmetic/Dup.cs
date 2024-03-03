using System;
using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Dup : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            throw new NotImplementedException();
        }

        public (ICloneable arg, ICloneable duplicate) Execute(ICloneable arg)
        {
            var duplicate = (ICloneable)arg.Clone();
            return (arg, duplicate);
        }
    }
}
