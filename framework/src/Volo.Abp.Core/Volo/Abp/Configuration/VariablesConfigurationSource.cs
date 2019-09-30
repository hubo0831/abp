using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Configuration
{
    /// <summary>变量配置源</summary>
    public class VariablesConfigurationSource : IConfigurationSource
    {
        /// <summary>配置</summary>
        public IConfiguration Configuration { get; set; }
        /// <summary>生成</summary>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VariablesConfigurationProvider() { Configuration = Configuration };
        }
    }
}
