using System.Collections.Generic;
using OpenGL.TextDrawing.Cff.Type2Charstring.Operators;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2SubpathGroup1
    {
        public List<IStem> HStems { get; } = new();
        public List<IStem> VStems { get; } = new();
        public List<IMask> CntrMasks { get; } = new();
        public List<IMask> HintMasks { get; } = new();
        public IMoveOperator MoveToOperator { get; set; }
        
        public T2Subpath Subpath { get; } = new();
    }
}