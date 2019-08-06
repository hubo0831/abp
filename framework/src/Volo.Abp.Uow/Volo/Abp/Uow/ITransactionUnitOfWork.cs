using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ITransactionUnitOfWork
    {
        void BeginTransaction(IUnitOfWorkOptions options);

        void CommitTransactions();

        Task CommitTransactionsAsync(CancellationToken cancellationToken = default);
    }
}
