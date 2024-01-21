using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Dictionary.repo;

public class DictionaryRepository : GenericRepository<TechnologyEntity>, IDictionaryRepository
{
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

    public IEnumerable<WorkDirectionDto> GetWorkDirectionsAll()
    {
        var workDirectionList = _matchingSystemContext.WorkDirections.ToList();
        return workDirectionList.Select(info => {
            WorkDirectionDto dto = new WorkDirectionDto();

            dto.DirectionCode = info.DirectionCode;
            dto.DirectionName_ru = info.DirectionNameRu;

            return dto;
        });
    }
}