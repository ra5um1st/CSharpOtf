using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public readonly struct T2Subroutine : IExecutiveOperator
    {
        private readonly T2Reader _reader;

        public T2Subroutine(T2Reader reader)
        {
            _reader = reader;
        }

        public void Execute(T2ProgramContext context)
        {
            while (_reader.Offset < _reader.Bytes.Length)
            {
                var token = _reader.ReadNextToken();

                if (token.IsNumber)
                {
                    var number = token.CreateOrCastT2Float();
                    context.ArgumentStack.Enqueue(number);
                }
                else if (token.OperatorType == typeof(HintMask) ||
                    token.OperatorType == typeof(CntrMask))
                {
                    var vstemOperator = T2OneByteOperators.Operators[3];
                    vstemOperator.Execute(context);

                    var @operator = T2OneByteOperators.Operators[token.Value[0]];
                    var mask = (IMask)@operator;

                    var maskBytesCount = HintMask.GetSize(context.Stems.Count);
                    mask.Mask = _reader.Read(maskBytesCount).ToArray();

                    @operator.Execute(context);
                }
                else if (token.TokenType == T2TokenType.TwoByteOperator)
                {
                    var @operator = T2TwoByteOperators.Operators[token.Value[1]];
                    @operator.Execute(context);
                } 
                else if (token.TokenType == T2TokenType.OneByteOperator)
                {
                    var @operator = T2OneByteOperators.Operators[token.Value[0]];
                    @operator.Execute(context);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            _reader.Reset();
        }
    }
}
