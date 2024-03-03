using System;
using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        internal class LocalSubrIndexByteReader : IElementByteReader<byte[][], LocalSubrIndex>
        {
            public byte[][] Read(LocalSubrIndex target, byte[] bytes, ref int offset)
            {
                var data = new byte[target.Count][];

                for (int i = 0; i < target.Count; i++)
                {
                    var length = target.ArrayOffset[i + 1] - target.ArrayOffset[i];
                    data[i] = bytes.AsSpan(offset, length).ToArray();
                    offset += length;
                }

                return data;
            }
        }
    }
}