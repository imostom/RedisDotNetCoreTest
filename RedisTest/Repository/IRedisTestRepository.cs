using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisTest.Repository
{
    public interface IRedisTestRepository
    {
        Task<List<string>> GetSecurityQuestions();
    }
}
