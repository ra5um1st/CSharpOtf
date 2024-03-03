using System;

namespace OpenGL.TextDrawing.Cff
{
    public struct OffSize
    {
        public sbyte Value;

        public OffSize()
        {
            Value = 1;
        }

        public OffSize(int value)
        {
            if (value < 1 || value > 4)
            {
                throw new ArgumentException("OffSize must be in range from 1 to 4.");
            }

            Value = (sbyte)value;
        }

        public static implicit operator CffInt(OffSize self)
        {
            return self.Value;
        }

        public static implicit operator int(OffSize self)
        {
            return self.Value;
        }

        public static implicit operator OffSize(int value)
        {
            return new OffSize(value);
        }

        public Type GetValueType()
        {
            switch (Value)
            {
                case 1:
                {
                    return typeof(byte);
                }
                case 2:
                {
                    return typeof(short);
                }
                case 3:
                {
                    return typeof(Int24);
                }
                case 4:
                {
                    return typeof(int);
                }
                default:
                    throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return $"{nameof(OffSize)} = {Value}";
        }
    }
}
