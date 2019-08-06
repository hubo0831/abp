using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    internal class ChildUnitOfWork : IUnitOfWork
    {
        public Guid Id => _parent.Id;

        public IUnitOfWorkOptions Options => _parent.Options;

        public IUnitOfWork Outer => _parent.Outer;

        public bool IsReserved => _parent.IsReserved;

        public bool IsDisposed => _parent.IsDisposed;

        public bool IsCompleted => _parent.IsCompleted;

        public string ReservationName => _parent.ReservationName;

        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler<UnitOfWorkEventArgs> Disposed;

        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        private readonly IUnitOfWork _parent;

        private readonly ITransactionUnitOfWork _parentTransaction;

        public ChildUnitOfWork([NotNull] IUnitOfWork parent, IUnitOfWorkOptions options = null)
        {
            Check.NotNull(parent, nameof(parent));

            _parent = parent;

            _parent.Failed += (sender, args) => { Failed.InvokeSafely(sender, args); };
            _parent.Disposed += (sender, args) => { Disposed.InvokeSafely(sender, args); };

            if (options != null && options.IsTransactional)
            {
                while (parent != null)
                {
                    _parentTransaction = parent as ITransactionUnitOfWork;
                    if (_parentTransaction != null) break;
                    parent = parent.Outer;
                }
                _parentTransaction.BeginTransaction(options);
            }
        }

        public void SetOuter(IUnitOfWork outer)
        {
            _parent.SetOuter(outer);
        }

        public void Initialize(UnitOfWorkOptions options)
        {
            _parent.Initialize(options);
        }

        public void Reserve(string reservationName, UnitOfWorkOptions options)
        {
            _parent.Reserve(reservationName, options);
        }

        public void SaveChanges()
        {
            _parent.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _parent.SaveChangesAsync(cancellationToken);
        }

        public void Complete()
        {
            SaveChanges();
            if (_parentTransaction != null) _parentTransaction.CommitTransactions();
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            if (_parentTransaction != null) await _parentTransaction.CommitTransactionsAsync(cancellationToken);
        }

        public void Rollback()
        {
            _parent.Rollback();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _parent.RollbackAsync(cancellationToken);
        }

        public void OnCompleted(Func<Task> handler)
        {
            _parent.OnCompleted(handler);
        }

        public IDatabaseApi FindDatabaseApi(string key)
        {
            return _parent.FindDatabaseApi(key);
        }

        public void AddDatabaseApi(string key, IDatabaseApi api)
        {
            _parent.AddDatabaseApi(key, api);
        }

        public IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
        {
            return _parent.GetOrAddDatabaseApi(key, factory);
        }

        public ITransactionApi FindTransactionApi(string key)
        {
            return _parent.FindTransactionApi(key);
        }

        public void AddTransactionApi(string key, ITransactionApi api)
        {
            _parent.AddTransactionApi(key, api);
        }

        public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
        {
            return _parent.GetOrAddTransactionApi(key, factory);
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}