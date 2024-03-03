using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2ProgramContext
    {
        public T2Float? Width { get; set; }
        public T2Point CurrentPoint { get; set; }
        public T2Path Path { get; set; } = new();

        // TODO : убрать потом, когда через билдер чарстирнга будет сделано
        public T2Path CurrentPath { get; set; } = new();
        public Queue<T2Float> ArgumentStack { get; set; } = new(10);
        public List<T2Float> TransientArray { get; } = new(10);
        public List<IStem> Stems { get; } = new();
        public List<IMask> HintMasks { get; } = new();
        public int LocalSurboutineBias { get; }
        public int GlobalSurboutineBias { get; }
        public Cff CffTable { get; }

        public int Index { get; }
        public SID SID => GetSID();
        public string Name => SID.ToString(CffTable);

        public IReadOnlyList<T2Subroutine> LocalSubroutines { get; set; }
        public IReadOnlyList<T2Subroutine> GlobalSubroutines { get; set; }

        public T2ProgramContext(Cff cffTable, int localSurbBias, int globalSurbBias, int index)
        {
            CffTable = cffTable;
            LocalSurboutineBias = localSurbBias;
            GlobalSurboutineBias = globalSurbBias;
            Index = index;
        }

        private SID GetSID()
        {
            var sid = new SID();
            var currentIndex = 0;

            for (int i = 0; i < CffTable.CharsetFormat1.Ranges1.Length; i++)
            {
                currentIndex += 1 + CffTable.CharsetFormat1.Ranges1[i].NLeft;

                if (currentIndex >= Index)
                {
                    sid = CffTable.CharsetFormat1.Ranges1[i].SID + CffTable.CharsetFormat1.Ranges1[i].NLeft + Index - currentIndex;
                    break;
                }
            }

            return Index == 0 ? Index : sid;
        }

        public int GetSubrIndex(int argument, CffSubrIndexType subrIndexType)
        {
            return subrIndexType == CffSubrIndexType.LocalSubrIndex ? LocalSurboutineBias + argument : GlobalSurboutineBias + argument;
        }

        public void TryDefineCharstringWidth(bool isOperatorArgumentsEven)
        {
            if (Width.HasValue)
            {
                return;
            }

            var hasCustomWidth = isOperatorArgumentsEven ? ArgumentStack.Count % 2 == 1 : ArgumentStack.Count % 2 == 0;

            if (hasCustomWidth)
            {
                var nominalWidth = (CffLongShort)CffTable.PrivateDict.Data[CffOperator1.nominalWidthX].First();
                var customWidth = nominalWidth + ArgumentStack.Dequeue();
                Width = customWidth;
            }
            else
            {
                Width = CffTable.PrivateDict.Data.ContainsKey(CffOperator1.defaultWidthX) ? 
                    (CffLongShort)CffTable.PrivateDict.Data[CffOperator1.defaultWidthX].First() :
                    0;
            }
        }
        public void TryCloseCurrentPath()
        {
            if (CurrentPath.Close())
            {
                //TODO: сделать в билдере чарстринга создание пути по паттерну
                Path.AddPathSegments(CurrentPath.PathSegments);
                CurrentPath = new();
            }

            // single edge
            if (CurrentPath.PathSegments.Count == 1)
            {

            }
        }
    }
}
