namespace System
{
    /// <summary>
    /// Extension methods for enum.
    /// </summary>
    public static class AbpEnumExtensions
    {
        /// <summary>����ת��Ϊö��</summary>
        public static T ToEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}