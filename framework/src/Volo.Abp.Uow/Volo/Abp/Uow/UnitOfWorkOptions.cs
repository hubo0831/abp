using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkOptions : IUnitOfWorkOptions
    {
        /// <summary>
        /// Default: false.
        /// </summary>
        public bool IsTransactional { get; set; }

        /// <summary>
        /// Is this UOW use TransactionScope?
        /// </summary>
        public bool UseTransactionScope { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        public UnitOfWorkOptions Clone()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                UseTransactionScope = UseTransactionScope,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout
            };
        }
    }
}