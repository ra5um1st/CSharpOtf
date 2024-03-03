using System;
using System.Reflection;

namespace OpenGL.TextDrawing.Extensions
{
    public static class MemberInfoExtensions
    {
        public static void SetValue<TTarget, TValue>(this MemberInfo memberInfo, ref TTarget target, TValue value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValueDirect(__makeref(target), value);
                    break;
                case MemberTypes.Property:
                    object boxed = target;
                    ((PropertyInfo)memberInfo).SetValue(boxed, value, null);
                    target = (TTarget)boxed;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public static object GetValue<TTarget>(this MemberInfo memberInfo, ref TTarget target)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValueDirect(__makeref(target));
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(target);
                default:
                    throw new NotSupportedException();
            }
        }

        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
