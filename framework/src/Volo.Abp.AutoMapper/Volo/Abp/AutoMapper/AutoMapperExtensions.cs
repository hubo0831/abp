using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public static class AutoMapperExtensions
    {
        /// <summary>对象映射转换</summary>
        public static T AutoMapTo<T>(this object value)
            where T : class
        {
            return Mapper.Map<T>(value);
        }
        /// <summary>对象映射转换</summary>
        public static void AutoMapTo(this object source, object desc)
        {
            Mapper.Map(source, desc);
        }
    }
}
