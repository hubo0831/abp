namespace Volo.Abp.Data
{
    public class DbConnectionOptions
    {
        /// <summary>
        /// 是否每个租户使用独立数据库
        /// </summary>
        /// <remarks>使用共享数据库时应为False,能提高性能</remarks>
        public bool UseDatabasePerTenant { get; set; }
        /// <summary>
        /// 连接串字典
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; set; }
        /// <summary>
        /// 新数据库路径(用于创建数据库)
        /// </summary>
        public string NewDatabasePath { get; set; }

        public DbConnectionOptions()
        {
            UseDatabasePerTenant = true;
            ConnectionStrings = new ConnectionStrings();
        }
    }
}
