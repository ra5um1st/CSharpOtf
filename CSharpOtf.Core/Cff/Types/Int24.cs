namespace OpenGL.TextDrawing.Cff
{
    public struct Int24
    {
        private readonly byte _b0;
        private readonly byte _b1;
        private readonly byte _b2;

        public Int24(int value)
        {
            _b0 = (byte)(value & 0xFF);
            _b1 = (byte)(value >> 8 & 0xFF);
            _b2 = (byte)(value >> 16 & 0xFF);
        }

        public static implicit operator int(Int24 self)
        {
            return self._b0 | self._b1 << 8 | self._b2 << 16;
        }

        public static implicit operator Int24(int value)
        {
            return new Int24(value);
        }

        public static int GetSize()
        {
            return sizeof(byte) * 3;
        }

        public override string ToString()
        {
            return ((int)this).ToString();
        }
    }
}