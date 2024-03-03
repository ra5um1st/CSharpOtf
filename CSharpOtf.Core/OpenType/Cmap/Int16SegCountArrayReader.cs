using System.Buffers.Binary;
using System;
using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.OpenType
{
    public class Int16SegCountArrayReader : IElementByteReader<short[], Format4Subtable>
    {
        public short[] Read(Format4Subtable target, byte[] bytes, ref int offset)
        {
            var span = bytes.AsSpan(offset, target.SegCountX2 * sizeof(short) / 2);
            var array = new short[target.SegCountX2 / 2];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = BinaryPrimitives.ReadInt16BigEndian(span.Slice(i * sizeof(short), sizeof(short)));
            }

            offset += span.Length;
            return array;
        }
    }
}