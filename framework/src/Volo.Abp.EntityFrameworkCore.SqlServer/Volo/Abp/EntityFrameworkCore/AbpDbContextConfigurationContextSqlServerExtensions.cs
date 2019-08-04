using System;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextConfigurationContextSqlServerExtensions
    {
        public static DbContextOptionsBuilder UseSqlServer(
            [NotNull] this AbpDbContextConfigurationContext context,
            [CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            context.DbContextOptions.UseApplicationServiceProvider(context.ServiceProvider);
            context.DbContextOptions.ReplaceService<IRelationalDatabaseCreator, AbpSqlServerDatabaseCreator>();
            if (context.ExistingConnection != null)
            {
                return context.DbContextOptions.UseSqlServer(context.ExistingConnection, sqlServerOptionsAction);
            }
            else
            {
                return context.DbContextOptions.UseSqlServer(context.ConnectionString, sqlServerOptionsAction);
            }
        }
    }
}
