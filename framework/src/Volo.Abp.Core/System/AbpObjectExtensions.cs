using System.Globalization;
using System.Linq;

namespace System
{
    /// <summary>
    /// Extension methods for all objects.
    /// </summary>
    public static class AbpObjectExtensions
    {
        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        /// <summary>强类型克隆</summary>
        public static T TypedClone<T>(this T value)
            where T : class
        {
            return value?.As<ICloneable>().Clone().As<T>();
        }
        /// <summary>对象转换</summary>
        public static T ConvertTo<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>转换为整数</summary>
        public static int ToInt32<T>(this T value)
            where T : IConvertible
        {
            return Convert.ToInt32(value);
        }

        /// <summary>生成紧缩字符串</summary>
        public static string ToCompactString(this Guid value)
        {
            return value.ToString("N");
        }
    }
}
