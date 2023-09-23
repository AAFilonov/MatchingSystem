using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

public class DictionaryRepository : Repository<Technology>, IDictionaryRepository
{

    public DictionaryRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<TechnologyDto> GetTechnologiesAll()
    {
        var technologyList = findAll();
        return technologyList.Select(info =>
        {
            TechnologyDto dto = new TechnologyDto();

            dto.TechnologyCode = info.TechnologyCode;
            dto.TechnologyName_ru = info.TechnologyNameRu;

            return dto;
        });
    }

    public IEnumerable<MatchingSystem.DataLayer.OldEntities.WorkDirection> GetWorkDirectionsAll()
    {
        throw new System.NotImplementedException();
    }
}