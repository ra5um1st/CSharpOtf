namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Div : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var arg1 = context.ArgumentStack.Dequeue();
            var arg2 = context.ArgumentStack.Dequeue();
            var result = Execute(arg1, arg2);
            context.ArgumentStack.Enqueue(result);
        }

        public T2Float Execute(T2Float num1, T2Float num2)
        {
            return num1 / num2;
        }
    }
}
