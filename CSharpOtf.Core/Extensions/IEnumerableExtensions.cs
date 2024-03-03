using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL.TextDrawing
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> RemoveLast<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Take(enumerable.Count() - 1);
        }
    }
}
