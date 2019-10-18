using System;
using System.Collections.Specialized;

using Microsoft.Extensions.Configuration;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    /// <summary>Quartz选项</summary>
    public class AbpQuartzOption
    {
        public Scheduler Scheduler { get; set; }

        public ThreadPool ThreadPool { get; set; }

        public Plugin Plugin { get; set; }

        public NameValueCollection ToProperties()
        {
            var properties = new NameValueCollection();
            if (this.Scheduler != null)
            {
                AddProperty(properties, "quartz.scheduler.instanceName", this.Scheduler.InstanceName);
            }
            if (this.ThreadPool != null)
            {
                AddProperty(properties, "quartz.threadPool.type", this.ThreadPool.Type);
                AddProperty(properties, "quartz.threadPool.threadCount", this.ThreadPool.ThreadCount.ToString());
            }
            if (this.Plugin != null)
            {
                AddProperty(properties, "quartz.plugin.jobInitializer.type", this.Plugin.JobInitializer?.Type);
                AddProperty(properties, "quartz.plugin.jobInitializer.fileNames", this.Plugin.JobInitializer?.FileNames);
            }
            return properties;
        }
        private void AddProperty(NameValueCollection properties, string key, string property)
        {
            if (property.IsNullOrEmpty()) return;
            properties[key] = property;
        }
    }

    public class Scheduler
    {
        public string InstanceName { get; set; }
    }

    public class ThreadPool
    {
        public string Type { get; set; }

        public int ThreadCount { get; set; }
    }

    public class Plugin
    {
        public JobInitializer JobInitializer { get; set; }
    }

    public class JobInitializer
    {
        public string Type { get; set; }
        public string FileNames { get; set; }
    }
}
