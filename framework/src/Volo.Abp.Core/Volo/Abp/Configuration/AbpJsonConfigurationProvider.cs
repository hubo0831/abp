using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration.Json;

namespace Volo.Abp.Configuration
{
    /// <summary>JSON配置提供者</summary>
    public class AbpJsonConfigurationProvider : JsonConfigurationProvider
    {
        /// <summary>JSON配置提供者</summary>
        public AbpJsonConfigurationProvider() : base(new JsonConfigurationSource() { ReloadOnChange = false }) { }
        /// <summary>加载</summary>
        public IDictionary<string, string> Load(string content)
        {
            var stream = new MemoryStream(content.Length * 2);
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 256, true))
            {
                streamWriter.Write(content);
            }
            stream.Position = 0;
            Load(stream);
            return this.Data;
        }
    }
}
