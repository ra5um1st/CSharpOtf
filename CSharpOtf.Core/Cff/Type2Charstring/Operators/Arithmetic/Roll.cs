using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class Roll : IExecutiveOperator
    {
        public void Execute(T2ProgramContext context)
        {
            var j = (int)context.ArgumentStack.Dequeue();
            var n = (int)context.ArgumentStack.Dequeue();
            var result = Execute(context.ArgumentStack.ToArray(), n, j);
            context.ArgumentStack.Clear();

            foreach (var num in result)
            {
                context.ArgumentStack.Enqueue(num);
            }
        }

        public T2Float[] Execute(T2Float[] nums, int n, int j)
        {
            if (n < 0)
            {
                throw new ArgumentException($"{nameof(n)} must be a non negative.");
            }

            for (int i = 0; i < n; i++)
            {
                var temp = nums[(i + j) % n];
                nums[(i + j) % n] = nums[i];
                nums[i] = temp;
            }

            return nums;
        }
    }
}
