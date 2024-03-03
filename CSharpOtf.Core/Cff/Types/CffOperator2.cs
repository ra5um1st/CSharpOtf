using System;
using System.Data;
using System.Linq;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffOperator2
    {
        public static readonly CffOperator2 Copyright = new(0);
        public static readonly CffOperator2 IsFixedPitch = new(1);
        public static readonly CffOperator2 ItalicAngle = new(2);
        public static readonly CffOperator2 UnderlinePosition = new(3);
        public static readonly CffOperator2 UnderlineThickness = new(4);
        public static readonly CffOperator2 PaintType = new(5);
        public static readonly CffOperator2 CharstringType = new(6);
        public static readonly CffOperator2 FontMatrix = new(7);
        public static readonly CffOperator2 StrokeWidth = new(8);
        public static readonly CffOperator2 BlueScale = new(9);
        public static readonly CffOperator2 BlueShift = new(10);
        public static readonly CffOperator2 BlueFuzz = new(11);
        public static readonly CffOperator2 StemSnapH = new(12);
        public static readonly CffOperator2 StemSnapV = new(13);
        public static readonly CffOperator2 ForceBold = new(14);
        public static readonly CffOperator2 LanguageGroup = new(17);
        public static readonly CffOperator2 ExpansionFactor = new(18);
        public static readonly CffOperator2 initialRandomSeed = new(19);
        public static readonly CffOperator2 SyntheticBase = new(20);
        public static readonly CffOperator2 PostScript = new(21);
        public static readonly CffOperator2 BaseFontName = new(22);
        public static readonly CffOperator2 BaseFontBlend = new(23);
        public static readonly CffOperator2 ROS = new(30);
        public static readonly CffOperator2 CIDFontVersion = new(31);
        public static readonly CffOperator2 CIDFontRevision = new(32);
        public static readonly CffOperator2 CIDFontType = new(33);
        public static readonly CffOperator2 CIDCount = new(34);
        public static readonly CffOperator2 UIDBase = new(35);
        public static readonly CffOperator2 FDArray = new(36);
        public static readonly CffOperator2 FDSelect = new(37);
        public static readonly CffOperator2 FontName = new(38);
        public static readonly CffOperator2[] Reserved = InitReserved();

        private readonly byte[] _value = new byte[2] { CffOperator1.Escape, 0 };

        public CffOperator2(byte[] value)
        {
            if (value.Length != 2)
            {
                throw new ArgumentException(nameof(value));
            }

            if (value[0] != CffOperator1.Escape)
            {
                throw new ArgumentException(nameof(value));
            }

            _value = value;
        }

        public CffOperator2(byte secondValue)
        {
            _value[1] = secondValue;
        }

        public static int GetSize()
        {
            return sizeof(byte) * 2;
        }

        public static implicit operator byte[](CffOperator2 self)
        {
            return self._value;
        }

        public static implicit operator byte(CffOperator2 self)
        {
            return self._value[1];
        }

        public static implicit operator CffOperator2(byte[] value)
        {
            return new CffOperator2(value);
        }

        public static implicit operator CffOperator2(byte secondValue)
        {
            return new CffOperator2(secondValue);
        }

        private static CffOperator2[] InitReserved()
        {
            return Enumerable.Range(39, 255 - 39 + 2)
                .Select(i => new CffOperator2((byte)i))
                .Concat(Enumerable.Range(24, 29 - 24 + 2).Select(i => new CffOperator2((byte)i)))
                .Concat(Enumerable.Range(15, 16 - 15 + 2).Select(i => new CffOperator2((byte)i)))
                .Concat(new CffOperator2[] { 39 })
                .ToArray();
        }

        public override string ToString()
        {
            if (Reserved.Contains(_value))
            {
                return $"{nameof(Reserved)}";
            }
            else
            {
                var value = _value[1];
                var fieldName = typeof(CffOperator2)
                    .GetFields()
                    .First(field => (CffOperator2)field.GetValue(null) == value)
                    .Name;

                return $"{fieldName}";
            }
        }
    }
}
