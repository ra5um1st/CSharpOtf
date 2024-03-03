using System;
using System.Collections.Generic;
using System.IO;
using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class PrivateDictByteReader : IElementByteReader<Dictionary<object, List<object>>, PrivateDict>
        {
            public Dictionary<object, List<object>> Read(PrivateDict target, byte[] bytes, ref int offset)
            {
                var data = new Dictionary<object, List<object>>();

                //var length = 50;
                var span = bytes.AsSpan(offset, target.Size);

                for (int j = 0; j < span.Length;)
                {
                    var operands = ReadOperands(span, ref j);
                    var @operator = ReadOperator(span, ref j);

                    data.Add(@operator, operands);
                }

                offset += span.Length;

                return data;
            }

            private object ReadOperator(Span<byte> array, ref int offset)
            {
                if (CffOperator1.IsCffOperator2(array[offset]))
                {
                    var operator2 = new CffOperator2(array[offset]);
                    offset += CffOperator2.GetSize();

                    return operator2;
                }
                else
                {
                    return new CffOperator1(array[offset++]);
                }
            }

            private List<object> ReadOperands(Span<byte> array, ref int offset)
            {
                var operands = new List<object>();

                while (!IsOperatorReached(array, ref offset))
                {
                    try
                    {
                        if (CffByte.TryParse(array.Slice(offset, CffByte.GetSize()), out var cffByte))
                        {
                            operands.Add(cffByte);
                            offset += CffByte.GetSize();
                        }
                        else if (CffLongShort.TryParse(array.Slice(offset, CffLongShort.GetSize()), out var cffShortL))
                        {
                            operands.Add(cffShortL);
                            offset += CffLongShort.GetSize();
                        }
                        else if (CffShort.TryParse(array.Slice(offset, CffShort.GetSize()), out var cffShort))
                        {
                            operands.Add(cffShort);
                            offset += CffShort.GetSize();
                        }
                        else if (CffInt.TryParse(array.Slice(offset, CffInt.GetSize()), out var cffInt))
                        {
                            operands.Add(cffInt);
                            offset += CffInt.GetSize();
                        }
                        else if (CffFloat.TryParse(array.Slice(offset, CffFloat.GetSize(array.Slice(offset))), out var cffFloat))
                        {
                            operands.Add(cffFloat);
                            offset += CffFloat.GetSize(array.Slice(offset));
                        }
                    }
                    catch (Exception e)
                    {
                        throw new InvalidDataException("An operand can not be parsed.", e);
                    }
                }

                return operands;
            }

            private bool IsOperatorReached(Span<byte> array, ref int offset)
            {
                return !CffOperator1.IsNumber(array[offset]) || CffOperator1.IsCffOperator2(array[offset]);
            }
        }
    }
}