using System.Collections.Generic;
using System;
using OpenGL.TextDrawing.Cff.Type2Charstring.Operators.PathConstruction;
using System.Linq;
using System.Collections;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2OneByteOperators
    {
        public const int Size = 1;

        static T2OneByteOperators()
        {
            var numbers = (Dictionary<byte, Type>)Numbers;

            var cffShortLs = Enumerable.Range(247, 254 - 247 + 1).Select(item => Convert.ToByte(item));

            foreach (var @byte in cffShortLs)
            {
                numbers.Add(@byte, typeof(CffLongShort));
            }

            var cffBytes = Enumerable.Range(32, 246 - 32 + 1).Select(item => Convert.ToByte(item));

            foreach (var @byte in cffBytes)
            {
                numbers.Add(@byte, typeof(CffByte));
            }
        }

        public static readonly IReadOnlyDictionary<byte, IExecutiveOperator> Operators = new Dictionary<byte, IExecutiveOperator>() 
        {
            { 1,    new HStem()       },
            { 3,    new VStem()       },
            { 4,    new VMoveTo()     },
            { 5,    new RLineTo()     },
            { 6,    new HLineTo()     },
            { 7,    new VLineTo()     },
            { 8,    new RRCurveTo()   },
            { 10,   new Callsubr()    },
            { 11,   new Return()      },
            { 12,   new Escape()      },
            { 14,   new Endchar()     },
            { 18,   new HStemhm()     },
            { 19,   new HintMask()    },
            { 20,   new CntrMask()    },
            { 21,   new RMoveTo()     },
            { 22,   new HMoveTo()     },
            { 23,   new VStemhm()     },
            { 24,   new RCurveLine()  },
            { 25,   new RLineCurve()  },
            { 26,   new VVCurveTo()   },
            { 27,   new HHCurveTo()   },
            { 29,   new Callgsubr()   },
            { 30,   new VHCurveTo()   },
            { 31,   new HVCurveTo()   },
        };

        public static readonly IReadOnlyDictionary<byte, Type> Numbers = new Dictionary<byte, Type>()
        {
            { 28,   typeof(CffShort) },
            { 255,  typeof(T2Float)  },
        };

        public static bool IsNumber(byte @byte)
        {
            //return IsCffByte(@byte) || IsCffShortL(@byte) || IsCffShort(@byte) || IsT2Float(@byte);
            return IsCffByte2(@byte) || IsCffShortL2(@byte) || IsCffShort2(@byte) || IsT2Float2(@byte);
        }

        public static bool IsTwoByteOperator(byte @byte)
        {
            return Operators.TryGetValue(@byte, out var value) && value.GetType() == typeof(Escape);
        }

        public static bool IsCffShortL(byte @byte)
        {
            return IsNumberOfType(@byte, typeof(CffLongShort));
        }

        public static bool IsCffShortL2(byte @byte)
        {
            return @byte >= 247 && @byte <= 254;
        }

        public static bool IsCffShort(byte @byte)
        {
            return IsNumberOfType(@byte, typeof(CffShort));
        }

        public static bool IsCffShort2(byte @byte)
        {
            return @byte == 28;
        }

        public static bool IsT2Float(byte @byte)
        {
            return IsNumberOfType(@byte, typeof(T2Float));
        }

        public static bool IsT2Float2(byte @byte)
        {
            return @byte == 255;
        }

        public static bool IsCffByte(byte @byte)
        {
            return IsNumberOfType(@byte, typeof(CffByte));
        }

        public static bool IsCffByte2(byte @byte)
        {
            return @byte >= 32 && @byte <= 246;
        }

        public static bool IsNumberOfType(byte @byte, Type type)
        {
            return Numbers.TryGetValue(@byte, out var value) && value == type;
        }

        public static bool IsMaskOperator(byte @byte)
        {
            return Operators.TryGetValue(@byte, out var value1) && value1.GetType() == typeof(HintMask) ||
                Operators.TryGetValue(@byte, out var value2) && value2.GetType() == typeof(CntrMask);
        }

        public static bool IsStemOperator(byte @byte)
        {
            return Operators.TryGetValue(@byte, out var value1) && value1.GetType() == typeof(HStem) ||
                Operators.TryGetValue(@byte, out var value2) && value2.GetType() == typeof(HStemhm) ||
                Operators.TryGetValue(@byte, out var value3) && value3.GetType() == typeof(VStem) ||
                Operators.TryGetValue(@byte, out var value4) && value4.GetType() == typeof(VStemhm);
        }
    }
}
