namespace Volo.Abp.Data
{
    public class AbpDbConnectionOptions
    {
        public bool UseDatabasePerTenant { get; set; }
        public string NewDatabasePath { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }

        public AbpDbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStrings();
        }
    }
}
