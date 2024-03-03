using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public readonly struct T2Float
    {
        private readonly float _value;

        public T2Float(float value)
        {
            _value = value;
        }

        public T2Float(ReadOnlySpan<byte> bytes)
        {
            _value = Parse(bytes);
        }

        public static implicit operator int(T2Float v)
        {
            return (int)v._value;
        }

        public static implicit operator float(T2Float v)
        {
            return v._value;
        }

        public static implicit operator T2Float(float v)
        {
            return new T2Float(v);
        }

        public static implicit operator T2Float(CffByte v)
        {
            return new T2Float(v);
        }

        public static implicit operator T2Float(CffFloat v)
        {
            return new T2Float(v);
        }

        public static implicit operator T2Float(CffInt v)
        {
            return new T2Float(v);
        }

        public static implicit operator T2Float(CffShort v)
        {
            return new T2Float(v);
        }

        public static implicit operator T2Float(CffLongShort v)
        {
            return new T2Float(v);
        }

        public static T2Float Parse(ReadOnlySpan<byte> bytes)
        {
            if (!T2OneByteOperators.IsT2Float(bytes[0]))
            {
                throw new ArgumentException($"'{string.Join(' ', bytes.ToArray())}' is not a T2Float number operator.");
            }

            return BitConverter.ToSingle(bytes.Slice(1));
        }

        public static int GetSize()
        {
            return 5;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
