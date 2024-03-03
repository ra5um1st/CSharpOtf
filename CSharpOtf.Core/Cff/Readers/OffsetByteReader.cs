using System;
using System.Buffers.Binary;
using OpenGL.TextDrawing.Cff;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public class OffsetByteReader : IElementByteReader<Offset, ICffIndex>
        {
            public Offset Read(ICffIndex target, byte[] bytes, ref int offset)
            {
                int value = 0;

                // TODO: Вынести куда-нибудь провеку на размер элемента
                if (target.ElementOffset == 1)
                {
                    value = bytes[offset];
                    offset += sizeof(byte);
                }
                else if (target.ElementOffset == 2)
                {
                    value = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(offset, sizeof(short)));
                    offset += sizeof(short);
                }
                else if (target.ElementOffset == 3)
                {
                    var reader = new BigEndianInt24Reader();
                    value = Convert.ToInt32(reader.Read(target, bytes, ref offset));
                }
                else if (target.ElementOffset == 4)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    //throw new NotSupportedException();
                }

                return new Offset(target.ElementOffset, value);
            }
        }
    }
}