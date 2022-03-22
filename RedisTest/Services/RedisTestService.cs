using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RedisTest.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace RedisTest.Services
{
    public class RedisTestService : IRedisTestService
    {
        private readonly IRedisTestRepository _redisTestRepository;
        private readonly CacheProvider _cacheProvider;

        public RedisTestService(IRedisTestRepository redisTestRepository, CacheProvider cacheProvider)
        {
            _redisTestRepository = redisTestRepository;
            _cacheProvider = cacheProvider;
        }


        public async Task<object> GetSecurityQuestions()
        {
            string serviceName = "GetSecurityQuestions";
            try
            {
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(4));

                var cachedData = await _cacheProvider.GetFromCache<List<string>>(serviceName);

                if (cachedData is null)
                {
                    var questions = await _redisTestRepository.GetSecurityQuestions();
                    if (questions is null)
                        return null;


                    //cache response
                    await _cacheProvider.SetCache<List<string>>(serviceName, questions, options);

                    return questions;
                }

                return cachedData;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
    }
}
