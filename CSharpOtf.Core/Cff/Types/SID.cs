using System;
using System.Linq;

namespace OpenGL.TextDrawing.Cff
{
    public readonly struct SID
    {
        public ushort Value { get; }

        public SID(ushort value)
        {
            Value = value;
        }

        public SID(int value)
        {
            Value = Convert.ToUInt16(value);
        }

        public string ToString(Cff cff)
        {
            return Value > StandartStrings.Table.Count - 1 ? cff.StringIndex.Data[Value - StandartStrings.Table.Count] : ToString();
        }

        public override string ToString()
        {
            return StandartStrings.Table.TryGetValue(this, out var value) ? value : "Unknown SID";
        }

        public static implicit operator string(SID sid)
        {
            return sid.ToString();
        }

        public static implicit operator ushort(SID sid)
        {
            return sid.Value;
        }

        public static implicit operator int(SID sid)
        {
            return sid.Value;
        }

        public static implicit operator SID(int value)
        {
            return new SID(value);
        }

        public static implicit operator SID(CffFloat value)
        {
            return new SID(value);
        }
    }
}