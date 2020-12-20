using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.DataLayer.Repository
{
    public class DictionaryRepository : ConnectionBase, IDictionaryRepository
    {
        public DictionaryRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesAllAsync()
        {
            return await Connection.QueryAsync<Technology>("select * from napp.get_Technologies_All()");
        }

        public IEnumerable<Technology> GetTechnologiesAll()
        {
            return Connection.Query<Technology>("select * from napp.get_Technologies_All()");
        }

        public async Task<IEnumerable<WorkDirection>> GetWorkDirectionsAllAsync()
        {
            return await Connection.QueryAsync<WorkDirection>("select * from napp.get_WorkDirections_All()");
        }

        public IEnumerable<WorkDirection> GetWorkDirectionsAll()
        {
            return Connection.Query<WorkDirection>("select * from napp.get_WorkDirections_All()");
        }
    }
}
