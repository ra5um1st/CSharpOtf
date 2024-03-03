using System;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffByte
    {
        private readonly sbyte _value = 0;

        private CffByte(sbyte value)
        {
            _value = value;
        }

        public CffByte(Span<byte> bytes)
        {
            Validate(bytes);

            _value = Parse(bytes);
        }

        public static CffByte Parse(ReadOnlySpan<byte> bytes)
        {
            return new CffByte((sbyte)(bytes[0] - 139));
        }

        public static bool TryParse(Span<byte> bytes, out CffByte value)
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
            if (bytes.Length != 1)
            {
                throw new ArgumentException($"Length of {nameof(bytes)} must be 1");
            }

            if (bytes[0] < 32 || bytes[0] > 246)
            {
                throw new ArgumentException($"Byte {nameof(bytes)} = {bytes[0]} must be in range from 32 to 246.");
            }
        }

        public static bool IsValid(Span<byte> bytes)
        {
            if (bytes.Length != 1)
            {
                return false;
            }

            if (bytes[0] < 32 || bytes[0] > 246)
            {
                return false;
            }

            return true;
        }

        public static implicit operator sbyte(CffByte self)
        {
            return self._value;
        }

        public static implicit operator CffByte(Span<byte> value)
        {
            return new CffByte(value);
        }

        public static implicit operator CffInt(CffByte self)
        {
            return self._value;
        }

        public static implicit operator CffFloat(CffByte self)
        {
            return self._value;
        }

        public static int GetSize()
        {
            return sizeof(byte);
        }

        public override string ToString()
        {
            return $"{_value}";
        }
    }
}
