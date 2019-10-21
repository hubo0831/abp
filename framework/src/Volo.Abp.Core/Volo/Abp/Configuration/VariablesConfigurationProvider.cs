using System;
using System.Collections.Generic;
using System.Net;
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
                {"ProcessorCount",  GetProcessorCount},
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
        /// <summary>默认主机IP</summary>
        private string HostIP { get; set; }
        /// <summary>默认主机端口</summary>
        private string HostPort { get; set; }
        /// <summary>获得默认主机IP</summary>
        private string GetHostIP()
        {
            if (this.HostIP == null)
            {
                ResolveServerUrl();
            }
            return this.HostIP;
        }
        /// <summary>获得默认主机端口</summary>
        private string GetHostPort()
        {
            if (this.HostPort == null)
            {
                ResolveServerUrl();
            }
            return this.HostPort;
        }
        /// <summary>解析服务URL</summary>
        private void ResolveServerUrl()
        {
            this.HostIP = string.Empty;
            this.HostPort = string.Empty;
            var urls = this.Configuration[ServerUrlsKey]?.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (urls.IsNullOrEmpty()) return;
            foreach (var url in urls)
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var result)) continue;
                this.HostIP = result.Host;
                this.HostPort = result.Port.ToString();
                break;
            }
        }
        /// <summary>获得处理器数量</summary>
        private string GetProcessorCount()
        {
            return Environment.ProcessorCount.ToString();
        }
    }
}
