using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public readonly ref struct T2Token
    {
        public T2TokenType TokenType { get; }
        public bool IsNumber { get; }
        public Type OperatorType { get; }
        public ReadOnlySpan<byte> Value { get; }

        public T2Token(T2TokenType tokenType, bool isNumber, Type operatorType, ReadOnlySpan<byte> value)
        {
            TokenType = tokenType;
            IsNumber = isNumber;
            OperatorType = operatorType;
            Value = value;
        }

        public static T2TokenType GetTokenType(byte @byte)
        {
            var tokenType = T2TokenType.Unknown;

            if (T2OneByteOperators.IsNumber(@byte))
            {
                return T2TokenType.OneByteOperator;
            }
            
            if (T2OneByteOperators.IsTwoByteOperator(@byte))
            {
                return T2TokenType.TwoByteOperator;
            }

            if (T2OneByteOperators.Operators.ContainsKey(@byte))
            {
                return T2TokenType.OneByteOperator;
            }

            return tokenType;
        }

        public CffByte CreateCffByte()
        {
            ThrowIfIncorrectType(typeof(CffByte));

            return CffByte.Parse(Value);
        }

        public CffLongShort CreateCffShortL()
        {
            ThrowIfIncorrectType(typeof(CffLongShort));

            return CffLongShort.Parse(Value);
        }

        public CffShort CreateCffShort()
        {
            ThrowIfIncorrectType(typeof(CffShort));

            return CffShort.Parse(Value);
        }

        public T2Float CreateT2Float()
        {
            ThrowIfIncorrectType(typeof(T2Float));

            return T2Float.Parse(Value);
        }

        public T2Float CreateOrCastT2Float()
        {
            if (OperatorType == typeof(CffByte))
            {
                return CreateCffByte();
            }

            if (OperatorType == typeof(CffLongShort))
            {
                return CreateCffShortL();
            }

            if (OperatorType == typeof(CffShort))
            {
                return CreateCffShort();
            }

            if (OperatorType == typeof(T2Float))
            {
                return CreateT2Float();
            }

            throw new NotSupportedException();
        }

        public IExecutiveOperator CreateOperator()
        {
            return (IExecutiveOperator)Activator.CreateInstance(OperatorType);
        }

        private readonly void ThrowIfIncorrectType(Type type)
        {
            if (OperatorType != type)
            {
                throw new ArgumentException($"Token operator type '{OperatorType}' is not a '{type}' type.");
            }
        }

        public override string ToString()
        {
            return $"{OperatorType.Name} : {string.Join(' ', Value.ToArray())}";
        }
    }
}
