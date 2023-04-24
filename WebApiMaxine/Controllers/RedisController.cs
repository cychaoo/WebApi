using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiMaxine.Interface;
using StackExchange.Redis;

namespace WebApiMaxine.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisManageService _redisService;
        private readonly ILogger<RedisController> _logger;

        public RedisController(IRedisManageService redisService, ILogger<RedisController> logger)
        {
            _redisService = redisService;
            _logger = logger;
        }
        /// <summary>
        /// Set
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Set")]
        public IActionResult Set(string value)
        {
            _redisService.Set("TESTONLY", value, TimeSpan.FromHours(2));
            var getValue = _redisService.GetValue("TESTONLY");
            return Ok(getValue);
        }
    }
}
