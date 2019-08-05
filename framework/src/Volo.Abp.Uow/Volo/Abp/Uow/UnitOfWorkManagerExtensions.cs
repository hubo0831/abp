using System.Transactions;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkManagerExtensions
    {
        [NotNull]
        public static IUnitOfWork Begin([NotNull] this IUnitOfWorkManager unitOfWorkManager, bool requiresNew = false)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

            return unitOfWorkManager.Begin(new UnitOfWorkOptions(), requiresNew);
        }

        public static void BeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.BeginReserved(reservationName, new UnitOfWorkOptions());
        }

        public static void TryBeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.TryBeginReserved(reservationName, new UnitOfWorkOptions());
        }
        public static TransactionScope BeginUowTransactionScope(this IUnitOfWork unitOfWork, IUnitOfWorkOptions options)
        {
            var transactionOptions = new TransactionOptions();
            if (options.Timeout.HasValue) transactionOptions.Timeout = options.Timeout.Value;
            if (options.IsolationLevel.HasValue) transactionOptions.IsolationLevel = GetIsolationLevel(options.IsolationLevel.Value);
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);
        }

        private static IsolationLevel GetIsolationLevel(System.Data.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Data.IsolationLevel.ReadCommitted: return IsolationLevel.ReadCommitted;
                case System.Data.IsolationLevel.Serializable: return IsolationLevel.Serializable;
                case System.Data.IsolationLevel.Snapshot: return IsolationLevel.Snapshot;
                case System.Data.IsolationLevel.RepeatableRead: return IsolationLevel.RepeatableRead;
                case System.Data.IsolationLevel.ReadUncommitted: return IsolationLevel.ReadUncommitted;
                case System.Data.IsolationLevel.Chaos: return IsolationLevel.Chaos;
                default: return IsolationLevel.Unspecified;
            }
        }
    }
}