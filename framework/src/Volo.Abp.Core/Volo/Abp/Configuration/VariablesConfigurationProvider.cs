using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.Configuration
{
    /// <summary>变量配置提供者</summary>
    public class VariablesConfigurationProvider : ConfigurationProvider
    {
        /// <summary>变量配置提供者</summary>
        public VariablesConfigurationProvider()
        {
            this.Builders = new Dictionary<string, Func<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"ApplicationName",  GetServiceName},
                {"ServiceName",  GetServiceName},
                {"HostIP",  GetHostIP},
                {"HostPort",  GetHostPort},
            };
        }
        /// <summary>配置节键</summary>
        public const string SectionKey = "Variables";
        /// <summary>服务器URL键(WebHostDefaults.ServerUrlsKey)</summary>
        private static readonly string ServerUrlsKey = "urls";
        /// <summary>配置节键</summary>
        private Dictionary<string, Func<string>> Builders { get; }
        /// <summary>配置</summary>
        public IConfiguration Configuration { get; set; }
        /// <summary>加载</summary>
        public override void Load()
        {
            var section = this.Configuration.GetSection(SectionKey);
            foreach (var child in section.GetChildren())
            {
                var key = child.Key;
                var fullKey = $"{SectionKey}:{key}";
                var value = this.Configuration[fullKey];
                if (!value.IsNullOrEmpty()) continue;
                var builder = this.Builders.GetOrDefault(key);
                if (builder == null) continue;
                Set(fullKey, builder());
            }
        }
        /// <summary>获得默认服务名</summary>
        private string GetServiceName()
        {
            return this.Configuration[HostDefaults.ApplicationKey];
        }
        /// <summary>获得默认主机IP</summary>
        private string GetHostIP()
        {
            var url = new Uri(this.Configuration[ServerUrlsKey]);
            return url.Host;
        }
        /// <summary>获得默认主机端口</summary>
        private string GetHostPort()
        {
            var url = new Uri(this.Configuration[ServerUrlsKey]);
            return url.Port.ToString();
        }
    }
}
