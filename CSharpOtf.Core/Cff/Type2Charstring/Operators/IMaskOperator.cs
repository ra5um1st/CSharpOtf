using System.Collections.Generic;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public interface IMask
    {
        IReadOnlyList<byte> Mask { get; set; }
        IReadOnlyList<MaskGroup> MaskGroups { get; }
    }

    public readonly struct MaskGroup
    {
        public IStem Stem { get; }
        public bool IsActive { get; }

        public MaskGroup(IStem stem, bool isActive)
        {
            Stem = stem;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"{GetType().Name} {nameof(IsActive)}: {IsActive} {Stem}";
        }
    }
}