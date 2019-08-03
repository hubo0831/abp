using System;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    /// <summary>SQLServer生成数据库</summary>
    public class AbpSqlServerDatabaseCreator : SqlServerDatabaseCreator, IRelationalDatabaseCreator
    {
        /// <summary>SQLServer生成数据库</summary>
        public AbpSqlServerDatabaseCreator(RelationalDatabaseCreatorDependencies dependencies, ISqlServerConnection connection, IRawSqlCommandBuilder rawSqlCommandBuilder, RelationalConnectionDependencies connectionDependencies, DbConnectionOptions connectionOptions)
                : base(dependencies, AddAttachDBFilename(connectionDependencies, connection, connectionOptions), rawSqlCommandBuilder)
        {
        }
        /// <summary>加入新数据库路径</summary>
        private static ISqlServerConnection AddAttachDBFilename(RelationalConnectionDependencies dependencies, ISqlServerConnection connection, DbConnectionOptions connectionOptions)
        {
            if (connectionOptions.NewDatabasePath.IsNullOrEmpty()) return connection;
            return CreateConnection(dependencies, connection, connectionOptions);
        }
        /// <summary>创建带新数据库路径的连接</summary>
        private static ISqlServerConnection CreateConnection(RelationalConnectionDependencies dependencies, ISqlServerConnection connection, DbConnectionOptions connectionOptions)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString)
            {
                AttachDBFilename = connectionOptions.NewDatabasePath
            };
            var options = new DbContextOptionsBuilder().UseSqlServer(sqlConnectionStringBuilder.ConnectionString, b =>
            {
                b.CommandTimeout(connection.CommandTimeout ?? 60);
            }).Options;
            return new SqlServerConnection(dependencies.With(options));
        }
    }
}