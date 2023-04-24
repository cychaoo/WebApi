namespace WebApiMaxine.DTO;

public class RedisConfigDTO
{
    public string Name { get; set; } = string.Empty;
    public string Ip { get; set; }  = string.Empty;
    public int Port { get; set; }
    public string Password { get; set; } = string.Empty;
    public int Timeout { get; set; } 
    public int Db { get; set; }
}
