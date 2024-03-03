using System;
using System.Buffers.Binary;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public static class BigEndianPrimitiveReader<T>
        {
            public static T Read(byte[] bytes, ref int offset)
            {
                if (!typeof(T).IsPrimitive)
                {
                    throw new ArgumentException(nameof(T));
                }

                object value;

                var typeCode = Type.GetTypeCode(typeof(T));
                var elementSize = GetTypeCodeSize(typeCode);
                var span = bytes.AsSpan(offset, elementSize);
                offset += elementSize;

                switch (typeCode)
                {
                    case TypeCode.Byte:
                    {
                        value = span[0];
                        break;
                    }
                    case TypeCode.SByte:
                    {
                        value = (sbyte)span[0];
                        break;
                    }
                    case TypeCode.Int16:
                    {
                        value = BinaryPrimitives.ReadInt16BigEndian(span);
                        break;
                    }
                    case TypeCode.UInt16:
                    {
                        value = BinaryPrimitives.ReadUInt16BigEndian(span);
                        break;
                    }
                    case TypeCode.Int32:
                    {
                        value = BinaryPrimitives.ReadInt32BigEndian(span);
                        break;
                    }
                    case TypeCode.UInt32:
                    {
                        value = BinaryPrimitives.ReadUInt32BigEndian(span);
                        break;
                    }
                    case TypeCode.Int64:
                    {
                        value = BinaryPrimitives.ReadUInt32BigEndian(span);
                        break;
                    }
                    case TypeCode.UInt64:
                    {
                        value = BinaryPrimitives.ReadUInt64BigEndian(span);
                        break;
                    }
                    default:
                        throw new NotSupportedException($"Type {typeCode.GetType()} is not a value type.");
                }

                return (T)Convert.ChangeType(value, typeCode);
            }

            private static int GetTypeCodeSize(TypeCode typeCode)
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        return sizeof(bool);
                    case TypeCode.Char:
                        return sizeof(char);
                    case TypeCode.SByte:
                        return sizeof(sbyte);
                    case TypeCode.Byte:
                        return sizeof(byte);
                    case TypeCode.Int16:
                        return sizeof(short);
                    case TypeCode.UInt16:
                        return sizeof(ushort);
                    case TypeCode.Int32:
                        return sizeof(int);
                    case TypeCode.UInt32:
                        return sizeof(uint);
                    case TypeCode.Int64:
                        return sizeof(long);
                    case TypeCode.UInt64:
                        return sizeof(ulong);
                    case TypeCode.Single:
                        return sizeof(float);
                    case TypeCode.Double:
                        return sizeof(double);
                    case TypeCode.Decimal:
                        return sizeof(decimal);
                    case TypeCode.DateTime:
                        return 8;
                    default:
                        throw new NotSupportedException($"Can not get TypeCode size. {typeCode} is not supported.");
                }
            }
        }
    }
}