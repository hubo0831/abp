using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.Json
{
    public static class AbpJsonExtensions
    {
        /// <summary>序列化为JSON</summary>
        public static string ToJson(this object value, bool indented = false)
        {
            var settings = indented ? new JsonSerializerSettings() { Formatting = Formatting.Indented } : null;
            return ToJson(value, settings);
        }
        /// <summary>实体转换为JSON</summary>
        public static string ToJson(this object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>从JSON反序列化为JToken</summary>
        public static JToken JsonTo(this string value, JsonSerializerSettings settings = null)
        {
            return (JToken)JsonConvert.DeserializeObject(value, settings);
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
        /// <summary>尝试解析JSON</summary>
        public static bool TryParseJson<T>(this string value, out T result, ILogger logger = null)
            where T : class
        {
            result = default;
            if (TryParseJson(value, typeof(T), out var result2, logger))
            {
                result = result2.As<T>();
                return true;
            }
            return false;
        }
        /// <summary>尝试解析JSON</summary>
        public static bool TryParseJson(this string value, Type resultType, out object result, ILogger logger = null)
        {
            result = null;
            if (value.IsNullOrEmpty()) return false;
            value = value.TrimStart();
            if (value.Length == 0) return false;
            var start = value[0];
            if (start != '{' && start != '[') return false;
            try
            {
                result = JsonConvert.DeserializeObject(value, resultType);
                return true;
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, ex.Message);
                return false;
            }
        }
    }
}