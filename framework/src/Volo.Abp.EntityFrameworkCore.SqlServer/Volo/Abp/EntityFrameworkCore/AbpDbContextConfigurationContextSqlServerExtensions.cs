using System;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp.Data;
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
            var dbConnectionOptions = context.ServiceProvider.GetRequiredService<IOptions<DbConnectionOptions>>().Value;
            if (!dbConnectionOptions.NewDatabasePath.IsNullOrEmpty())
            {
                context.DbContextOptions.Options.WithExtension(new AbpSqlServerOptionsExtension(dbConnectionOptions));
            }
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
