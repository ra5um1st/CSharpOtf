using System;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffInt
    {
        private readonly int _value;

        public CffInt(int value)
        {
            _value = value;
        }

        public CffInt(Span<byte> bytes)
        {
            _value = Parse(bytes);
        }

        public static CffInt Parse(Span<byte> bytes)
        {
            Validate(bytes);

            return bytes[1] << 24 | bytes[2] << 16 | bytes[3] << 8 | bytes[4];
        }

        public static bool TryParse(Span<byte> bytes, out CffInt value)
        {
            if (IsValid(bytes))
            {
                value = Parse(bytes);
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private static void Validate(Span<byte> bytes)
        {
            if (bytes.Length != 5)
            {
                throw new ArgumentException($"{nameof(bytes)} length must be 5.");
            }

            if (bytes[0] != CffOperator1.longint)
            {
                throw new ArgumentException($"First byte must be {CffOperator1.longint}.");
            }
        }

        private static bool IsValid(Span<byte> bytes)
        {
            if (bytes.Length != 5)
            {
                return false;
            }

            if (bytes[0] != CffOperator1.longint)
            {
                return false;
            }

            return true;
        }

        public static implicit operator int(CffInt self)
        {
            return self._value;
        }

        public static implicit operator CffInt(int value)
        {
            return new CffInt(value);
        }

        public static implicit operator CffInt(Span<byte> bytes)
        {
            return new CffInt(bytes);
        }

        public static implicit operator CffFloat(CffInt self)
        {
            return self._value;
        }

        public static int GetSize()
        {
            return sizeof(int) + CffOperator1.GetSize();
        }

        public override string ToString()
        {
            return $"{nameof(CffInt)} = {_value}";
        }
    }
}
