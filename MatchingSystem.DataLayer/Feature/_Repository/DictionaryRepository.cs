using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature._Repository
{
    //TODO DPL-18 переписать на EF
    public class DictionaryRepository : ConnectionBase, IDictionaryRepository
    {
        public DictionaryRepository(string connectionString) : base(connectionString)
        {
        }
        

        public IEnumerable<TechnologyDto> GetTechnologiesAll()
        {
            return Connection.Query<TechnologyDto>("select * from napp.get_Technologies_All()");
        }
        
        public IEnumerable<WorkDirection> GetWorkDirectionsAll()
        {
            return Connection.Query<WorkDirection>("select * from napp.get_WorkDirections_All()");
        }
    }
}
