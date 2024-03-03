using System;
using System.Data;
using System.Linq;
using OpenGL.TextDrawing.Extensions;

namespace OpenGL.TextDrawing
{
    public class DynamicArrayLengthAttribute : ArrayLengthAttribute
    {
        public int AdditionalLength { get; set; }
        public string ArrayLengthMemberName { get; }

        public DynamicArrayLengthAttribute(string arrayLengthMemberName)
        {
            ArrayLengthMemberName = arrayLengthMemberName;
        }

        public override int GetArrayLength(object target)
        {
            var arrayLength = target.GetType()
               .GetMember(ArrayLengthMemberName)
               .Select(item => item.GetValue(ref target))
               .First();

            return Convert.ToInt32(arrayLength) + AdditionalLength;
        }
    }
}