using System;
using System.Buffers.Binary;
using static OpenGL.TextDrawing.OpenTypeParser;

namespace OpenGL.TextDrawing.OpenType
{
    internal class UInt16SegCountArrayReader : IElementByteReader<ushort[], Format4Subtable>
    {
        public ushort[] Read(Format4Subtable target, byte[] bytes, ref int offset)
        {
            var span = bytes.AsSpan(offset, target.SegCountX2 * sizeof(ushort) / 2);
            var array = new ushort[target.SegCountX2 / 2];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(i * sizeof(ushort), sizeof(ushort)));
            }

            offset += span.Length;
            return array;
        }
    }
}