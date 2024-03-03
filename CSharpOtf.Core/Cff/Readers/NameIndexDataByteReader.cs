using System;
using System.Text;
using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class NameIndexDataByteReader : IElementByteReader<string[], NameIndex>
        {
            public string[] Read(NameIndex target, byte[] bytes, ref int offset)
            {
                var data = new string[target.Count];

                for (int i = 0; i < target.Count; i++)
                {
                    var length = target.ArrayOffset[i + 1] - target.ArrayOffset[i];
                    data[i] = Encoding.UTF8.GetString(bytes.AsSpan(offset, length));
                    offset += length;
                }

                return data;
            }
        }
    }
}