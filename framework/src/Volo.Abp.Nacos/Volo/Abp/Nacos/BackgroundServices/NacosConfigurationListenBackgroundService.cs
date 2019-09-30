using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Nacos;

using Volo.Abp.Nacos.Configuration;

namespace Volo.Abp.Nacos.BackgroundServices
{
    /// <summary>nacos配置侦听后台服务</summary>
    public class NacosConfigurationListenBackgroundService : BackgroundService
    {
        /// <summary>日志</summary>
        private readonly NacosConfigurationProvider _configProvider;
        /// <summary>日志</summary>
        private readonly ILogger _logger;
        /// <summary>nacos配置客户端</summary>
        private readonly NacosConfigOptions _configOptions;
        /// <summary>nacos配置客户端</summary>
        private readonly INacosConfigClient _configClient;
        /// <summary>nacos配置侦听后台服务</summary>
        public NacosConfigurationListenBackgroundService(NacosConfigurationSource configurationSource, IOptions<NacosConfigOptions> options, INacosConfigClient configClient, ILoggerFactory loggerFactory)
        {
            _configProvider = configurationSource.Provider;
            _configOptions = options.Value;
            _configClient = configClient;
            _logger = loggerFactory.CreateLogger<NacosConfigurationListenBackgroundService>();
        }
        /// <summary>执行</summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _configClient.AddListenerAsync(new AddListenerRequest
            {
                DataId = _configOptions.DataId,
                Group = _configOptions.Group,
                Tenant = _configOptions.Tenant,
                Callbacks = new List<Action<string>> { OnListenCallback }
            });
        }
        /// <summary>侦听回调</summary>
        private void OnListenCallback(string content)
        {
            _logger.LogInformation("nacos config changed");
            //_logger.LogInformation("nacos config changed:{content}", content);
            _configProvider.Reload(content);
        }
        /// <summary>停止</summary>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _configClient.RemoveListenerAsync(new RemoveListenerRequest
            {
                DataId = _configOptions.DataId,
                Group = _configOptions.Group,
                Tenant = _configOptions.Tenant,
                Callbacks = new List<Action> { OnRemoveListenCallback }
            });
            await base.StopAsync(cancellationToken);
        }
        /// <summary>侦听回调</summary>
        private void OnRemoveListenCallback()
        {
            _logger.LogInformation("listerner removed");
        }
    }
}
