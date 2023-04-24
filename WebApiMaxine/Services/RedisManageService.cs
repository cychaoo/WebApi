using WebApiMaxine.Interface;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using WebApiMaxine.DTO;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace WebApiMaxine.Services
{
    public class RedisManageService: IRedisManageService
    {
        public volatile ConnectionMultiplexer _redisConnection;
        private readonly object _redisConnectionLock = new object();
        private readonly ConfigurationOptions _configOptions;
        private readonly ILogger<RedisManageService> _logger;
        private readonly RedisConfigDTO _redisConfig;


        public RedisManageService(ILogger<RedisManageService> logger, IOptions<RedisConfigDTO> redisConfig)
        {
            _logger = logger;
            _redisConfig = redisConfig.Value;
            ConfigurationOptions options = ReadRedisSetting(); //連線參數
            if (options == null)
            {
                _logger.LogError("RedisSetting有誤");
            }
            _configOptions = options;
            _redisConnection = ConnectionRedis();
            
        }

        private ConnectionMultiplexer ConnectionRedis()
        {
            if (_redisConnection != null && _redisConnection.IsConnected)
            {
                return _redisConnection; // 已有連接，直接使用
            }
            lock (_redisConnectionLock)
            {
                if (_redisConnection != null)
                {
                    _redisConnection.Dispose(); 
                }
                try
                {
                    _redisConnection = ConnectionMultiplexer.Connect(_configOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Redis啟動失敗：{ex.Message}");
                }
            }
            return _redisConnection;
        }

        private ConfigurationOptions ReadRedisSetting()
        {
            try
            {
                if (_redisConfig.Name.Any())
                {
                    ConfigurationOptions options = new ConfigurationOptions
                    {
                        EndPoints =
                            {
                                {
                                    _redisConfig.Ip,
                                    _redisConfig.Port
                                }
                            },
                        ClientName = _redisConfig.Name,
                        Password = _redisConfig.Password,
                        ConnectTimeout = _redisConfig.Timeout,
                        DefaultDatabase = _redisConfig.Db,
                    };
                    return options;
                }
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Get Redis setting error：{ex.Message}");
                return null;
            }
        }
        public string GetValue(string key)
        {
            return _redisConnection.GetDatabase().StringGet(key);
        }

        public void Set(string key, object value, TimeSpan ts)
        {
            if(value != null)
            {
                _redisConnection.GetDatabase().StringSet(key, JsonConvert.SerializeObject(value), ts);
            }
        }
        public bool Get(string key)
        {
            return _redisConnection.GetDatabase().KeyExists(key);
        }

        public bool Delete(string key)
        {
            return _redisConnection.GetDatabase().KeyDelete(key);
        }
    }
}
