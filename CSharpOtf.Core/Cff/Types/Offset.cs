namespace OpenGL.TextDrawing.Cff
{
    public struct Offset
    {
        public OffSize OffSize;
        private readonly int _value;

        public Offset(OffSize offSize, int value)
        {
            OffSize = offSize;
            _value = value;
        }

        public static implicit operator CffInt(Offset self)
        {
            return self._value;
        }

        public static implicit operator int(Offset self)
        {
            return self._value;
        }

        public override string ToString()
        {
            return $"{nameof(Offset)} = {_value}";
        }
    }
}
