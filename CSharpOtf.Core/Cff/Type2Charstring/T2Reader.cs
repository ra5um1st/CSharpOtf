using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2Reader
    {
        private delegate ReadOnlySpan<byte> ReadFunc(int size);
        private delegate ReadOnlySpan<byte> ReadNumberBytesFunc(byte @byte);

        private readonly ReadOnlyMemory<byte> _bytesMemory;

        public byte[] Bytes { get; }
        public int Offset { get; private set; }

        public T2Reader(byte[] bytes)
        {
            _bytesMemory = bytes.AsMemory();
            Bytes = bytes;
            Offset = 0;
        }

        public void Reset()
        {
            Offset = 0;
        }

        public ReadOnlySpan<byte> Read(int length)
        {
            ThrowIfCannotRead(length);

            var span = _bytesMemory.Span.Slice(Offset, length);
            Offset += length;

            return span;
        }

        public ReadOnlySpan<byte> Peek(int length)
        {
            ThrowIfCannotRead(length);

            return _bytesMemory.Span.Slice(Offset, length);
        }

        public byte GetNextByte()
        {
            ThrowIfEndOfArray();

            return Bytes[Offset];
        }

        private void ThrowIfEndOfArray()
        {
            if (Bytes.Length <= Offset)
            {
                throw new ArgumentException("Cannot read next operator. Array of bytes already read.");
            }
        }

        private void ThrowIfCannotRead(int length)
        {
            if (Offset + length > Bytes.Length)
            {
                throw new ArgumentException("Cannot read.");
            }
        }

        public T2Token PeekNextToken()
        {
            return ReadNextTokenInternal(
                PeekNumberBytes,
                Peek);
        }

        public T2Token ReadNextToken()
        {
            return ReadNextTokenInternal(
                ReadNumberBytes,
                Read);
        }

        private T2Token ReadNextTokenInternal(
            ReadNumberBytesFunc readNumberBytesFunc,
            ReadFunc readFunc)
        {
            ThrowIfEndOfArray();

            var nextByte = Bytes[Offset];
            var nextTokenType = T2Token.GetTokenType(nextByte);

            ReadOnlySpan<byte> nextOperatorBytes;
            Type nextOperatorType;

            switch (nextTokenType)
            {
                case T2TokenType.OneByteOperator:
                {
                    if (T2OneByteOperators.IsNumber(nextByte))
                    {
                        nextOperatorType = T2OneByteOperators.Numbers[nextByte];
                        nextOperatorBytes = readNumberBytesFunc(nextByte);

                        return new T2Token(nextTokenType, true, nextOperatorType, nextOperatorBytes);
                    }
                    else
                    {
                        nextOperatorType = T2OneByteOperators.Operators[nextByte].GetType();
                        nextOperatorBytes = readFunc(T2OneByteOperators.Size);

                        return new T2Token(nextTokenType, false, nextOperatorType, nextOperatorBytes);
                    }
                }
                case T2TokenType.TwoByteOperator:
                {
                    nextOperatorBytes = readFunc(T2TwoByteOperators.Size);
                    nextOperatorType = T2TwoByteOperators.Operators[nextOperatorBytes[1]].GetType();

                    return new T2Token(nextTokenType, false, nextOperatorType, nextOperatorBytes);
                }
                default:
                {
                    throw new NotSupportedException($"Operator with value '{nextByte}' is not supported. Offset: '{Offset}'.");
                }
            }
        }

        private ReadOnlySpan<byte> ReadNumberBytes(byte @byte)
        {
            return ReadNumberBytesInternal(@byte, Read);
        }

        private ReadOnlySpan<byte> PeekNumberBytes(byte @byte)
        {
            return ReadNumberBytesInternal(@byte, Peek);
        }

        private ReadOnlySpan<byte> ReadNumberBytesInternal(byte @byte, ReadFunc readFunc)
        {
            if (T2OneByteOperators.IsCffByte2(@byte))
            {
                return readFunc(CffByte.GetSize());
            }

            if (T2OneByteOperators.IsCffShortL2(@byte))
            {
                return readFunc(CffLongShort.GetSize());
            }

            if (T2OneByteOperators.IsCffShort2(@byte))
            {
                return readFunc(CffShort.GetSize());
            }

            if (T2OneByteOperators.IsT2Float2(@byte))
            {
                return readFunc(T2Float.GetSize());
            }

            throw new NotSupportedException($"Unsupported '{@byte}' number byte value.");
        }
    }
}