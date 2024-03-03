namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public struct T2Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public T2Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public T2Point AddX(float x)
        {
            return new T2Point(X + x, Y );
        }

        public T2Point AddY(float y)
        {
            return new T2Point(X, Y + y);
        }

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }

        public static T2Point operator +(T2Point a, T2Point b)
        {
            return new T2Point(a.X + b.X, a.Y + b.Y);
        }

        public static T2Point operator -(T2Point a, T2Point b)
        {
            a.X -= b.X;
            a.Y -= b.Y;

            return new T2Point(a.X - b.X, a.Y - b.Y);
        }

        public static T2Point operator *(T2Point point, float value)
        {
            return new T2Point(point.X * value, point.Y * value);
        }

        public static bool operator ==(T2Point a, T2Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(T2Point a, T2Point b)
        {
            return a.X != b.X && a.Y != b.Y;
        }
    }
}
