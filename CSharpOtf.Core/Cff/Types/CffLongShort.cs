using System;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffLongShort
    {
        private readonly short _value = 0;

        public CffLongShort(short value)
        {
            Validate(value);

            _value = value;
        }

        public CffLongShort(Span<byte> bytes)
        {
            _value = Parse(bytes);
        }

        public static CffLongShort Parse(ReadOnlySpan<byte> bytes)
        {
            Validate(bytes);

            if (InPositiveRange(bytes))
            {
                var value = (bytes[0] - 247) * 256 + bytes[1] + 108;

                return new CffLongShort((short)value);
            }
            else
            {
                var value = -(bytes[0] - 251) * 256 - bytes[1] - 108;

                return new CffLongShort((short)value);
            }

        }

        public static bool TryParse(ReadOnlySpan<byte> bytes, out CffLongShort value)
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
            if (bytes.Length != 2)
            {
                throw new ArgumentException($"{nameof(bytes)} length must be 2.");
            }

            if (!InPositiveRange(bytes) && !InNegativeRange(bytes))
            {
                throw new ArgumentException($"First byte does not in range from 247 to 254");
            }
        }

        public static bool IsValid(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 2)
            {
                return false;
            }

            if (!InPositiveRange(bytes) && !InNegativeRange(bytes))
            {
                return false;
            }

            return true;
        }

        private static bool InNegativeRange(ReadOnlySpan<byte> bytes)
        {
            return bytes[0] >= 251 && bytes[0] <= 254;
        }

        private static bool InPositiveRange(ReadOnlySpan<byte> bytes)
        {
            return bytes[0] >= 247 && bytes[0] <= 250;
        }

        private static void Validate(short value)
        {
            if (value > -108 && value < -1131)
            {
                throw new ArgumentException($"{nameof(value)} must be in range from -1131 to -108.");
            }

            if (value < 108 && value > 1131)
            {
                throw new ArgumentException($"{nameof(value)} must be in range from 108 to 1131.");
            }
        }

        public static implicit operator short(CffLongShort self)
        {
            return self._value;
        }

        public static implicit operator CffLongShort(short value)
        {
            return new CffLongShort(value);
        }

        public static implicit operator CffLongShort(Span<byte> bytes)
        {
            return new CffLongShort(bytes);
        }

        public static implicit operator CffInt(CffLongShort self)
        {
            return self._value;
        }

        public static implicit operator CffFloat(CffLongShort self)
        {
            return self._value;
        }

        public static int GetSize()
        {
            return sizeof(short);
        }

        public override string ToString()
        {
            return $"{nameof(CffLongShort)} = {_value}";
        }
    }
}
