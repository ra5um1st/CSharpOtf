namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Random : IExecutiveOperator
    {
        private static readonly System.Random _random = new System.Random();

        public void Execute(T2ProgramContext context)
        {
            var result = Execute();
            context.ArgumentStack.Enqueue(result);
        }

        public T2Float Execute()
        {
            var randomValue = 0f;

            while (randomValue == 0)
            {
                randomValue = _random.NextSingle();
            }

            return randomValue;
        }
    }
}
