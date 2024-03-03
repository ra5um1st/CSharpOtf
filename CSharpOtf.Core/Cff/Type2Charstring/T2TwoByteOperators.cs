using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2TwoByteOperators
    {
        public const int Size = 2;

        public static readonly IReadOnlyDictionary<byte, IExecutiveOperator> Operators = new Dictionary<byte, IExecutiveOperator>
        {
           { 3,  new And()    },
           { 4,  new Or()     },
           { 5,  new Not()    },
           { 9,  new Abs()    },
           { 10, new Add()    },
           { 11, new Sub()    },
           { 12, new Div()    },
           { 14, new Neg()    },
           { 15, new Eq()     },
           { 18, new Drop()   },
           { 20, new Put()    },
           { 21, new Get()    },
           { 22, new IfElse() },
           { 23, new Random() },
           { 24, new Mul()    },
           { 26, new Sqrt()   },
           { 27, new Dup()    },
           { 28, new Exch()   },
           { 29, new Index()  },
           { 30, new Roll()   },
           { 34, new HFlex()  },
           { 35, new Flex()   },
           { 36, new HFlex1() },
           { 37, new Flex1()  },
        };

        public static readonly byte[] Reserved = new byte[]
        {
            0,
            1,
            2,
            6,
            7,
            8,
            13,
            16,
            17,
            19,
            25,
            31,
            32,
            33,
        }
        .Concat(Enumerable.Range(38, 255 - 38 + 2).Select(item => (byte)item ))
        .ToArray();
    }
}
