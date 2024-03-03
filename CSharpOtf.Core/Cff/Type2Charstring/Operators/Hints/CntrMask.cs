using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class CntrMask : ClearArgumentStackOperator, IMask, IExecutiveOperator
    {
        private readonly List<MaskGroup> _maskGroups = new List<MaskGroup>(2);
        public IReadOnlyList<MaskGroup> MaskGroups => _maskGroups;

        public IReadOnlyList<byte> Mask { get; set; }

        public override void Execute(T2ProgramContext context)
        {
            context.TryDefineCharstringWidth(true);

            var binaryPresentation = Mask.SelectMany(item => Convert.ToString(item, 2).PadLeft(8, '0').Select(item =>
            {
                switch (item)
                {
                    case '0':
                        return (byte)0;
                    case '1':
                        return (byte)1;
                    default:
                        return (byte)2;
                }
            })).ToArray();

            for (int j = 0; j < binaryPresentation.Length; j++)
            {
                var stem = context.Stems.ElementAtOrDefault(j);
                var isActive = binaryPresentation[j] == 1;
                _maskGroups.Add(new MaskGroup(stem, isActive));
            }

            context.HintMasks.Add(this);
            base.Execute(context);
        }

        public static int GetSize(int stemsCount)
        {
            return (stemsCount + ((sizeof(byte) * 8) - 1)) / (sizeof(byte) * 8);
        }
    }
}
