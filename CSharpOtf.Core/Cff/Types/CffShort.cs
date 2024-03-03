using System;
using System.Buffers.Binary;
using System.Numerics;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffShort
    {
        private readonly short _value;

        public CffShort(short value)
        {
            _value = value;
        }

        public CffShort(Span<byte> bytes)
        {
            _value = Parse(bytes);
        }

        public static CffShort Parse(ReadOnlySpan<byte> bytes)
        {
            Validate(bytes);

            return (short)(bytes[1] << 8 | bytes[2]);
        }

        public static bool TryParse(Span<byte> bytes, out CffShort value)
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

        private static void Validate(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentException($"{nameof(bytes)} length must be 3.");
            }

            if (bytes[0] != CffOperator1.shortint)
            {
                throw new ArgumentException($"First byte must be {CffOperator1.shortint}");
            }
        }

        public static bool IsValid(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 3)
            {
                return false;
            }

            if (bytes[0] != CffOperator1.shortint)
            {
                return false;
            }

            return true;
        }

        public static implicit operator short(CffShort self)
        {
            return self._value;
        }

        public static implicit operator CffShort(short value)
        {
            return new CffShort(value);
        }

        public static implicit operator CffShort(Span<byte> bytes)
        {
            return new CffShort(bytes);
        }

        public static implicit operator CffInt(CffShort self)
        {
            return self._value;
        }

        public static implicit operator CffFloat(CffShort self)
        {
            return self._value;
        }

        public static int GetSize()
        {
            return sizeof(short) + CffOperator1.GetSize();
        }

        public override string ToString()
        {
            return $"{nameof(CffShort)} = {_value}";
        }
    }
}
