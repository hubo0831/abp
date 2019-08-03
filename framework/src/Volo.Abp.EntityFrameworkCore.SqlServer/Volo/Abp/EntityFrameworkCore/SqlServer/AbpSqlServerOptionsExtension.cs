
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    public class AbpSqlServerOptionsExtension : SqlServerOptionsExtension
    {
        public AbpSqlServerOptionsExtension(DbConnectionOptions dbConnectionOptions)
        {
            this.DbConnectionOptions = dbConnectionOptions;
        }

        public DbConnectionOptions DbConnectionOptions { get; }

        public override bool ApplyServices(IServiceCollection services)
        {
            new EntityFrameworkRelationalServicesBuilder(services)
                .TryAdd<DbConnectionOptions>(this.DbConnectionOptions)
                .TryAdd<IRelationalDatabaseCreator, AbpSqlServerDatabaseCreator>();
            base.ApplyServices(services);
            return true;
        }
    }
}