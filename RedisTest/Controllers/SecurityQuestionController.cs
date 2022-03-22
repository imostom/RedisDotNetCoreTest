using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisTest.Services;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisTest.Controllers
{
    [Route("api/redistest")]
    [ApiController]
    public class SecurityQuestionController : ControllerBase
    {
        //private readonly IConnectionMultiplexer _redis;
        private readonly IRedisTestService _testService;
        private readonly CacheProvider _cacheProvider;

        //public SecurityQuestionController(IConnectionMultiplexer redis, IRedisTestService testService, CacheProvider cacheProvider)
        public SecurityQuestionController(IRedisTestService testService, CacheProvider cacheProvider)
        {
            //_redis = redis;
            _testService = testService;
            _cacheProvider= cacheProvider;
        }

        [HttpGet]
        [Route("getsecurityquestions")]
        public async Task<IActionResult> GetBodyCategories()
        {
            var resp = await _testService.GetSecurityQuestions();
            return Ok(resp);
        }
    }
}
