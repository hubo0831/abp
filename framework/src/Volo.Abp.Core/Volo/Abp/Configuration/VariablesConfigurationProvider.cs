using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
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
            this.Builders = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
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
        private Dictionary<string, Func<string, string>> Builders { get; }
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
                if (!value.IsNullOrEmpty() && !value.StartsWith("$")) continue;
                var builder = this.Builders.GetOrDefault(key);
                if (builder == null) continue;
                Set(fullKey, builder(value));
            }
        }
        /// <summary>获得默认服务名</summary>
        private string GetServiceName(string value)
        {
            return this.Configuration[HostDefaults.ApplicationKey];
        }
        /// <summary>默认主机IP</summary>
        private string HostIP { get; set; }
        /// <summary>默认主机端口</summary>
        private string HostPort { get; set; }
        /// <summary>获得默认主机IP</summary>
        private string GetHostIP(string value)
        {
            if (this.HostIP == null)
            {
                ResolveServerUrl(value);
            }
            return this.HostIP;
        }
        /// <summary>获得默认主机端口</summary>
        private string GetHostPort(string value)
        {
            if (this.HostPort == null)
            {
                ResolveServerUrl(value);
            }
            return this.HostPort;
        }
        /// <summary>解析服务URL</summary>
        private void ResolveServerUrl(string value)
        {
            this.HostIP = string.Empty;
            this.HostPort = string.Empty;
            var urls = this.Configuration[ServerUrlsKey]?.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (urls.IsNullOrEmpty()) return;
            foreach (var url in urls)
            {
                var match = Regex.Match(url, "https?://(?<ip>[^:]+):(?<port>\\d+)");
                if (!match.Success) continue;
                this.HostIP = match.Groups["ip"].Value;
                this.HostPort = match.Groups["port"].Value;
                if (this.HostIP == "*")
                {
                    var hostName = Dns.GetHostName();
                    var ipAddresses = Dns.GetHostAddresses(hostName);
                    if (value != null && value.StartsWith("$"))
                    {
                        var ipPrefix = value.Substring(1);
                        foreach (var ipAddress in ipAddresses)
                        {
                            var ip = ipAddress.ToString();
                            if (!ip.StartsWith(ipPrefix)) continue;
                            this.HostIP = ip;
                            break;
                        }
                    }
                    else
                    {
                        var first = ipAddresses.FirstOrDefault(e => e.AddressFamily == AddressFamily.InterNetwork);
                        if (first != null) this.HostIP = first.ToString();
                    }
                }
                break;
            }
        }
        /// <summary>获得处理器数量</summary>
        private string GetProcessorCount(string value)
        {
            return Environment.ProcessorCount.ToString();
        }
    }
}
