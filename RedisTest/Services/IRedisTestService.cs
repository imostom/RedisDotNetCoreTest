using System.Threading.Tasks;

namespace RedisTest.Services
{
    public interface IRedisTestService
    {
        Task<object> GetSecurityQuestions();
    }
}
