using System;
using System.Data;
using System.Linq;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffOperator1
    {
        public static readonly CffOperator1 Version = new(0);
        public static readonly CffOperator1 Notice = new(1);
        public static readonly CffOperator1 FullName = new(2);
        public static readonly CffOperator1 FamilyName = new(3);
        public static readonly CffOperator1 Weight = new(4);
        public static readonly CffOperator1 FontBBox = new(5);
        public static readonly CffOperator1 BlueValues = new(6);
        public static readonly CffOperator1 OtherBlues = new(7);
        public static readonly CffOperator1 FamilyBlues = new(8);
        public static readonly CffOperator1 FamilyOtherBlues = new(9);
        public static readonly CffOperator1 StdHW = new(10);
        public static readonly CffOperator1 StdVW = new(11);
        public static readonly CffOperator1 Escape = new(12);
        public static readonly CffOperator1 UniqueID = new(13);
        public static readonly CffOperator1 XUID = new(14);
        public static readonly CffOperator1 Charset = new(15);
        public static readonly CffOperator1 Encoding = new(16);
        public static readonly CffOperator1 CharStrings = new(17);
        public static readonly CffOperator1 Private = new(18);
        public static readonly CffOperator1 Subrs = new(19);
        public static readonly CffOperator1 defaultWidthX = new(20);
        public static readonly CffOperator1 nominalWidthX = new(21);
        public static readonly CffOperator1 shortint = new(28);
        public static readonly CffOperator1 longint = new(29);
        public static readonly CffOperator1 BCD = new(30);
        public static readonly CffOperator1[] Numbers1 = Enumerable.Range(32, 246 - 33 + 2).Select(i => new CffOperator1((byte)i)).ToArray();
        public static readonly CffOperator1[] Numbers2 = Enumerable.Range(247, 254 - 247 + 2).Select(i => new CffOperator1((byte)i)).ToArray();
        public static readonly CffOperator1[] Reserved = Enumerable.Range(22, 27 - 22 + 2).Select(i => new CffOperator1((byte)i)).Concat(new CffOperator1[] { 31, 255 }).ToArray();

        private readonly byte _value;

        public CffOperator1(byte value)
        {
            _value = value;
        }

        public static bool IsNumber(byte value)
        {
            return value == shortint ||
                value == longint ||
                value == BCD ||
                Numbers1.Contains(value) ||
                Numbers2.Contains(value);
        }

        public static bool IsCffOperator2(byte value)
        {
            return value == Escape;
        }

        public static int GetSize()
        {
            return sizeof(byte);
        }

        public static implicit operator byte(CffOperator1 self)
        {
            return self._value;
        }

        public static implicit operator CffOperator1(byte value)
        {
            return new CffOperator1(value);
        }

        public override string ToString()
        {
            if (Numbers1.Contains(_value))
            {
                return $"{nameof(Numbers1)}";
            }
            else if (Numbers2.Contains(_value))
            {
                return $"{nameof(Numbers2)}";
            }
            else if (Reserved.Contains(_value))
            {
                return $"{nameof(Reserved)}";
            }
            else
            {
                var value = _value;
                var fieldName = typeof(CffOperator1)
                    .GetFields()
                    .First(field => (CffOperator1)field.GetValue(null) == value)
                    .Name;

                return $"{fieldName}";
            }
        }
    }
}
