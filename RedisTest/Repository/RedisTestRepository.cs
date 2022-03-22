using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RedisTest.Repository
{
    public class RedisTestRepository : IRedisTestRepository
    {
        private readonly IDapperGenericRepository _dapperGenericRepository;

        public RedisTestRepository(IDapperGenericRepository dapperGenericRepository)
        {
            _dapperGenericRepository = dapperGenericRepository;
        }


        public async Task<List<string>> GetSecurityQuestions()
        {
            var queryString = @"SELECT Question FROM SecurityQuestion WITH(NOLOCK)";

            var dbPara = new DynamicParameters();

            var response = await _dapperGenericRepository.GetAll<string>(queryString, dbPara, commandType: CommandType.Text);

            return response;
        }
    }
}
