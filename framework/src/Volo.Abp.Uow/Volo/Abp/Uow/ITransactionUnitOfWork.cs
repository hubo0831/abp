namespace Volo.Abp.Uow
{
    public interface ITransactionUnitOfWork
    {
        void Begin(IUnitOfWorkOptions options = null);

        void Commit();
    }
}
