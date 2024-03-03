using System;
using System.Data;
using System.Linq;
using System.Reflection;
using OpenGL.TextDrawing.Extensions;
using OpenGL.TextDrawing.StructByteReader;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public static class StructByteReader<TTarget> where TTarget : new()
        {
            public static TTarget Read(byte[] bytes, ref int offset, StructByteReaderConfig config = null)
            {
                return Read(bytes, ref offset, new TTarget(), config);
            }

            public static TTarget Read(byte[] bytes, ref int offset, TTarget @struct, StructByteReaderConfig config = null)
            {
                config ??= StructByteReaderConfig.Default;

                var members = typeof(TTarget).GetMembers().Where(item => 
                    item.MemberType == MemberTypes.Field || 
                    item.MemberType == MemberTypes.Property);

                foreach (var member in members)
                {
                    var ignoreAttribute = member.GetCustomAttribute<StructReaderIgnoreAttribute>();
                    var dynamicOffsetAttribute = member.GetCustomAttribute<DynamicOffsetAttribute>();

                    if (ignoreAttribute != null)
                    {
                        continue;
                    }

                    if (dynamicOffsetAttribute != null)
                    {
                        var dynamicOffset = Activator.CreateInstance(dynamicOffsetAttribute.DynamycOffsetType);
                        offset += (int)dynamicOffsetAttribute.DynamycOffsetType.GetMethod(nameof(IDynamicOffset.GetOffset)).Invoke(dynamicOffset, new object[] { @struct });
                    }

                    var value = ReadMemberValue(bytes, ref offset, @struct, config, member);
                    member.SetValue(ref @struct, value);
                }

                return @struct;
            }

            private static object ReadMemberValue(byte[] bytes, ref int offset, TTarget target, StructByteReaderConfig config, MemberInfo member) 
            {
                var memberType = member.GetMemberType();

                var fixedArrayLengthAttribute = member.GetCustomAttribute<FixedArrayLengthAttribute>();
                var dynamicArrayLengthAttribute = member.GetCustomAttribute<DynamicArrayLengthAttribute>();
                var elementByteReaderAttribute = member.GetCustomAttribute<ElementByteReaderAttribute>();

                if (memberType.IsArray && (fixedArrayLengthAttribute != null || dynamicArrayLengthAttribute != null))
                {
                    var arrayLength = GetArrayLengthFromAttributes(target, fixedArrayLengthAttribute, dynamicArrayLengthAttribute);
                    return ReadMemberArray(target, bytes, ref offset, arrayLength, memberType, elementByteReaderAttribute, config);
                }
                else
                {
                    return ReadMember(target, bytes, ref offset, memberType, elementByteReaderAttribute, config);
                }
            }

            private static object ReadMember(
                TTarget @struct,
                byte[] bytes, 
                ref int offset, 
                Type memberType,
                ElementByteReaderAttribute elementByteReaderAttribute,
                StructByteReaderConfig config)
            {
                object value;
                var elementType = memberType;

                if (elementByteReaderAttribute != null)
                {
                    value = ReadWithElementByteReaderAttribute(bytes, ref offset, @struct, memberType, elementByteReaderAttribute);
                }
                else if (elementType.IsValueType && elementType.IsPrimitive)
                {
                    // Read clr type
                    value = ReadPrimitive(bytes, ref offset, config, memberType);
                }
                else
                {
                    // Read class or struct
                    value = ReadStruct(bytes, ref offset, memberType);
                }

                return value;
            }

            private static object ReadPrimitive(byte[] bytes, ref int offset, StructByteReaderConfig config, Type memberType)
            {
                object value;
                if (config.IsBigEndian)
                {
                    value = InvokeStaticRead(bytes, ref offset, typeof(BigEndianPrimitiveReader<>), memberType);
                }
                else
                {
                    throw new NotImplementedException();
                }

                return value;
            }

            private static object ReadMemberArray(
                TTarget target, 
                byte[] bytes, 
                ref int offset, 
                int arrayLength, 
                Type memberType, 
                ElementByteReaderAttribute elementByteReaderAttribute,
                StructByteReaderConfig config)
            {
                var elementType = memberType.GetElementType();

                if (elementByteReaderAttribute != null)
                {
                    var reader = CreateByteReaderFromAttribute(bytes, ref offset, target, memberType, elementByteReaderAttribute);
                    var genericElementType = elementByteReaderAttribute.ElementByteReaderType
                        .GetInterfaces()
                        .First(item => item.GetGenericTypeDefinition() == typeof(IElementByteReader<,>))
                        .GetGenericArguments()
                        .First();

                    if (genericElementType.IsArray)
                    {
                        return ReadElement(target, bytes, ref offset, elementByteReaderAttribute.ElementByteReaderType, reader);
                    }
                    else
                    {
                        return ReadElementArray(target, bytes, ref offset, arrayLength, elementByteReaderAttribute.ElementByteReaderType, elementType, reader);
                    }

                }
                else if (elementType.IsValueType && elementType.IsPrimitive)
                {
                    if (elementType.IsPrimitive)
                    {
                        // Read clr type array
                        return ReadPrimitiveArray(bytes, ref offset, arrayLength, elementType, config);
                    }
                    else
                    {
                        return ReadStructArray(bytes, ref offset, arrayLength, elementType);
                    }
                }
                else
                {
                    // Read array with custom reader
                    return ReadStructArray(bytes, ref offset, arrayLength, elementType);
                }
            }

            private static object ReadStruct(byte[] bytes, ref int offset, Type memberType)
            {
                object value;
                var args = new object[] { bytes, offset, null };
                value = typeof(StructByteReader<>).MakeGenericType(memberType)
                    .GetMethod(nameof(StructByteReader<TTarget>.Read), new Type[] { typeof(byte[]), typeof(int).MakeByRefType(), typeof(StructByteReaderConfig) })
                    .Invoke(null, args);
                offset = (int)args[1];
                return value;
            }

            private static object InvokeStaticRead(byte[] bytes, ref int offset, Type readerType, Type memberType)
            {
                object value;
                var args = new object[] { bytes, offset };
                value = readerType.MakeGenericType(memberType).GetMethod("Read").Invoke(null, args);
                offset = (int)args[1];

                return value;
            }

            private static object ReadStructArray(byte[] bytes, ref int offset, int arrayLength, Type elementType)
            {
                var array = Array.CreateInstance(elementType, arrayLength);

                for (int i = 0; i < arrayLength; i++)
                {
                    var value = ReadStruct(bytes, ref offset, elementType);
                    array.SetValue(value, i);
                }

                return array;
            }

            private static object ReadPrimitiveArray(byte[] bytes, ref int offset, int arrayLength, Type elementType, StructByteReaderConfig config)
            {
                var array = Array.CreateInstance(elementType, arrayLength);

                for (int i = 0; i < arrayLength; i++)
                {
                    var value = ReadPrimitive(bytes, ref offset, config, elementType);
                    array.SetValue(value, i);
                }

                return array;
            }

            private static object ReadElementArray(TTarget target, byte[] bytes, ref int offset, int arrayLength, Type readerType, Type elementType, object reader)
            {
                var array = Array.CreateInstance(elementType, arrayLength);

                for (int i = 0; i < arrayLength; i++)
                {
                    var value = ReadElement(target, bytes, ref offset, readerType, reader);
                    array.SetValue(value, i);
                }

                return array;
            }

            private static int GetArrayLengthFromAttributes(TTarget @struct, FixedArrayLengthAttribute fixedArrayLengthAttribute, DynamicArrayLengthAttribute dynamicArrayLengthAttribute)
            {
                return fixedArrayLengthAttribute?.GetArrayLength(@struct) ?? dynamicArrayLengthAttribute.GetArrayLength(@struct);
            }

            private static object ReadWithElementByteReaderAttribute(byte[] bytes, ref int offset, TTarget @struct, Type memberType, ElementByteReaderAttribute elementByteReaderAttribute)
            {
                // TODO: сделать кэш для reader ов.
                var reader = CreateByteReaderFromAttribute(bytes, ref offset, @struct, memberType, elementByteReaderAttribute);
                var value = ReadElement(@struct, bytes, ref offset, elementByteReaderAttribute.ElementByteReaderType, reader);

                return value;
            }

            private static object CreateByteReaderFromAttribute(byte[] bytes, ref int offset, TTarget @struct, Type memberType, ElementByteReaderAttribute elementByteReaderAttribute)
            {
                var readerType = elementByteReaderAttribute.ElementByteReaderType;
                var reader = Activator.CreateInstance(elementByteReaderAttribute.ElementByteReaderType);

                return reader;
            }

            private static object ReadElement(TTarget target, byte[] bytes, ref int offset, Type readerType, object elementReader)
            {
                object value;

                var readMethod = readerType.GetMethod("Read", BindingFlags.Public | BindingFlags.Instance);
                var args = new object[] { target, bytes, offset };
                value = readMethod.Invoke(elementReader, args);
                offset = (int)args[2];

                return value;
            }
        }
    }
}