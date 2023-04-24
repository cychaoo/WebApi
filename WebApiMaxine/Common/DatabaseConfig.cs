namespace WebApiMaxine.Common;

public class DatabaseConfig
{
    public ICollection<DatabaseConfigDetail> Details { get; set; } = new List<DatabaseConfigDetail>();
}
public class DatabaseConfigDetail
{
    public DBName DbName { get; set; }

    public string ConnectionString { get; set; } = string.Empty;
}
