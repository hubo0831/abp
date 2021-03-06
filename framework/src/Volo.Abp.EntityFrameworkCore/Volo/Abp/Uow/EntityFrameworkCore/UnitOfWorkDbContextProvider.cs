using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    //TODO: Implement logic in DefaultDbContextResolver.Resolve in old ABP.

    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
        }

        public TDbContext GetDbContext()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException("A DbContext can only be created inside a unit of work!");
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = _connectionStringResolver.Resolve(connectionStringName);

            var dbContextKey = $"{typeof(TDbContext).FullName}_{connectionString}";

            var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
            TDbContext dbContext;
            if (databaseApi == null)
            {
                dbContext = CreateDbContext(unitOfWork, connectionStringName, connectionString);
                databaseApi = new EfCoreDatabaseApi<TDbContext>(dbContext);
                unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
            }
            else
            {
                dbContext = ((EfCoreDatabaseApi<TDbContext>)databaseApi).DbContext;
                CheckDbContextTransaction(unitOfWork, dbContext, connectionString);
            }

            return dbContext;
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                var dbContext = CreateDbContext(unitOfWork);

                if (unitOfWork.Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(unitOfWork.Options.Timeout.Value.TotalSeconds.To<int>());
                }

                return dbContext;
            }
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? CreateDbContextWithTransaction(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
        }

        public TDbContext CreateDbContextWithTransaction(IUnitOfWork unitOfWork)
        {
            var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbtransaction,
                        dbContext
                    )
                );

                return dbContext;
            }
            else
            {
                DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction(); //TODO: Why not using the new created transaction?
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);

                return dbContext;
            }
        }
        private void CheckDbContextTransaction(IUnitOfWork unitOfWork, TDbContext dbContext, string connectionString)
        {
            if (!unitOfWork.Options.IsTransactional) return;
            if (dbContext.Database.CurrentTransaction != null) return;

            var transactionApiKey = $"EntityFrameworkCore_{connectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(transactionApiKey,
                    new EfCoreTransactionApi(dbtransaction, dbContext)
                );
            }
            else
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction(); //TODO: Why not using the new created transaction?
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);
            }
        }
    }
}