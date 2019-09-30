namespace Volo.Abp.Nacos.Configuration
{
    /// <summary>nacos配置选项</summary>
    public class NacosConfigOptions
    {
        /// <summary>Tenant information. It corresponds to the Namespace field in Nacos.</summary>
        public string Tenant { get; set; }

        /// <summary>Configuration ID</summary>
        public string DataId { get; set; }

        /// <summary>Configuration group</summary>
        public string Group { get; set; }
    }
}