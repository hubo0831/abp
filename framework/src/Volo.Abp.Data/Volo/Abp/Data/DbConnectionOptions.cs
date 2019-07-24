namespace Volo.Abp.Data
{
    public class DbConnectionOptions
    {
        /// <summary>
        /// 是否每个租户使用独立数据库
        /// </summary>
        public bool UseDatabasePerTenant { get; set; }
        /// <summary>
        /// 连接串字典
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; set; }

        public DbConnectionOptions()
        {
            UseDatabasePerTenant = true;
            ConnectionStrings = new ConnectionStrings();
        }
    }
}
