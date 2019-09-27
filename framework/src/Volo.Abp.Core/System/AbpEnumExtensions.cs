namespace System
{
    /// <summary>
    /// Extension methods for enum.
    /// </summary>
    public static class AbpEnumExtensions
    {
        /// <summary>整数转换为枚举</summary>
        public static T ToEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}