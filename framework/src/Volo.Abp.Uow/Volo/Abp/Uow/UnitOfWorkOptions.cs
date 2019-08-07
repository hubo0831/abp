using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkOptions : IUnitOfWorkOptions
    {
        public static UnitOfWorkOptions Default => new UnitOfWorkOptions();
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

        public UnitOfWorkOptions Clone()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                UseParentTransaction = UseParentTransaction
            };
        }
    }
}