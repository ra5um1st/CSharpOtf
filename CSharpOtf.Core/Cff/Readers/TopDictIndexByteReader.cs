using System;
using System.Collections.Generic;
using System.IO;
using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class TopDictIndexByteReader : IElementByteReader<TopDictData, TopDictIndex>
        {
            public TopDictData Read(TopDictIndex target, byte[] bytes, ref int offset)
            {
                var data = new TopDictData();

                for (int i = 0; i < target.Count; i++)
                {
                    var length = target.ArrayOffset[i + 1] - target.ArrayOffset[i];
                    var span = bytes.AsSpan(offset, length);

                    for (int j = 0; j < length;)
                    {
                        var operands = ReadOperands(span, ref j);
                        var operatorBytes = ReadOperator(span, ref j);
                        AddToTopDictData(data, operatorBytes, operands);
                    }

                    offset += length;
                }

                return data;
            }

            private void AddToTopDictData(TopDictData data, ReadOnlySpan<byte> operatorBytes, List<CffFloat> operands)
            {
                if (CffOperator1.IsCffOperator2(operatorBytes[0]))
                {
                    var @operator = new CffOperator2(operatorBytes[1]);

                    if (@operator == CffOperator2.Copyright)
                    {
                        data.Copyright = operands[0];
                    }
                    else if (@operator == CffOperator2.IsFixedPitch)
                    {
                        data.IsFixedPitch = operands[0] == 1 ||
                            (operands[0] == 0 ?
                                false :
                                throw new ArgumentException($"Cannot convert '{operands[0]}' to bool type."));
                    }
                    else if (@operator == CffOperator2.ItalicAngle)
                    {
                        data.ItalicAngle = operands[0];
                    }
                    else if (@operator == CffOperator2.UnderlinePosition)
                    {
                        data.UnderlinePosition = operands[0];
                    }
                    else if (@operator == CffOperator2.UnderlineThickness)
                    {
                        data.UnderlineThickness = operands[0];
                    }
                    else if (@operator == CffOperator2.PaintType)
                    {
                        data.PaintType = operands[0];
                    }
                    else if (@operator == CffOperator2.CharstringType)
                    {
                        data.CharstringType = operands[0];
                    }
                    else if (@operator == CffOperator2.FontMatrix)
                    {
                        if (operands.Count % 2 == 1)
                        {
                            throw new ArgumentException("Matrix cannot be even.");
                        }

                        data.FontMatrix = operands.ToArray();
                    }
                    else if (@operator == CffOperator2.StrokeWidth)
                    {
                        data.StrokeWidth = operands[0];
                    }
                    else if (@operator == CffOperator2.SyntheticBase)
                    {
                        data.SyntheticBase = operands[0];
                    }
                    else if (@operator == CffOperator2.PostScript)
                    {
                        data.PostScript = operands[0];
                    }
                    else if (@operator == CffOperator2.BaseFontName)
                    {
                        data.BaseFontName = operands[0];
                    }
                    else if (@operator == CffOperator2.BaseFontBlend)
                    {
                        data.BaseFontBlend = operands.ConvertAll<CffInt>(item => item).ToArray();
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                else
                {
                    var @operator = new CffOperator1(operatorBytes[0]);

                    if (@operator == CffOperator1.Version)
                    {
                        data.Version = operands[0];
                    }
                    else if (@operator == CffOperator1.Notice)
                    {
                        data.Notice = operands[0];
                    }
                    else if (@operator == CffOperator1.FullName)
                    {
                        data.FullName = operands[0];
                    }
                    else if (@operator == CffOperator1.FamilyName)
                    {
                        data.FamilyName = operands[0];
                    }
                    else if (@operator == CffOperator1.Weight)
                    {
                        data.Weight = operands[0];
                    }
                    else if (@operator == CffOperator1.UniqueID)
                    {
                        data.UniqueID = operands[0];
                    }
                    else if (@operator == CffOperator1.FontBBox)
                    {
                        if (operands.Count != 4)
                        {
                            throw new ArgumentException("FontBBox must have size of 4.");
                        }

                        data.FontBBox = operands.ToArray();
                    }
                    else if (@operator == CffOperator1.XUID)
                    {
                        data.XUID = operands.ToArray();
                    }
                    else if (@operator == CffOperator1.Charset)
                    {
                        data.Charset = operands[0];
                    }
                    else if (@operator == CffOperator1.Encoding)
                    {
                        data.Encoding = operands[0];
                    }
                    else if (@operator == CffOperator1.CharStrings)
                    {
                        data.CharStrings = operands[0];
                    }
                    else if (@operator == CffOperator1.Private)
                    {
                        data.Private = new Private()
                        {
                            PrivateDictSize = operands[0],
                            PrivateDictOffset = operands[1],
                        };
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            private ReadOnlySpan<byte> ReadOperator(ReadOnlySpan<byte> bytes, ref int offset)
            {
                if (CffOperator1.IsCffOperator2(bytes[offset]))
                {
                    var size = CffOperator2.GetSize();
                    var operatorBytes = bytes.Slice(offset, size);
                    offset += size;

                    return operatorBytes;
                }
                else
                {
                    var size = CffOperator1.GetSize();
                    var operatorBytes = bytes.Slice(offset, size);
                    offset += size;

                    return operatorBytes;
                }
            }

            private List<CffFloat> ReadOperands(Span<byte> array, ref int offset)
            {
                var operands = new List<CffFloat>();

                while (!IsOperatorReached(array, ref offset))
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
                    else
                    {
                        throw new InvalidDataException("An operand can not be parsed.");
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