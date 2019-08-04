using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    /// <summary>SQLServer生成数据库</summary>
    public class AbpSqlServerDatabaseCreator : SqlServerDatabaseCreator, IRelationalDatabaseCreator
    {
        /// <summary>SQLServer生成数据库</summary>
        public AbpSqlServerDatabaseCreator(IDbContextServices contextServices, RelationalDatabaseCreatorDependencies dependencies, ISqlServerConnection connection, IRawSqlCommandBuilder rawSqlCommandBuilder)
                : base(dependencies, connection, rawSqlCommandBuilder)
        {
            this.RawConnection = connection;
            this.AttachDBFilename = GetAttachDBFilename(contextServices);
        }
        /// <summary>原始连接</summary>
        private ISqlServerConnection RawConnection { get; }
        /// <summary>新数据库路径</summary>
        private string AttachDBFilename { get; }

        /// <summary>获得新数据库路径</summary>
        private static string GetAttachDBFilename(IDbContextServices contextServices)
        {
            var sp = contextServices.ContextOptions.FindExtension<CoreOptionsExtension>().ApplicationServiceProvider;
            if (sp == null) return null;
            var dbConnectionOptions = sp.GetRequiredService<IOptions<DbConnectionOptions>>().Value;
            return dbConnectionOptions.NewDatabasePath;
        }
        /// <summary>创建新数据库</summary>
        public override void Create()
        {
            if (this.AttachDBFilename.IsNullOrEmpty())
            {
                base.Create();
                return;
            }
            using (var masterConnection = this.RawConnection.CreateMasterConnection())
            {
                Dependencies.MigrationCommandExecutor.ExecuteNonQuery(CreateCreateOperations(), masterConnection);
                ClearPool();
            }
            Exists2(retryOnNotExists: true);
        }
        /// <summary>创建新数据库</summary>
        public override async Task CreateAsync(CancellationToken cancellationToken = default)
        {
            if (this.AttachDBFilename.IsNullOrEmpty())
            {
                await base.CreateAsync(cancellationToken);
                return;
            }
            using (var masterConnection = this.RawConnection.CreateMasterConnection())
            {
                await Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(CreateCreateOperations(), masterConnection, cancellationToken);
                ClearPool();
            }
            await Exists2Async(retryOnNotExists: true, cancellationToken);
        }
        /// <summary>创建创建数据库路径的操作</summary>
        private IReadOnlyList<MigrationCommand> CreateCreateOperations()
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.RawConnection.DbConnection.ConnectionString);
            return Dependencies.MigrationsSqlGenerator.Generate(new SqlServerCreateDatabaseOperation[1]
            {
                new SqlServerCreateDatabaseOperation
                {
                    Name = sqlConnectionStringBuilder.InitialCatalog,
                    FileName = this.AttachDBFilename
                }
            });
        }
        /// <summary>清除连接池</summary>
        private void ClearPool()
        {
            SqlConnection.ClearPool((SqlConnection)this.RawConnection.DbConnection);
        }
        /// <summary>是否存在数据库</summary>
        private bool Exists2(bool retryOnNotExists)
        {
            var method = this.GetType().BaseType.GetMethod("Exists", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(bool) }, null);
            return (bool)method.Invoke(this, new object[] { retryOnNotExists });
        }
        /// <summary>是否存在数据库</summary>
        private Task<bool> Exists2Async(bool retryOnNotExists, CancellationToken cancellationToken)
        {
            var method = this.GetType().BaseType.GetMethod("ExistsAsync", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(bool), typeof(CancellationToken) }, null);
            return (Task<bool>)method.Invoke(this, new object[] { retryOnNotExists, cancellationToken });
        }
    }
}