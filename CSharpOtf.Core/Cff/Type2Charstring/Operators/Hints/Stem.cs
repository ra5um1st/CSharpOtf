using System;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public abstract class Stem : ClearArgumentStackOperator, IStem
    {
        public abstract bool IsVertical { get; }
        public T2Float Stem1 { get; private set; }
        public T2Float Stem2 { get; private set; }

        public override void Execute(T2ProgramContext context)
        {
            context.TryDefineCharstringWidth(true);

            var stemsCount = context.ArgumentStack.Count / 2;
            var lastStem = new T2Float();

            for (int i = 0; i < stemsCount; i++)
            {
                var stem = (Stem)Activator.CreateInstance(GetType());
                stem.Stem1 = context.ArgumentStack.Dequeue() + lastStem;
                stem.Stem2 = context.ArgumentStack.Dequeue() + stem.Stem1;
                lastStem = stem.Stem2;
                context.Stems.Add(stem);
            }

            base.Execute(context);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {Stem1} {Stem2}";
        }
    }
}
