using System.Collections.Generic;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Dictionary.repo;

namespace MatchingSystem.DataLayer.Feature._Repository
{

    public class DictionaryRepositoryDapper : ConnectionBase, IDictionaryRepository
    {
        public DictionaryRepositoryDapper(string connectionString) : base(connectionString)
        {
        }
        

        public IEnumerable<TechnologyDto> GetTechnologiesAll()
        {
            return Connection.Query<TechnologyDto>("select * from napp.get_Technologies_All()");
        }
        
        public IEnumerable<WorkDirectionDto> GetWorkDirectionsAll()
        {
            return Connection.Query<WorkDirectionDto>("select * from napp.get_WorkDirections_All()");
        }
    }
}
