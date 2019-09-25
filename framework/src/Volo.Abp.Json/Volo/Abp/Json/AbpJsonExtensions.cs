using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.Json
{
    public static class AbpJsonExtensions
    {
        /// <summary>实体转换为JSON</summary>
        public static string ToJson(this object value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>JSON转换为实体</summary>
        public static T JsonTo<T>(this string value, JsonSerializerSettings settings = null)
        {
            return (T)JsonConvert.DeserializeObject<T>(value, settings);
        }
        /// <summary>将JToken转化成.NET类型</summary>
        public static object ToObject(this JToken jToken)
        {
            if (jToken == null) return null;
            object result = null;
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    result = ToDictionary(jToken.As<JObject>());
                    break;
                case JTokenType.Array:
                    result = ToArry(jToken.As<JArray>());
                    break;
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.None:
                case JTokenType.Comment:
                case JTokenType.Raw:
                    break;
                default:
                    result = jToken.As<JValue>().Value;
                    break;
            }
            return result;
        }
        /// <summary>将JObject转化成字典</summary>
        public static Dictionary<string, object> ToDictionary(this JObject jObject)
        {
            var result = new Dictionary<string, object>(StringComparer.Ordinal);
            foreach (var pair in jObject)
            {
                var value = ToObject(pair.Value);
                result.Add(pair.Key, value);
            }
            return result;
        }
        /// <summary>将JArray转化成数组</summary>
        public static object[] ToArry(this JArray jArray)
        {
            var result = new object[jArray.Count];
            for (int i = 0; i < jArray.Count; i++)
            {
                var value = ToObject(jArray[i]);
                result[i] = value;
            }
            return result;
        }
    }
}