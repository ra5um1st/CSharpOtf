using System;
using System.Buffers.Binary;

namespace OpenGL.TextDrawing.Cff
{
    public struct CffFloat
    {
        private static readonly int _decimalPoint = 0xa;
        private static readonly int _exponent = 0xb;
        private static readonly int _negativeExponent = 0xc;
        private static readonly int _minus = 0xe;
        private static readonly int _startOfRealNumber = 0x1e;
        private static readonly int _endOfRealNumber = 0xf;

        private readonly float _value = 0f;

        public CffFloat(float value)
        {
            _value = value;
        }

        public CffFloat(Span<byte> bytes)
        {
            Validate(bytes);

            _value = Parse(bytes);
        }

        private static CffFloat Parse(Span<byte> bytes)
        {
            var nibbles = new byte[2];
            var value = 0f;
            var localExponent = 1f;
            var exponent = 0;
            var exponentValue = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (IsEnd(bytes[i]))
                {
                    break;
                }

                if (IsStart(bytes[i]))
                {
                    continue;
                }

                nibbles[0] = GetLowNibble(bytes[i]);
                nibbles[1] = GetHighNibble(bytes[i]);

                for (int j = 0; j < nibbles.Length; j++)
                {
                    if (IsNumber(nibbles[j]) && exponent == 0)
                    {
                        value += nibbles[j] / localExponent;
                    }

                    if (IsNumber(nibbles[j]) && exponent != 0)
                    {
                        exponentValue = Convert.ToInt32(Math.Pow(10, nibbles[j]));
                    }

                    if (nibbles[j] == _decimalPoint ||
                        localExponent > 1)
                    {
                        localExponent *= 10;
                    }

                    if (nibbles[j] == _minus)
                    {
                        value *= -1;
                    }

                    if (nibbles[j] == _negativeExponent)
                    {
                        exponent = _negativeExponent;
                    }

                    if (nibbles[j] == _exponent)
                    {
                        exponent = _exponent;
                    }
                }

                if (exponent != 0)
                {
                    if (exponent == _exponent)
                    {
                        value *= exponentValue;
                    }
                    else
                    {
                        value /= exponentValue;
                    }
                }
            }

            return value;
        }

        public static bool TryParse(Span<byte> bytes, out CffFloat value)
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

        public static implicit operator float(CffFloat self)
        {
            return self._value;
        }

        public static implicit operator int(CffFloat self)
        {
            return (int)self._value;
        }

        public static implicit operator CffInt(CffFloat self)
        {
            return (int)self._value;
        }

        public static implicit operator CffFloat(double value)
        {
            var @float = Convert.ToSingle(value);
            return new CffFloat(@float);
        }

        public static implicit operator CffFloat(float value)
        {
            return new CffFloat(value);
        }

        public static implicit operator CffFloat(Span<byte> bytes)
        {
            return new CffFloat(bytes);
        }

        private static void Validate(Span<byte> bytes)
        {
            if (bytes[0] != _startOfRealNumber)
            {
                throw new ArgumentException("First byte of CFF float number must be 0x1e");
            }

            if (!IsEnd(bytes[bytes.Length - 1]))
            {
                throw new ArgumentException("Last byte nibble of CFF float number must be 0xf.");
            }

            if (!ContainsNumber(bytes))
            {
                throw new ArgumentException($"{nameof(bytes)} must contain at least one number.");
            }
        }

        public static bool IsValid(Span<byte> bytes)
        {
            if (bytes[0] != _startOfRealNumber)
            {
                return false;
            }

            if (!IsEnd(bytes[bytes.Length - 1]))
            {
                return false;
            }

            if (!ContainsNumber(bytes))
            {
                return false;
            }

            return true;
        }

        private static bool ContainsNumber(Span<byte> bytes)
        {
            for (int i = 1; i < bytes.Length; i++)
            {
                var lowNibble = GetLowNibble(bytes[i]);
                var highNibble = GetHighNibble(bytes[i]);

                if (IsNumber(lowNibble) || IsNumber(highNibble))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsNumber(byte value)
        {
            return value >= 0 && value <= 9;
        }

        private static byte GetHighNibble(byte value)
        {
            return (byte)(value & 0x0F);
        }

        private static byte GetLowNibble(byte value)
        {
            return (byte)((value & 0xF0) >> 4);
        }

        public static int GetSize(ReadOnlySpan<byte> bytes)
        {
            var size = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                size++;

                if (IsEnd(bytes[i]))
                {
                    return size;
                }
            }

            throw new ArgumentException($"Cannot recognize a cff real number size. Span of bytes has no end of cff real number byte '{_endOfRealNumber}'.");
        }

        private static bool IsEnd(byte value)
        {
            var lowNibble = GetLowNibble(value);
            var highNibble = GetHighNibble(value);

            return lowNibble == _endOfRealNumber || highNibble == _endOfRealNumber;
        }

        private static bool IsStart(byte value)
        {
            return value == _startOfRealNumber;
        }

        public override string ToString()
        {
            return $"{nameof(CffFloat)} = {_value}";
        }
    }
}
