namespace Volo.Abp.BackgroundJobs.Quartz
{
    /// <summary>Quartz常数</summary>
    public static class AbpQuartzConsts
    {
        /// <summary>常数前缀</summary>
        public const string Prefix = "Abp.Quartz";
        /// <summary>任务组名</summary>
        public const string JobGroup = Prefix + ".JobGroup";
        /// <summary>触发器组名</summary>
        public const string TriggerGroup = Prefix + ".TriggerGroup";
        /// <summary>实时任务</summary>
        public const string RealTimeJob = Prefix + ".RealTimeJob";
        /// <summary>调度任务</summary>
        public const string ScheduleJob = Prefix + ".ScheduleJob";
    }
}
