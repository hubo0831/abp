using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Nacos.Configuration
{
    /// <summary>Nacos配置源</summary>
    public class NacosConfigurationSource : IConfigurationSource
    {
        /// <summary>生成</summary>
        public NacosConfigurationProvider Provider { get; private set; }
        /// <summary>生成</summary>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this.Provider = new NacosConfigurationProvider();
        }
    }
}
