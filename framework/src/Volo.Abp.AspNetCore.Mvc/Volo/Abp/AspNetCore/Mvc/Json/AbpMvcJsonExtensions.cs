using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Serialization;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public static class AbpMvcJsonExtensions
    {
        /// <summary>获得JSON对象解析器</summary>
        public static IContractResolver GetMvcJsonContractResolver(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IOptions<MvcJsonOptions>>().Value.SerializerSettings.ContractResolver;
        }
        /// <summary>获得JSON对象契约</summary>
        public static JsonObjectContract GetObjectContract<T>(this IContractResolver contractResolver)
        {
            return contractResolver.ResolveContract(typeof(T)).As<JsonObjectContract>();
        }
    }
}
