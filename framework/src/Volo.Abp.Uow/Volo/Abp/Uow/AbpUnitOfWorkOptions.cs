using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class AbpUnitOfWorkOptions : IAbpUnitOfWorkOptions
    {
        public static AbpUnitOfWorkOptions Default { get; } = new AbpUnitOfWorkOptions();

        /// <summary>
        /// Default: false.
        /// </summary>
        public bool IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 是否使用父的事务环境
        /// </summary>
        public bool UseParentTransaction { get; set; }

        public AbpUnitOfWorkOptions Clone()
        {
            return new AbpUnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout
            };
        }
    }
}